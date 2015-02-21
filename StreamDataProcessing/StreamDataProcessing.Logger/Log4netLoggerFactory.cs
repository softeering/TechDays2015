using log4net;
using Microsoft.SCP.SCPLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StreamDataProcessing.Logger
{
	public class Log4netLoggerFactory : ILoggerFactory
	{
		public ILogger GetLogger(string name)
		{
			return new Log4netLogger(LogManager.GetLogger(name));
		}
	}
}
