using System;
using System.Configuration;

namespace SharePointListCopyTool.Models
{
    public class AppConfig
    {
        public static int BatchSize
        {
            get
            {
                string value = ConfigurationManager.AppSettings["BatchSize"];
                return int.TryParse(value, out int result) ? result : 500;
            }
        }

        public static int RetryAttempts
        {
            get
            {
                string value = ConfigurationManager.AppSettings["RetryAttempts"];
                return int.TryParse(value, out int result) ? result : 3;
            }
        }

        public static int RetryDelaySeconds
        {
            get
            {
                string value = ConfigurationManager.AppSettings["RetryDelaySeconds"];
                return int.TryParse(value, out int result) ? result : 5;
            }
        }

        public static int RequestTimeout
        {
            get
            {
                string value = ConfigurationManager.AppSettings["RequestTimeout"];
                return int.TryParse(value, out int result) ? result : 180000;
            }
        }

        public static string LogFilePath
        {
            get { return ConfigurationManager.AppSettings["LogFilePath"] ?? "Logs\\SharePointCopyTool.log"; }
        }

        public static string LogLevel
        {
            get { return ConfigurationManager.AppSettings["LogLevel"] ?? "Info"; }
        }
    }
}
