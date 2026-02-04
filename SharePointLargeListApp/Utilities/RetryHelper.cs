namespace SharePointLargeListApp.Utilities
{
    public class RetryHelper
    {
        /// <summary>
        /// Executes an action with exponential backoff retry logic
        /// </summary>
        public static async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            int initialDelayMs = 1000,
            Action<int, Exception>? onRetry = null)
        {
            int retryCount = 0;
            int delay = initialDelayMs;

            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (retryCount < maxRetries && IsTransientError(ex))
                {
                    retryCount++;
                    onRetry?.Invoke(retryCount, ex);

                    // Exponential backoff: 1s, 2s, 4s, 8s, etc.
                    await Task.Delay(delay);
                    delay *= 2;

                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a synchronous action with exponential backoff retry logic
        /// </summary>
        public static void ExecuteWithRetry(
            Action operation,
            int maxRetries = 3,
            int initialDelayMs = 1000,
            Action<int, Exception>? onRetry = null)
        {
            int retryCount = 0;
            int delay = initialDelayMs;

            while (true)
            {
                try
                {
                    operation();
                    return;
                }
                catch (Exception ex) when (retryCount < maxRetries && IsTransientError(ex))
                {
                    retryCount++;
                    onRetry?.Invoke(retryCount, ex);

                    Thread.Sleep(delay);
                    delay *= 2;

                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if an error is transient and should be retried
        /// </summary>
        private static bool IsTransientError(Exception ex)
        {
            // SharePoint throttling errors
            if (ex.Message.Contains("429") || ex.Message.Contains("throttle", StringComparison.OrdinalIgnoreCase))
                return true;

            // Server errors (5xx)
            if (ex.Message.Contains("503") || ex.Message.Contains("500"))
                return true;

            // Timeout errors
            if (ex is TimeoutException)
                return true;

            // Network errors
            if (ex.Message.Contains("network", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase))
                return true;

            // SharePoint ServerException
            if (ex.GetType().Name == "ServerException")
                return true;

            return false;
        }
    }
}
