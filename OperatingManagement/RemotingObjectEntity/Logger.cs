using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace OperatingManagement.RemotingObjectEntity
{
    public class Logger
    {
        private static string loggerName = "OMServer.Logging";
        private static ILog mLogger = null;

        public static ILog GetLogger()
        {
            if (mLogger == null)
                mLogger = LogManager.GetLogger(loggerName);
            return mLogger;
        }

        public static void Debug(string msg)
        {
            GetLogger().Debug(msg + "——");
        }

        public static void Info(string msg)
        {
            GetLogger().Info(msg + "——");
        }

        public static void Error(string msg)
        {
            GetLogger().Error(msg + "——");
        }

        public static void Error(string msg, Exception ex)
        {
            GetLogger().Error(msg + "——", ex);
        }
    }
}
