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
	class AverageBolt : ISCPBolt
	{
		private Context ctx;

		private int _numberOfCalls = 0;
		private long _durationSum = 0;

		//Constructor
		public AverageBolt(Context ctx)
		{
			Context.Logger.Info("AverageBolt constructor called");

			this.ctx = ctx;

			//Define the schema for the incoming tuples from spout
			var inputSchema = new Dictionary<string, List<Type>>();
			inputSchema.Add("default", new List<Type>() { typeof(Statistic) });

			//Define the schema for tuples to be emitted from this bolt
			var outputSchema = new Dictionary<string, List<Type>>();
			outputSchema.Add("default", new List<Type>() { typeof(double) });

			this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
		}

		//Get an instance
		public static AverageBolt Get(Context ctx, Dictionary<string, Object> parms)
		{
			return new AverageBolt(ctx);
		}

		//Process a tuple from the stream
		public void Execute(SCPTuple tuple)
		{
			Context.Logger.Info("Execute enter");

			//Get the word that was emitted from the splitter
			var stat = tuple.GetValue(0) as Statistic;

			var calls = stat.NumberOfCalls.HasValue ? stat.NumberOfCalls.Value : 0;
			var duration = stat.DurationAverage.HasValue ? stat.DurationAverage.Value : 0;

			this._numberOfCalls += calls;
			this._durationSum += duration * calls;

			double res = 0;
			if (this._durationSum > 0)
				res = this._durationSum / this._numberOfCalls;

			Context.Logger.Info("Emit: duration average: {0}", res);

			//Emit the word and the count
			//This bolt emits a stream, which is useful for testing. In a real world solution, you would store the data to a database, queue, 
			//or other persistent store at the end of processing.
			this.ctx.Emit(Constants.DEFAULT_STREAM_ID, new List<SCPTuple> { tuple }, new Values(res));

			Context.Logger.Info("Execute exit");
		}
	}
}