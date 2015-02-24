using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataInit
{
	class Program
	{
		static void Main(string[] args)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;
			var numberOfRows = 1000000;

			DataTable data = new DataTable();
			data.Columns.Add("SubMarketID", typeof(int));
			data.Columns.Add("Dec1", typeof(decimal));

			var rand = new Random();

			for (int i = 0; i < numberOfRows; i++)
			{
				var d = (decimal)rand.NextDouble();
				data.Rows.Add(rand.Next(1,1000), ((decimal)(i + 1) * 10000 / d == 0 ? (decimal)0.1 : d));
			}

			using (var connection = new SqlConnection(connectionString))
			{
				connection.DumpTable(data, "CSIndex", true).Wait();
			}

			Console.WriteLine("Completed...");
			Console.Read();
		}
	}
}
