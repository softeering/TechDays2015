using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.SCP;
using System.Reflection;
using Microsoft.SCP.SCPLogger.Config;
using log4net.Config;
using StreamingDataProcessing.Spout;
using StreamingDataProcessing.Bolt;

//[assembly: SCPLogger("SCPSample.log.Log4netLoggerFactory")]
namespace StreamingDataProcessing
{	
    class Program
    {
        static void Main(string[] args)
        {
			XmlConfigurator.Configure();

			if (args.Count() > 0) //Code to run on HDInsight cluster will go there
			{
				//The component to run
				string compName = args[0];
				//Run the component
				if ("dataloadspout".Equals(compName))
				{
					//Set the prefix for logging
					System.Environment.SetEnvironmentVariable("microsoft.scp.logPrefix", "SCPSample-Spout");
					SCPRuntime.Initialize();  //Initialize the runtime
					SCPRuntime.LaunchPlugin(new newSCPPlugin(DataLoadSpout.Get)); //Run the plugin
				}
				else if ("deserializerbolt".Equals(compName))
				{
					System.Environment.SetEnvironmentVariable("microsoft.scp.logPrefix", "SCPSample-DeserializerBolt");
					SCPRuntime.Initialize();
					SCPRuntime.LaunchPlugin(new newSCPPlugin(DeserializerBolt.Get));
				}
				else if ("averagebolt".Equals(compName))
				{
					System.Environment.SetEnvironmentVariable("microsoft.scp.logPrefix", "SCPSample-AverageBolt");
					SCPRuntime.Initialize();
					SCPRuntime.LaunchPlugin(new newSCPPlugin(AverageBolt.Get));
				}
				else if ("sumbolt".Equals(compName))
				{
					System.Environment.SetEnvironmentVariable("microsoft.scp.logPrefix", "SCPSample-SumBolt");
					SCPRuntime.Initialize();
					SCPRuntime.LaunchPlugin(new newSCPPlugin(SumBolt.Get));
				}
				else
				{
					throw new Exception(string.Format("unexpected compName: {0}", compName));
				}
			}
			else // Local Testing
			{
				System.Environment.SetEnvironmentVariable("microsoft.scp.logPrefix", "SCPSample-LocalTest");
				SCPRuntime.Initialize();

				if (Context.pluginType != SCPPluginType.SCP_NET_LOCAL)
				{
					throw new Exception(string.Format("unexpected pluginType: {0}", Context.pluginType));
				}
				LocalTest localTest = new LocalTest();
				localTest.RunTestCase();


				Console.ReadKey();
			}
        }
    }
}
