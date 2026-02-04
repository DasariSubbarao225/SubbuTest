using Microsoft.SharePoint.Client;
using SharePointLargeListApp.Models;
using SharePointLargeListApp.Services;
using SharePointLargeListApp.Utilities;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using IOFile = System.IO.File;

namespace SharePointLargeListApp
{
    public partial class MainForm : WinForms.Form
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private Logger? _logger;
        private ProcessResult? _lastResult;
        private ClientContext? _context;

        public MainForm()
        {
            InitializeComponent();
            _logger = new Logger();
        }

        private void AuthMethod_CheckedChanged(object? sender, EventArgs e)
        {
            pnlModernAuth.Visible = rbModernAuth.Checked;
            pnlUsernamePassword.Visible = rbUsernamePassword.Checked;
            pnlClientSecret.Visible = rbClientSecret.Checked;
            // Interactive authentication doesn't need any input panels - it will open browser
        }

        private async void btnTestConnection_Click(object? sender, EventArgs e)
        {
            try
            {
                btnTestConnection.Enabled = false;
                lblConnectionStatus.Text = "Testing connection...";
                lblConnectionStatus.ForeColor = System.Drawing.Color.Blue;

                if (string.IsNullOrWhiteSpace(txtSiteUrl.Text))
                {
                    MessageBox.Show("Please enter a Site URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var authService = new AuthenticationService();
                ClientContext? context = null;

                if (rbInteractive.Checked)
                {
                    // Interactive browser-based authentication - no credentials needed
                    lblConnectionStatus.Text = "Opening browser for authentication...";
                    context = await authService.GetContextInteractive(txtSiteUrl.Text.Trim());
                }
                else if (rbModernAuth.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtClientId.Text) || string.IsNullOrWhiteSpace(txtTenantId.Text))
                    {
                        MessageBox.Show("Please enter Client ID and Tenant ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    context = await authService.GetContextWithMSAL(
                        txtSiteUrl.Text.Trim(),
                        txtClientId.Text.Trim(),
                        txtTenantId.Text.Trim()
                    );
                }
                else if (rbUsernamePassword.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        MessageBox.Show("Please enter Username and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    context = authService.GetContextWithCredentials(
                        txtSiteUrl.Text.Trim(),
                        txtUsername.Text.Trim(),
                        txtPassword.Text
                    );
                }
                else if (rbClientSecret.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtClientIdSecret.Text) ||
                        string.IsNullOrWhiteSpace(txtClientSecret.Text) ||
                        string.IsNullOrWhiteSpace(txtTenantIdSecret.Text))
                    {
                        MessageBox.Show("Please enter Client ID, Client Secret, and Tenant ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    context = await authService.GetContextWithClientSecret(
                        txtSiteUrl.Text.Trim(),
                        txtClientIdSecret.Text.Trim(),
                        txtClientSecret.Text,
                        txtTenantIdSecret.Text.Trim()
                    );
                }

                if (context != null)
                {
                    bool success = authService.TestConnection(context, out string webTitle, out string error);

                    if (success)
                    {
                        lblConnectionStatus.Text = $"✓ Connected: {webTitle}";
                        lblConnectionStatus.ForeColor = System.Drawing.Color.Green;
                        _context = context;
                        _logger?.Log($"Successfully connected to: {webTitle}", LogLevel.Success);
                        MessageBox.Show($"Successfully connected to:\n{webTitle}", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        lblConnectionStatus.Text = "✗ Connection Failed";
                        lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                        _logger?.LogError("Connection test failed", new Exception(error));
                        MessageBox.Show($"Connection failed:\n{error}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "✗ Connection Failed";
                lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                _logger?.LogError("Authentication error", ex);
                MessageBox.Show($"Authentication failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestConnection.Enabled = true;
            }
        }

        private async void btnProcess_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validation
                if (!ValidateInputs())
                    return;

                // Initialize logger for this session
                _logger = new Logger();

                // Disable controls
                SetProcessingState(true);

                // Get or create connection
                if (_context == null)
                {
                    await EstablishConnection();
                    if (_context == null)
                        return;
                }

                // Configure settings
                var config = new SharePointConfig
                {
                    SiteUrl = txtSiteUrl.Text.Trim(),
                    ListName = txtListName.Text.Trim(),
                    CalculatedColumnName = txtCalculatedColumn.Text.Trim(),
                    TargetColumnName = txtTargetColumn.Text.Trim(),
                    BatchSize = (int)numBatchSize.Value,
                    UpdateBatchSize = (int)numUpdateBatchSize.Value,
                    MaxRetryAttempts = 3
                };

                // Create services
                var spService = new SharePointService(_context, config, _logger);
                var processor = new ListProcessor(spService, config, _logger);

                // Wire up events
                processor.ProgressChanged += Processor_ProgressChanged;
                processor.LogMessage += Processor_LogMessage;

                // Start processing
                _cancellationTokenSource = new CancellationTokenSource();
                ProcessResult result;

                if (chkUseIdRange.Checked)
                {
                    LogMessage("Using ID Range processing method...");
                    result = await processor.ProcessListByIdRangeAsync(_cancellationTokenSource.Token);
                }
                else
                {
                    LogMessage("Using Pagination processing method...");
                    result = await processor.ProcessListAsync(_cancellationTokenSource.Token);
                }

                // Store result for export
                _lastResult = result;

                // Show summary
                ShowSummary(result);

                // Enable export if there are failures
                btnExportFailures.Enabled = result.Errors.Any();
            }
            catch (Exception ex)
            {
                _logger?.LogError("Processing error", ex);
                MessageBox.Show($"Error during processing:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetProcessingState(false);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSiteUrl.Text))
            {
                MessageBox.Show("Please enter a Site URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtListName.Text))
            {
                MessageBox.Show("Please enter a List Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCalculatedColumn.Text))
            {
                MessageBox.Show("Please enter the Calculated Column name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTargetColumn.Text))
            {
                MessageBox.Show("Please enter the Target Column name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task EstablishConnection()
        {
            try
            {
                var authService = new AuthenticationService();

                if (rbInteractive.Checked)
                {
                    // Interactive browser-based authentication
                    _context = await authService.GetContextInteractive(txtSiteUrl.Text.Trim());
                }
                else if (rbModernAuth.Checked)
                {
                    _context = await authService.GetContextWithMSAL(
                        txtSiteUrl.Text.Trim(),
                        txtClientId.Text.Trim(),
                        txtTenantId.Text.Trim()
                    );
                }
                else if (rbUsernamePassword.Checked)
                {
                    _context = authService.GetContextWithCredentials(
                        txtSiteUrl.Text.Trim(),
                        txtUsername.Text.Trim(),
                        txtPassword.Text
                    );
                }
                else if (rbClientSecret.Checked)
                {
                    _context = await authService.GetContextWithClientSecret(
                        txtSiteUrl.Text.Trim(),
                        txtClientIdSecret.Text.Trim(),
                        txtClientSecret.Text,
                        txtTenantIdSecret.Text.Trim()
                    );
                }

                if (_context != null)
                {
                    authService.TestConnection(_context, out _, out _);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to establish connection:\n{ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _context = null;
            }
        }

        private void SetProcessingState(bool isProcessing)
        {
            grpConnection.Enabled = !isProcessing;
            grpAuthentication.Enabled = !isProcessing;
            grpColumns.Enabled = !isProcessing;
            grpSettings.Enabled = !isProcessing;
            btnProcess.Enabled = !isProcessing;
            btnCancel.Enabled = isProcessing;
            btnTestConnection.Enabled = !isProcessing;
        }

        private void Processor_ProgressChanged(object? sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Processor_ProgressChanged(sender, e)));
                return;
            }

            progressBar.Value = Math.Min(e.Percentage, 100);
            lblProgress.Text = $"{e.Phase}: {e.Current:N0} / {e.Total:N0} ({e.Percentage}%)";
        }

        private void Processor_LogMessage(object? sender, string message)
        {
            LogMessage(message);
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessage(message)));
                return;
            }

            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
            txtLog.AppendText(logEntry + Environment.NewLine);
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void ShowSummary(ProcessResult result)
        {
            string message = $"Processing Complete!\n\n" +
                           $"Total Items: {result.TotalItems:N0}\n" +
                           $"Successfully Updated: {result.ProcessedItems:N0}\n" +
                           $"Failed: {result.FailedItems:N0}\n" +
                           $"Duration: {result.Duration:hh\\:mm\\:ss}\n\n";

            if (result.FailedItems > 0)
            {
                message += "Failed items can be exported using the 'Export Failed Items' button.";
            }

            MessageBox.Show(message, "Processing Complete",
                MessageBoxButtons.OK,
                result.FailedItems > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                LogMessage("Cancellation requested by user...");
                btnCancel.Enabled = false;
            }
        }

        private void btnExportFailures_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_lastResult == null || !_lastResult.Errors.Any())
                {
                    MessageBox.Show("No failed items to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string filePath = CsvExporter.GenerateFailedItemsReport(_lastResult);
                _logger?.Log($"Failed items exported to: {filePath}", LogLevel.Success);

                var result = MessageBox.Show(
                    $"Failed items exported successfully!\n\n" +
                    $"File: {filePath}\n\n" +
                    $"Would you like to open the file?",
                    "Export Successful",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError("Export failed", ex);
                MessageBox.Show($"Failed to export:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenLog_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_logger != null)
                {
                    string logPath = _logger.GetLogFilePath();
                    if (IOFile.Exists(logPath))
                    {
                        Process.Start(new ProcessStartInfo(logPath) { UseShellExecute = true });
                    }
                    else
                    {
                        MessageBox.Show("Log file not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open log file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
