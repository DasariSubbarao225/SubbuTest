using Microsoft.SharePoint.Client;
using Microsoft.Identity.Client;
using System.Security;
using System.Net;
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
        /// Opens Microsoft Edge browser for user to sign in with their credentials
        /// Uses Microsoft's public Client ID - no Azure AD app registration needed
        /// RECOMMENDED: Use this method for interactive user authentication
        /// </summary>
        public async Task<ClientContext> GetContextInteractive(string siteUrl, string? username = null)
        {
            try
            {
                // Extract tenant from SharePoint URL (e.g., "contoso" from "contoso.sharepoint.com")
                var uri = new Uri(siteUrl);
                var host = uri.Host;
                var tenantName = host.Split('.')[0];
                var tenantId = "organizations"; // Use 'organizations' for multi-tenant

                var authority = $"https://login.microsoftonline.com/{tenantId}";
                var redirectUri = "http://localhost";

                // Build the MSAL public client application
                var app = PublicClientApplicationBuilder
                    .Create(DefaultPublicClientId)
                    .WithAuthority(authority)
                    .WithRedirectUri(redirectUri)
                    .Build();

                // Define the scopes
                var scopes = new[] { $"{uri.Scheme}://{uri.Authority}/.default" };

                AuthenticationResult? result = null;

                // Try silent authentication first (uses cached token if available)
                try
                {
                    var accounts = await app.GetAccountsAsync();
                    var accountToUse = accounts.FirstOrDefault();
                    
                    // If username provided, try to find matching account
                    if (!string.IsNullOrEmpty(username) && accounts.Any())
                    {
                        accountToUse = accounts.FirstOrDefault(a => 
                            a.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) 
                            ?? accounts.FirstOrDefault();
                    }

                    if (accountToUse != null)
                    {
                        result = await app.AcquireTokenSilent(scopes, accountToUse)
                            .ExecuteAsync();
                    }
                }
                catch (MsalUiRequiredException)
                {
                    // Silent authentication failed, will use interactive
                }

                // If silent authentication failed, open Edge browser for interactive sign-in
                if (result == null)
                {
                    var interactiveRequest = app.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.SelectAccount)
                        .WithSystemWebViewOptions(new SystemWebViewOptions()
                        {
                            OpenBrowserAsync = OpenEdgeBrowserAsync,
                            HtmlMessageSuccess = "<html><body><h1>Authentication Successful!</h1><p>You can close this window and return to the application.</p></body></html>",
                            HtmlMessageError = "<html><body><h1>Authentication Failed</h1><p>Error: {0}</p><p>Error Description: {1}</p></body></html>"
                        });
                    
                    // Pre-fill username if provided
                    if (!string.IsNullOrEmpty(username))
                    {
                        interactiveRequest = interactiveRequest.WithLoginHint(username);
                    }
                    
                    result = await interactiveRequest.ExecuteAsync();
                }

                // Create the ClientContext with the access token
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
        /// Method 1: Username/Password Authentication (Legacy - works for accounts without MFA)
        /// Note: This method is deprecated and may not work with modern SharePoint Online
        /// Recommend using Modern Authentication (MSAL) instead
        /// </summary>
        public ClientContext GetContextWithCredentials(string siteUrl, string username, string password)
        {
            try
            {
                // For newer versions of CSOM, SharePointOnlineCredentials may not be available
                // This is a workaround using basic authentication which may not work for all tenants
                var context = new ClientContext(siteUrl);
                
                // Note: Basic authentication is being deprecated by Microsoft
                // This method is included for legacy support but may not work
                // Users should prefer Modern Authentication (MSAL)
                throw new NotSupportedException(
                    "Username/Password authentication is no longer supported in this version of CSOM. " +
                    "Please use Modern Authentication (MSAL - Interactive) or App-Only (Client Secret) instead.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Authentication failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Method 2: Modern Authentication with MSAL (Interactive - Recommended)
        /// Requires Azure AD App Registration with delegated permissions
        /// </summary>
        public async Task<ClientContext> GetContextWithMSAL(string siteUrl, string clientId, string tenantId, string? loginHint = null)
        {
            try
            {
                var authority = $"https://login.microsoftonline.com/{tenantId}";
                var redirectUri = "http://localhost";

                // Build the MSAL public client application
                var app = PublicClientApplicationBuilder
                    .Create(clientId)
                    .WithAuthority(authority)
                    .WithRedirectUri(redirectUri)
                    .Build();

                // Define the scopes
                var scopes = new[] { $"{new Uri(siteUrl).Scheme}://{new Uri(siteUrl).Authority}/.default" };

                AuthenticationResult? result = null;

                // Try silent authentication first
                try
                {
                    var accounts = await app.GetAccountsAsync();
                    if (accounts.Any())
                    {
                        result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                            .ExecuteAsync();
                    }
                }
                catch (MsalUiRequiredException)
                {
                    // Silent authentication failed, need interactive
                }

                // If silent authentication failed, use interactive
                if (result == null)
                {
                    var interactiveRequest = app.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.SelectAccount);
                    
                    // Add login hint if provided
                    if (!string.IsNullOrEmpty(loginHint))
                    {
                        interactiveRequest = interactiveRequest.WithLoginHint(loginHint);
                    }
                    
                    result = await interactiveRequest.ExecuteAsync();
                }

                // Create the ClientContext with the access token
                var context = new ClientContext(siteUrl);
                context.ExecutingWebRequest += (sender, e) =>
                {
                    e.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + result.AccessToken;
                };

                return context;
            }
            catch (Exception ex)
            {
                throw new Exception($"MSAL Authentication failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Method 2b: Username/Password Authentication with MSAL (ROPC Flow)
        /// Requires Azure AD App Registration with delegated permissions
        /// NOTE: This flow has limitations:
        /// - Does NOT work with MFA-enabled accounts
        /// - Does NOT work with federated/external accounts
        /// - Requires "Allow public client flows" enabled in Azure AD App Registration
        /// - Not recommended for production use due to security concerns
        /// Use interactive authentication (GetContextWithMSAL) when possible
        /// </summary>
        public async Task<ClientContext> GetContextWithUsernamePassword(
            string siteUrl, 
            string clientId, 
            string tenantId, 
            string username, 
            string password)
        {
            try
            {
                var authority = $"https://login.microsoftonline.com/{tenantId}";

                // Build the MSAL public client application
                var app = PublicClientApplicationBuilder
                    .Create(clientId)
                    .WithAuthority(authority)
                    .Build();

                // Define the scopes
                var scopes = new[] { $"{new Uri(siteUrl).Scheme}://{new Uri(siteUrl).Authority}/.default" };

                // Use Resource Owner Password Credentials (ROPC) flow
                var securePassword = new System.Security.SecureString();
                foreach (char c in password)
                {
                    securePassword.AppendChar(c);
                }

                var result = await app.AcquireTokenByUsernamePassword(scopes, username, securePassword)
                    .ExecuteAsync();

                // Create the ClientContext with the access token
                var context = new ClientContext(siteUrl);
                context.ExecutingWebRequest += (sender, e) =>
                {
                    e.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + result.AccessToken;
                };

                return context;
            }
            catch (MsalServiceException ex) when (ex.ErrorCode == "invalid_grant")
            {
                throw new Exception(
                    "Authentication failed. This could be due to:\n" +
                    "- Incorrect username or password\n" +
                    "- MFA is enabled on the account (ROPC doesn't support MFA)\n" +
                    "- Account is federated/external\n" +
                    "- Conditional access policies blocking the flow\n" +
                    "Consider using interactive authentication instead.", ex);
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user_type")
            {
                throw new Exception(
                    "Authentication failed. The user type is not supported with username/password flow. " +
                    "This typically happens with federated accounts. Use interactive authentication instead.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Username/Password Authentication failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Method 3: App-Only Authentication with Client Secret
        /// Requires Azure AD App Registration with application permissions
        /// </summary>
        public async Task<ClientContext> GetContextWithClientSecret(
            string siteUrl, 
            string clientId, 
            string clientSecret, 
            string tenantId)
        {
            try
            {
                var authority = $"https://login.microsoftonline.com/{tenantId}";

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();

                var scopes = new[] { $"{new Uri(siteUrl).Scheme}://{new Uri(siteUrl).Authority}/.default" };

                var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                var context = new ClientContext(siteUrl);
                context.ExecutingWebRequest += (sender, e) =>
                {
                    e.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + result.AccessToken;
                };

                return context;
            }
            catch (Exception ex)
            {
                throw new Exception($"Client Secret Authentication failed: {ex.Message}", ex);
            }
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
