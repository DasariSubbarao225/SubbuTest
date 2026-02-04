using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharePointListCopyTool.Helpers;
using SharePointListCopyTool.Models;
using SharePointListCopyTool.Services;

namespace SharePointListCopyTool
{
    public partial class MainForm : Form
    {
        private SharePointService _spService;
        private CancellationTokenSource _cancellationTokenSource;

        public MainForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoggingService.LogInfo("Application started");
            UpdateUIState(false);
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSiteUrl.Text))
            {
                MessageBox.Show("Please enter a SharePoint site URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnConnect.Enabled = false;
                Cursor = Cursors.WaitCursor;
                AddLog("Connecting to SharePoint...");

                await Task.Run(() =>
                {
                    _spService = new SharePointService(txtSiteUrl.Text.Trim());
                    _spService.StatusChanged += SpService_StatusChanged;
                    _spService.ProgressChanged += SpService_ProgressChanged;
                    _spService.Connect(null, null);
                });

                await LoadListsAsync();
                UpdateUIState(true);
                AddLog("Connected successfully!");
                MessageBox.Show("Connected to SharePoint successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AddLog($"ERROR: {ex.Message}");
                MessageBox.Show($"Failed to connect to SharePoint:\n\n{ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateUIState(false);
            }
            finally
            {
                btnConnect.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async Task LoadListsAsync()
        {
            try
            {
                AddLog("Loading lists...");
                var lists = await Task.Run(() => _spService.GetListNames());

                cmbLists.Items.Clear();
                foreach (var list in lists)
                {
                    cmbLists.Items.Add(list);
                }

                AddLog($"Loaded {lists.Count} lists");
            }
            catch (Exception ex)
            {
                AddLog($"ERROR loading lists: {ex.Message}");
                throw;
            }
        }

        private async void cmbLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLists.SelectedItem == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                string listName = cmbLists.SelectedItem.ToString();
                AddLog($"Loading columns for list: {listName}");

                var columns = await Task.Run(() => _spService.GetListColumns(listName));

                cmbSourceColumn.Items.Clear();
                cmbDestinationColumn.Items.Clear();

                foreach (var column in columns)
                {
                    cmbSourceColumn.Items.Add(column);
                    cmbDestinationColumn.Items.Add(column);
                }

                AddLog($"Loaded {columns.Count} columns");
            }
            catch (Exception ex)
            {
                AddLog($"ERROR loading columns: {ex.Message}");
                MessageBox.Show($"Failed to load columns:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            if (MessageBox.Show(
                $"This will copy data from '{cmbSourceColumn.Text}' to '{cmbDestinationColumn.Text}' in the list '{cmbLists.Text}'.\n\nDo you want to continue?",
                "Confirm Operation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                SetOperationInProgress(true);

                string listName = cmbLists.Text;
                string sourceColumn = cmbSourceColumn.Text;
                string destinationColumn = cmbDestinationColumn.Text;
                bool skipEmpty = chkSkipEmpty.Checked;

                AddLog($"Starting copy operation...");
                AddLog($"List: {listName}");
                AddLog($"Source Column: {sourceColumn}");
                AddLog($"Destination Column: {destinationColumn}");
                AddLog($"Skip Empty: {skipEmpty}");

                var result = await _spService.CopyColumnDataAsync(
                    listName,
                    sourceColumn,
                    destinationColumn,
                    skipEmpty,
                    _cancellationTokenSource.Token);

                AddLog("=== Operation Completed ===");
                AddLog($"Total Items: {result.TotalItems}");
                AddLog($"Success: {result.SuccessCount}");
                AddLog($"Failed: {result.FailureCount}");
                AddLog($"Skipped: {result.SkippedCount}");
                AddLog($"Duration: {result.Duration.TotalSeconds:F2} seconds");

                if (result.FailureCount > 0)
                {
                    AddLog($"Failed Item IDs: {string.Join(", ", result.FailedItemIds)}");
                }

                MessageBox.Show(result.ToString(), "Operation Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                AddLog("Operation cancelled by user");
                MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AddLog($"ERROR: {ex.Message}");
                MessageBox.Show($"Operation failed:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetOperationInProgress(false);
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                AddLog("Cancelling operation...");
                _cancellationTokenSource.Cancel();
                btnStop.Enabled = false;
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private bool ValidateInputs()
        {
            if (cmbLists.SelectedItem == null)
            {
                MessageBox.Show("Please select a list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbSourceColumn.SelectedItem == null)
            {
                MessageBox.Show("Please select a source column.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbDestinationColumn.SelectedItem == null)
            {
                MessageBox.Show("Please select a destination column.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbSourceColumn.Text == cmbDestinationColumn.Text)
            {
                MessageBox.Show("Source and destination columns cannot be the same.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdateUIState(bool connected)
        {
            grpConnection.Enabled = !connected;
            grpOperation.Enabled = connected;
            btnStart.Enabled = connected;
        }

        private void SetOperationInProgress(bool inProgress)
        {
            btnStart.Enabled = !inProgress;
            btnStop.Enabled = inProgress;
            grpConnection.Enabled = !inProgress;
            cmbLists.Enabled = !inProgress;
            cmbSourceColumn.Enabled = !inProgress;
            cmbDestinationColumn.Enabled = !inProgress;
            chkSkipEmpty.Enabled = !inProgress;
        }

        private void SpService_StatusChanged(object sender, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddLog(status)));
            }
            else
            {
                AddLog(status);
            }
        }

        private void SpService_ProgressChanged(object sender, int percentage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => progressBar.Value = Math.Min(percentage, 100)));
            }
            else
            {
                progressBar.Value = Math.Min(percentage, 100);
            }
        }

        private void AddLog(string message)
        {
            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            txtLog.AppendText(logMessage + Environment.NewLine);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _spService?.Disconnect();
            LoggingService.LogInfo("Application closed");
        }
    }
}
