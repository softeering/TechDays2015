using log4net;
using Microsoft.SCP.SCPLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StreamingDataProcessing.log
{
	public class Log4netLoggerFactory : ILoggerFactory
	{
		public ILogger GetLogger(string name)
		{
			var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			return new Log4netLogger(log);
		}
	}
}
