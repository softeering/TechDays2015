using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json.Linq;
using TPL.DataFlow.Processor.TestService;

namespace TPL.DataFlow.Processor
{
	class DataflowPalindromes
	{
		// http://blog.stephencleary.com/2012/09/introduction-to-dataflow-part-2.html
		static void Main(string[] args)
		{
			dynamic config = new JObject();
			config.ConnectionString = "server=chcxssatech006; database=GPCMaster; user id=tempuser; password=toto123-";
			config.DegreeOfParallelism = 10;
			config.NumberOfHotels = 200;

			var tplService = new TestServiceImpl(config);

			Stopwatch sw = Stopwatch.StartNew();
			tplService.ProcessAsync().Wait();
			sw.Stop();
			Console.WriteLine("TPL completed: {0} ms", sw.ElapsedMilliseconds);

			var foreachLoopService = new TestServiceImplForeachLoop(config);
			sw.Restart();
			//foreachLoopService.ProcessAsync().Wait();
			sw.Stop();
			Console.WriteLine("Foreach loop completed: {0} ms", sw.ElapsedMilliseconds);

			var parallelForeachLoop = new TestServiceImplParallelForeachLoop(config);
			sw.Restart();
			parallelForeachLoop.ProcessAsync().Wait();
			sw.Stop();
			Console.WriteLine("Parallel Foreach loop completed: {0} ms", sw.ElapsedMilliseconds);

			Console.WriteLine("Completed...");
			Console.ReadLine();
		}
	}
}
