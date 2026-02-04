using Microsoft.SharePoint.Client;
using Microsoft.Identity.Client;
using System.Diagnostics;
using IOFile = System.IO.File;

namespace SharePointLargeListApp.Services
{
    public class AuthenticationService
    {
        // Microsoft's public Client ID (Azure CLI) - can be used for testing/development
        private const string DefaultPublicClientId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";

        /// <summary>
        /// Simplified Browser-Based Interactive Authentication
        /// Opens the system browser for user sign-in and can reuse existing browser sessions
        /// Uses Microsoft's public Client ID - no Azure AD app registration needed
        /// </summary>
        public async Task<ClientContext> GetContextInteractive(string siteUrl)
        {
            try
            {
                var uri = new Uri(siteUrl);
                var tenantId = "36da45f1-dd2c-4d1f-af13-5abe46b99921";

                var authority = $"https://login.microsoftonline.com/{tenantId}";
                var redirectUri = "http://localhost";

                var app = PublicClientApplicationBuilder
                    .Create(DefaultPublicClientId)
                    .WithAuthority(authority)
                    .WithRedirectUri(redirectUri)
                    .Build();

                var scopes = new[] { $"{uri.Scheme}://{uri.Authority}/.default" };

                AuthenticationResult? result = null;

                try
                {
                    var accounts = await app.GetAccountsAsync();
                    var accountToUse = accounts.FirstOrDefault();

                    if (accountToUse != null)
                    {
                        result = await app.AcquireTokenSilent(scopes, accountToUse)
                            .ExecuteAsync();
                    }
                }
                catch (MsalUiRequiredException)
                {
                }

                if (result == null)
                {
                    result = await app.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.SelectAccount)
                        .WithSystemWebViewOptions(new SystemWebViewOptions()
                        {
                            OpenBrowserAsync = OpenEdgeBrowserAsync,
                            HtmlMessageSuccess = "<html><body><h1>Authentication Successful!</h1><p>You can close this window and return to the application.</p></body></html>",
                            HtmlMessageError = "<html><body><h1>Authentication Failed</h1><p>Error: {0}</p><p>Error Description: {1}</p></body></html>"
                        })
                        .ExecuteAsync();
                }

                var context = new ClientContext(siteUrl);
                context.ExecutingWebRequest += (sender, e) =>
                {
                    e.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + result.AccessToken;
                };

                return context;
            }
            catch (Exception ex)
            {
                throw new Exception($"Interactive Authentication failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Custom method to open Microsoft Edge browser for authentication
        /// </summary>
        private static Task OpenEdgeBrowserAsync(Uri uri)
        {
            try
            {
                // Try to find Microsoft Edge executable
                string edgePath = GetEdgePath();
                
                if (!string.IsNullOrEmpty(edgePath) && IOFile.Exists(edgePath))
                {
                    // Launch Edge with the authentication URL
                    var psi = new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = $"\"{uri}\" --new-window",
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    // Fallback to default browser if Edge is not found
                    var psi = new ProcessStartInfo
                    {
                        FileName = uri.ToString(),
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                
                return Task.CompletedTask;
            }
            catch
            {
                // Final fallback - use default browser
                var psi = new ProcessStartInfo
                {
                    FileName = uri.ToString(),
                    UseShellExecute = true
                };
                Process.Start(psi);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Get the path to Microsoft Edge executable
        /// </summary>
        private static string GetEdgePath()
        {
            // Check common Edge installation paths
            var possiblePaths = new[]
            {
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft\Edge\Application\msedge.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft\Edge\Application\msedge.exe")
            };

            foreach (var path in possiblePaths)
            {
                if (IOFile.Exists(path))
                {
                    return path;
                }
            }

            // Try to get Edge path from registry
            try
            {
                var edgeKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe");
                if (edgeKey != null)
                {
                    var path = edgeKey.GetValue("")?.ToString();
                    if (!string.IsNullOrEmpty(path) && IOFile.Exists(path))
                    {
                        return path;
                    }
                }
            }
            catch
            {
                // Ignore registry errors
            }

            return string.Empty;
        }

        /// <summary>
        /// Test the connection by loading the web title
        /// </summary>
        public bool TestConnection(ClientContext context, out string webTitle, out string error)
        {
            try
            {
                context.Load(context.Web, w => w.Title);
                context.ExecuteQuery();
                webTitle = context.Web.Title;
                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                webTitle = string.Empty;
                error = ex.Message;
                return false;
            }
        }
    }
}
