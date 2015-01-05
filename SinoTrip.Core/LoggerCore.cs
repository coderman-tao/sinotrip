using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Core
{
    public abstract class LoggerCore
    {
        private static readonly ILog logError = LogManager.GetLogger("logerror");

        private static readonly ILog logInfo = LogManager.GetLogger("loginfo");
        private static readonly ILog logDebug = LogManager.GetLogger("logdebug");
        private static readonly ILog logWarn = LogManager.GetLogger("logwarn");
        private static readonly ILog logFatal = LogManager.GetLogger("logfatal");

        public static void Error(string message)
        {
            if (logError.IsErrorEnabled)
            {
                logError.Error(message);
            }

        }

        public static void Error(string message, Exception ex)
        {
            if (logError.IsErrorEnabled)
            {
                string msg = message.Trim();
                logError.Error(message, ex);
            }
        }

        public static void Debug(string message)
        {

            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(message);
            }

        }

        public static void Debug(string message, Exception ex)
        {

            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(message, ex);
            }

        }
        /// <summary>
        /// 严重错误
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {

            if (logFatal.IsFatalEnabled)
            {
                logFatal.Fatal(message);
            }
        }

        public static void Fatal(string message, Exception ex)
        {

            if (logFatal.IsFatalEnabled)
            {
                logFatal.Fatal(message, ex);
            }
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            if (logInfo.IsInfoEnabled)
            {
                logInfo.Info(message);
            }
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            if (logWarn.IsWarnEnabled)
            {
                logWarn.Warn(message);
            }
        }

        public static void Warn(string message, Exception ex)
        {
            if (logWarn.IsWarnEnabled)
            {
                logWarn.Warn(message, ex);
            }
        }
    }
}
