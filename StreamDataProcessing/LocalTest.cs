using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.SCP;
using StreamDataProcessing.Spout;
using StreamDataProcessing.Bolt;

namespace StreamDataProcessing
{
	class LocalTest
	{
		public void RunTestCase()
		{
			// for this test we'll use pplain text files 
			// In HDInsight we can connect to a message bus.

			Dictionary<string, Object> emptyDictionary = new Dictionary<string, object>();

			{
				var spoutCtx = LocalContext.Get(); //Get local context
				var spout = DataLoadSpout.Get(spoutCtx, emptyDictionary); //Get an instance of the spout

				spout.NextTuple(emptyDictionary);

				//Store the stream for the next component
				spoutCtx.WriteMsgQueueToFile("spout.txt");
			}

			{
				var deserializerCtx = LocalContext.Get();
				var deserializer = DeserializerBolt.Get(deserializerCtx, emptyDictionary);

				//Read from the 'stream' emitted by the spout
				deserializerCtx.ReadFromFileToMsgQueue("spout.txt");
				var batch = deserializerCtx.RecvFromMsgQueue();

				foreach (var tuple in batch)
				{
					deserializer.Execute(tuple);
				}

				//store the stream for the next component
				deserializerCtx.WriteMsgQueueToFile("deserializer.txt");
			}

			{
				var sumCtx = LocalContext.Get();
				var sum = SumBolt.Get(sumCtx, emptyDictionary);

				//Read from the 'stream' emitted by the spout
				sumCtx.ReadFromFileToMsgQueue("deserializer.txt");
				var batch = sumCtx.RecvFromMsgQueue();

				foreach (var tuple in batch)
				{
					sum.Execute(tuple);
				}

				//store the stream for the next component
				sumCtx.WriteMsgQueueToFile("sum.txt");
			}

			{
				var avgCtx = LocalContext.Get();
				var avg = AverageBolt.Get(avgCtx, emptyDictionary);

				//Read from the 'stream' emitted by the spout
				avgCtx.ReadFromFileToMsgQueue("deserializer.txt");
				var batch = avgCtx.RecvFromMsgQueue();

				foreach (var tuple in batch)
				{
					avg.Execute(tuple);
				}

				//store the stream for the next component
				avgCtx.WriteMsgQueueToFile("avg.txt");
			}
		}
	}
}
