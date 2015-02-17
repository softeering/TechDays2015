using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPL.DataFlow.Processor.TestService
{
	internal class Helpers
	{
		internal static async Task<IEnumerable<HotelModel>> GetHotelKeysAsync(string connectionString, int numberOfHotels)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("connectionString");

			List<HotelModel> hotels = new List<HotelModel>();

			using (var connection = new SqlConnection(connectionString))
			{
				await connection.OpenAsync();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = string.Format("SELECT TOP {0} HotelKey, Latitude, Longitude FROM dimhotelexpand WHERE ISNULL(Latitude, 0) > 0 AND ISNULL(Longitude, 0) > 0 ", numberOfHotels);

					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							hotels.Add(new HotelModel
							{
								HotelKey = reader.GetInt32(0),
								Latitude = reader.GetDouble(1),
								Longitude = reader.GetDouble(2)
							});
						}
					}

				}
			}

			return hotels;
		}

	}
}
