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
			data.Columns.Add("HotelKey", typeof(int));
			data.Columns.Add("SubMarket", typeof(string));
			data.Columns.Add("Int1", typeof(int));
			data.Columns.Add("Dec1", typeof(decimal));
			data.Columns.Add("BigInt1", typeof(int));

			for (int i = 0; i < numberOfRows; i++)
			{
				var rand = new Random();
				var d = (decimal)rand.NextDouble();
				data.Rows.Add(i, string.Format("SubMarket{0}", rand.Next(1, 100)), i,
					((decimal)(i + 1) * 10000 / d == 0 ? (decimal)0.1 : d), i * 10000);
			}

			using (var connection = new SqlConnection(connectionString))
			{
				connection.DumpTable(data, "WithoutCSIndex", true).Wait();
				connection.DumpTable(data, "WithCSIndex", true).Wait();
			}

			Console.WriteLine("Completed...");
			Console.Read();
		}
	}
}
