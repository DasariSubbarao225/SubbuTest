using System.Text;

namespace SharePointLargeListApp.Utilities
{
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public Logger(string logDirectory = "Logs")
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string fileName = $"SPLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            _logFilePath = Path.Combine(logDirectory, fileName);
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            lock (_lockObject)
            {
                try
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging failed: {ex.Message}");
                }
            }
        }

        public void LogError(string message, Exception? exception = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(message);

            if (exception != null)
            {
                sb.AppendLine($"Exception: {exception.GetType().Name}");
                sb.AppendLine($"Message: {exception.Message}");
                sb.AppendLine($"StackTrace: {exception.StackTrace}");

                if (exception.InnerException != null)
                {
                    sb.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                }
            }

            Log(sb.ToString(), LogLevel.Error);
        }

        public string GetLogFilePath() => _logFilePath;
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Success
    }
}
