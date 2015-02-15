using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json.Linq;
using TPL.DataFlow.Processor.Core;

namespace TPL.DataFlow.Processor.TestService
{
	public sealed class TestServiceImplParallelForeachLoop
	{
		public TestServiceImplParallelForeachLoop(JObject config)
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
		private object LOCK = new object();

		public async Task ProcessAsync()
		{
			string fileName = "hotelsparallelforeachloop.txt";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var hotels = await Helpers.GetHotelKeysAsync(this._connectionString, this._numberOfHotels);

			Parallel.ForEach(hotels, new ParallelOptions() { MaxDegreeOfParallelism = this._degreeOfParallelism }, item =>
			{
				using (var client = new HttpClient())
				{
					Thread.Sleep(200);
					item.Country = "Switzerland";
				}

				lock (LOCK)
				{
					using (var sw = new StreamWriter(fileName, true))
					{
						sw.WriteLine(item.ToString());
					}
				}
			});
		}

	}
}
