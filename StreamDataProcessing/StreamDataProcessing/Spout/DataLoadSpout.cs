using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.SCP;
using Microsoft.SCP.Rpc.Generated;
using Newtonsoft.Json.Linq;
using System.Net;

namespace StreamDataProcessing.Spout
{
	public class DataLoadSpout : ISCPSpout
	{
		private Context ctx;

		//Constructor
		public DataLoadSpout(Context ctx)
		{
			Context.Logger.Info("DataLoadSpout constructor called");

			this.ctx = ctx;

			//Define the schema for the emitted tuples
			var outputSchema = new Dictionary<string, List<Type>>();
			outputSchema.Add("default", new List<Type>() { typeof(string) });

			//Declare the schema for the stream
			this.ctx.DeclareComponentSchema(new ComponentStreamSchema(null, outputSchema));
		}

		//Return a new instance of the spout
		public static DataLoadSpout Get(Context ctx, Dictionary<string, Object> parms)
		{
			return new DataLoadSpout(ctx);
		}

		//Emit the next tuple
		//NOTE: When using data from an external data source
		//such as Service Bus, Event Hub, Twitter, etc.,
		//you would read and emit it in NextTuple
		public void NextTuple(Dictionary<string, object> parms)
		{
			Context.Logger.Info("NextTuple enter");

			var res = new WebClient().DownloadString("http://dynamo/odata/DynamoStatistic?$format=json&$orderby=DataSet");
	
			Context.Logger.Info("Emit: {0}", res);

			//Emit the sentence
			this.ctx.Emit(new Values(res));

			Context.Logger.Info("NextTuple exit");
		}

		//Ack's are not implemented
		public void Ack(long seqId, Dictionary<string, object> parms)
		{
			throw new NotImplementedException();
		}

		//Ack's are not implemented, so
		//fail should never be called
		public void Fail(long seqId, Dictionary<string, object> parms)
		{
			throw new NotImplementedException();
		}

	}
}