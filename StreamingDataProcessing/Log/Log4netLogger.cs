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
	public class Log4netLogger : LoggerBase
	{
		private readonly ILog _logger;

		public Log4netLogger(ILog logger)
		{
			this._logger = logger;
		}

		public override void Log(LogLevel level, string title, string message, Exception ex)
		{
			if (this._logger != null)
			{
				switch (level) { 
					case LogLevel.Debug:
						this._logger.Debug(string.Format("{0}:{1}", title, message), ex);
						break;
					case LogLevel.Error:
						this._logger.Error(string.Format("{0}:{1}", title, message), ex);
						break;
					case LogLevel.Fatal:
						this._logger.Fatal(string.Format("{0}:{1}", title, message), ex);
						break;
					case LogLevel.Info:
						this._logger.Info(string.Format("{0}:{1}", title, message), ex);
						break;
					case LogLevel.All:
						this._logger.Info(string.Format("{0}:{1}", title, message), ex);
						break;
				}
			}
		}
		public override bool IsLogEnabled(LogLevel level)
		{
			return true;
		}
	}
}
