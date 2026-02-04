using System;
using System.Net;
using Microsoft.SharePoint.Client;
using SharePointListCopyTool.Services;

namespace SharePointListCopyTool.Helpers
{
    public static class ExceptionHandler
    {
        public static string HandleException(Exception ex, string context = "")
        {
            string errorMessage = string.Empty;

            if (ex is ServerException serverEx)
            {
                errorMessage = HandleServerException(serverEx, context);
            }
            else if (ex is WebException webEx)
            {
                errorMessage = HandleWebException(webEx, context);
            }
            else if (ex is UnauthorizedAccessException)
            {
                errorMessage = $"Access Denied: You don't have permission to perform this operation. {context}";
                LoggingService.LogError(errorMessage, ex);
            }
            else if (ex is ArgumentException argEx)
            {
                errorMessage = $"Invalid Argument: {argEx.Message}. {context}";
                LoggingService.LogError(errorMessage, ex);
            }
            else
            {
                errorMessage = $"Unexpected Error: {ex.Message}. {context}";
                LoggingService.LogException(ex, context);
            }

            return errorMessage;
        }

        private static string HandleServerException(ServerException ex, string context)
        {
            string errorMessage;

            if (ex.ServerErrorTypeName == "Microsoft.SharePoint.SPException")
            {
                if (ex.Message.Contains("does not exist"))
                {
                    errorMessage = $"SharePoint Resource Not Found: {ex.Message}. {context}";
                }
                else if (ex.Message.Contains("Access denied"))
                {
                    errorMessage = $"SharePoint Access Denied: You don't have sufficient permissions. {context}";
                }
                else
                {
                    errorMessage = $"SharePoint Error: {ex.Message}. {context}";
                }
            }
            else if (ex.ServerErrorTypeName == "System.UnauthorizedAccessException")
            {
                errorMessage = $"Authorization Error: Insufficient permissions to access SharePoint. {context}";
            }
            else
            {
                errorMessage = $"SharePoint Server Error ({ex.ServerErrorTypeName}): {ex.Message}. {context}";
            }

            LoggingService.LogError(errorMessage, ex);
            return errorMessage;
        }

        private static string HandleWebException(WebException ex, string context)
        {
            string errorMessage;

            if (ex.Response is HttpWebResponse response)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        errorMessage = $"Authentication Failed (401): Please check your credentials. {context}";
                        break;
                    case HttpStatusCode.Forbidden:
                        errorMessage = $"Access Forbidden (403): You don't have permission to access this resource. {context}";
                        break;
                    case HttpStatusCode.NotFound:
                        errorMessage = $"Resource Not Found (404): The requested resource was not found. {context}";
                        break;
                    case (HttpStatusCode)429:
                        errorMessage = $"Throttled (429): Too many requests. The operation will be retried. {context}";
                        break;
                    case HttpStatusCode.ServiceUnavailable:
                        errorMessage = $"Service Unavailable (503): SharePoint service is temporarily unavailable. {context}";
                        break;
                    default:
                        errorMessage = $"HTTP Error ({response.StatusCode}): {ex.Message}. {context}";
                        break;
                }
            }
            else
            {
                errorMessage = $"Network Error: {ex.Message}. {context}";
            }

            LoggingService.LogError(errorMessage, ex);
            return errorMessage;
        }

        public static bool IsThrottledException(Exception ex)
        {
            if (ex is WebException webEx && webEx.Response is HttpWebResponse response)
            {
                return response.StatusCode == (HttpStatusCode)429;
            }

            if (ex is ServerException serverEx)
            {
                return serverEx.Message.Contains("throttled") || 
                       serverEx.Message.Contains("429") ||
                       serverEx.Message.Contains("Request rate is too high");
            }

            return false;
        }

        public static bool IsRetryableException(Exception ex)
        {
            if (IsThrottledException(ex))
                return true;

            if (ex is WebException webEx && webEx.Response is HttpWebResponse response)
            {
                return response.StatusCode == HttpStatusCode.ServiceUnavailable ||
                       response.StatusCode == HttpStatusCode.RequestTimeout ||
                       response.StatusCode == HttpStatusCode.GatewayTimeout;
            }

            return false;
        }
    }
}
