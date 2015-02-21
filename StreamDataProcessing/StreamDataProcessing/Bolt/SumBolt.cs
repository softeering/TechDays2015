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

namespace StreamDataProcessing.Bolt
{
	class SumBolt : ISCPBolt
	{
		private Context ctx;

		//Store each word and a count of how many times it has
		//been emitted by the splitter
		private int _count = 0;

		//Constructor
		public SumBolt(Context ctx)
		{
			Context.Logger.Info("SumBolt constructor called");

			this.ctx = ctx;

			//Define the schema for the incoming tuples from spout
			var inputSchema = new Dictionary<string, List<Type>>();
			inputSchema.Add("default", new List<Type>() { typeof(Statistic) });

			//Define the schema for tuples to be emitted from this bolt
			var outputSchema = new Dictionary<string, List<Type>>();
			outputSchema.Add("default", new List<Type>() { typeof(int) });

			this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
		}

		//Get an instance
		public static SumBolt Get(Context ctx, Dictionary<string, Object> parms)
		{
			return new SumBolt(ctx);
		}

		//Process a tuple from the stream
		public void Execute(SCPTuple tuple)
		{
			Context.Logger.Info("Execute enter");

			//Get the word that was emitted from the splitter
			var stat = tuple.GetValue(0) as Statistic;
			this._count += stat.NumberOfCalls.HasValue ? stat.NumberOfCalls.Value : 0;
			Context.Logger.Info("Emit: count: {0}", this._count);

			//Emit the word and the count
			//This bolt emits a stream, which is useful for testing. In a real world solution, you would store the data to a database, queue, 
			//or other persistent store at the end of processing.
			this.ctx.Emit(Constants.DEFAULT_STREAM_ID, new List<SCPTuple> { tuple }, new Values(this._count));

			Context.Logger.Info("Execute exit");
		}
	}
}