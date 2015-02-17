using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json.Linq;
using TPL.DataFlow.Processor.Core;

namespace TPL.DataFlow.Processor.TestService
{
	public sealed class TestServiceImplForeachLoop
	{
		public TestServiceImplForeachLoop(JObject config)
		{
			this.LoadConfig(config);
		}

		private void LoadConfig(JObject config)
		{
			this._connectionString = config.Value<string>("ConnectionString");
			this._degreeOfParallelism = config.Value<int>("DegreeOfParallelism");
			this._numberOfHotels = config.Value<int>("NumberOfHotels");
		}

		private string _connectionString;
		private int _degreeOfParallelism;
		private int _numberOfHotels;

		public async Task ProcessAsync()
		{
			string fileName = "hotelsforeachloop.txt";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var hotels = await Helpers.GetHotelKeysAsync(this._connectionString, this._numberOfHotels);

			foreach (var item in hotels)
			{
				using (var client = new HttpClient())
				{
					await Task.Delay(200);
					item.Country = "Switzerland";
				}

				using (var sw = new StreamWriter(fileName, true))
				{
					await sw.WriteLineAsync(item.ToString());
				}
			}
		}

	}
}
