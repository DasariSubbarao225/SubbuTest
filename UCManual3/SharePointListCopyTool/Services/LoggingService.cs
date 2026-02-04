using System;
using NLog;

namespace SharePointListCopyTool.Services
{
    public class LoggingService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void LogWarning(string message)
        {
            logger.Warn(message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                logger.Error(ex, message);
            }
            else
            {
                logger.Error(message);
            }
        }

        public static void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public static void LogException(Exception ex, string context = "")
        {
            string message = string.IsNullOrEmpty(context)
                ? $"Exception occurred: {ex.Message}"
                : $"Exception occurred in {context}: {ex.Message}";

            logger.Error(ex, message);
        }
    }
}
