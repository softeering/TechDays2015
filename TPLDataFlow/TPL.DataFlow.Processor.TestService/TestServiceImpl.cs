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
	public sealed class TestServiceImpl : ProcessorBase<HotelModel>
	{
		public TestServiceImpl(JObject config)
			: base(config)
		{
		}

		protected override void AddAdditionalBlock()
		{
			string fileName = "hotels.txt";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var geoCodingBlock = new TransformBlock<HotelModel, HotelModel>(async item =>
			{
				using (var client = new HttpClient())
				{
					// mimik the call to a web service
					await Task.Delay(200);
					item.Country = "Switzerland";
				}

				return item;
			}, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = this._degreeOfParallelism });

			base.RegisterAdditionalBlock(geoCodingBlock);
			this.StartBlockInstance.LinkTo(geoCodingBlock);

			var saveBlock = new ActionBlock<HotelModel>(async item =>
			{
				using (var sw = new StreamWriter(fileName, true))
				{
					await sw.WriteLineAsync(item.ToString());
				}
			}, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

			base.RegisterAdditionalBlock(saveBlock);
			geoCodingBlock.LinkTo(saveBlock);
		}

		protected override void LoadConfig(JObject config)
		{
			this._connectionString = config.Value<string>("ConnectionString");
			this._degreeOfParallelism = config.Value<int>("DegreeOfParallelism");
			this._numberOfHotels = config.Value<int>("NumberOfHotels");
		}

		private string _connectionString;
		private int _degreeOfParallelism;
		private int _numberOfHotels;

		protected async override Task<BufferBlock<HotelModel>> GetItemsToProcess()
		{
			var buffer = new BufferBlock<HotelModel>();

			foreach (var item in await Helpers.GetHotelKeysAsync(this._connectionString, this._numberOfHotels))
			{
				buffer.Post(item);
			}

			return buffer;
		}
	}
}
