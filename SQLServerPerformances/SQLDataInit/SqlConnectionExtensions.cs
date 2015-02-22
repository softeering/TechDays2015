using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace System.Data.SqlClient
{
	public static class SqlConnectionExtensions
	{
		public static async Task<DataTableCacheResult> DumpTable(this SqlConnection connection, DataTable source, string tableName = null, bool truncateBeforeLoad = false)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");

			if (source == null)
				throw new ArgumentNullException("source");

			if (source.Columns == null || source.Columns.Count == 0)
				throw new ArgumentException("Source table must have at least one column");

			if (string.IsNullOrWhiteSpace(tableName))
			{
				tableName = source.TableName;
				if (string.IsNullOrWhiteSpace(tableName))
					tableName = Guid.NewGuid().ToString("N");
			}

			bool selfOpened = false;

			if (connection.State != ConnectionState.Open)
			{
				await connection.OpenAsync().ConfigureAwait(false);
				selfOpened = true;
			}

			var result = new DataTableCacheResult();
			SqlTransaction transaction = null;

			try
			{
				connection.MergeSchema(tableName, source);

				if (source.Rows.Count > 0)
				{
					Stopwatch sw = Stopwatch.StartNew();

					if (truncateBeforeLoad)
					{
						transaction = connection.BeginTransaction();
						// truncate the table in the same transaction
						using (var command = connection.CreateCommand())
						{
							command.Transaction = transaction;
							command.CommandText = string.Format("TRUNCATE TABLE [{0}]", tableName);
							await command.ExecuteNonQueryAsync().ConfigureAwait(false);
						}
					}

					using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
					{
						bulkCopy.BulkCopyTimeout = 300; // 5 minutes
						bulkCopy.DestinationTableName = tableName;

						// be able to use same column name for source and destination
						foreach (DataColumn col in source.Columns)
						{
							bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
						}

						await bulkCopy.WriteToServerAsync(source).ConfigureAwait(false);
					}

					if (transaction != null)
						transaction.Commit();

					sw.Stop();
					result.Ellapsed = sw.Elapsed;
				}

				result.OutputTableName = tableName;
			}
			catch (Exception exc)
			{
				if (transaction != null)
					transaction.Rollback();

				throw exc;
			}
			finally
			{
				if (selfOpened && connection.State == ConnectionState.Open)
					connection.Close();
			}

			return result;
		}

		public static DataTable LoadCachedTable(this SqlConnection connection, string tableName, string whereClause = null)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");

			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentNullException("tableName");

			if (connection.State != ConnectionState.Open)
				connection.Open();

			var query = string.Format("SELECT * FROM [{0}]", tableName);
			if (!string.IsNullOrWhiteSpace(whereClause))
				query = string.Concat(query, " WHERE ", whereClause);

			using (var command = connection.CreateCommand())
			{
				command.CommandText = query;
				using (var adapter = new SqlDataAdapter(command))
				{
					var table = new DataTable(tableName);
					adapter.Fill(table);
					return table;
				}
			}
		}

		public static void MergeSchema(this SqlConnection connection, string targetTableName, DataTable source)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");

			if (source == null)
				throw new ArgumentNullException("source");

			if (source.Columns == null || source.Columns.Count == 0)
				throw new ArgumentException("Source table must have at least one column");

			ServerConnection serverConnection = new ServerConnection(connection);
			Server server = new Server(serverConnection);
			Database database = server.Databases[serverConnection.DatabaseName];
			Table table = database.Tables[targetTableName];

			//if (dropTableIfExists && table != null)
			//{
			//	table.Drop();
			//	table = null;
			//}

			if (table == null)
			{
				// http://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
				table = new Table(database, targetTableName);

				foreach (DataColumn column in source.Columns.OfType<DataColumn>().OrderBy(c => c.Ordinal))
				{
					table.Columns.Add(new Column(table, column.ColumnName, SqlConnectionExtensions.GetDataType(column.DataType, column.MaxLength)));
				}

				table.Create();
			}
			else
			{
				var columns = table.Columns.OfType<Column>();

				foreach (var item in source.Columns.OfType<DataColumn>().OrderBy(c => c.Ordinal))
				{
					var existing = columns.FirstOrDefault(c => c.Name.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase));
					if (existing == null)
					{
						// add the column
						var newColumn = new Column(table, item.ColumnName, SqlConnectionExtensions.GetDataType(item.DataType, item.MaxLength));
						newColumn.Nullable = true;
						newColumn.Create();
					}
					else
					{
						// check if the Column Data Type has changed
						if (!existing.DataType.Name.Equals(GetDataType(item.DataType, item.MaxLength).Name))
						{
							// need to remove the column and recreate it
							existing.Drop();
							// add the column with the good data type
							var newColumn = new Column(table, item.ColumnName, SqlConnectionExtensions.GetDataType(item.DataType, item.MaxLength));
							newColumn.Create();
						}
					}
				}

				columns.Where(c => !source.Columns.OfType<DataColumn>().Any(i => i.ColumnName.Equals(c.Name, StringComparison.OrdinalIgnoreCase))).ToList().ForEach(c => c.Drop());
			}
		}

		internal static DataType GetDataType(Type source, int length)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (source == typeof(bool))
				return DataType.Bit;
			else if (source == typeof(DateTime))
				return DataType.DateTime;
			else if (source == typeof(Guid))
				return DataType.UniqueIdentifier;
			else if (source == typeof(int))
				return DataType.Int;
			else if (source == typeof(string))
				return DataType.NVarCharMax;
			else if (source == typeof(short))
				return DataType.SmallInt;
			else if (source == typeof(long))
				return DataType.BigInt;
			else if (source == typeof(double) || source == typeof(decimal))
				return DataType.Decimal(16, 38);
			else if (source == typeof(float))
				return DataType.Float;

			throw new NotSupportedException();
		}
	}

	public class DataTableCacheResult
	{
		public int RowsCopied { get; set; }
		public TimeSpan Ellapsed { get; set; }
		public string[] ColumnsAdded { get; set; }
		public string[] ColumnsRemoved { get; set; }
		public string OutputTableName { get; set; }
	}

}
