using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.SCP;
using Microsoft.SCP.Rpc.Generated;
using System.Diagnostics;
using SCPSample.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StreamingDataProcessing.Bolt
{
	class DeserializerBolt : ISCPBolt
	{
		private Context ctx;

		public DeserializerBolt(Context ctx)
		{
			Context.Logger.Info("DeserializerBolt constructor called");

			this.ctx = ctx;

			//Define the schema for the incoming tuples from spout
			var inputSchema = new Dictionary<string, List<Type>>();
			inputSchema.Add("default", new List<Type>() { typeof(string) });

			//Define the schema for tuples to be emitted from this bolt
			var outputSchema = new Dictionary<string, List<Type>>();
			outputSchema.Add("default", new List<Type>() { typeof(Statistic) });

			this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
		}

		//Get an instance
		public static DeserializerBolt Get(Context ctx, Dictionary<string, Object> parms)
		{
			return new DeserializerBolt(ctx);
		}

		//Process a tuple from the stream
		public void Execute(SCPTuple tuple)
		{
			Context.Logger.Info("Execute enter");

			var json = tuple.GetString(0);
			var token = JToken.Parse(json);
			var root = token.SelectToken("value");

			var res = JsonConvert.DeserializeObject<List<Statistic>>(root.ToString());

			foreach (var item in res)
			{
				Context.Logger.Info("Emit: {0}", item.DataSet);
				this.ctx.Emit(Constants.DEFAULT_STREAM_ID, new List<SCPTuple> { tuple }, new Values(item));
			}

			Context.Logger.Info("Execute exit");
		}
	}
}