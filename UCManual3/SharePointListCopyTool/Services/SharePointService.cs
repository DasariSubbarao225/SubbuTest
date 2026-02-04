using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SharePointListCopyTool.Helpers;
using SharePointListCopyTool.Models;

namespace SharePointListCopyTool.Services
{
    public class SharePointService
    {
        private ClientContext _context;
        private readonly string _siteUrl;

        public event EventHandler<string> StatusChanged;
        public event EventHandler<int> ProgressChanged;

        public SharePointService(string siteUrl)
        {
            _siteUrl = siteUrl;
        }

        public static ClientContext CreateClientContext(string siteUrl, string username, string password)
        {
            try
            {
                LoggingService.LogInfo($"Attempting to connect to SharePoint site: {siteUrl}");

                OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
                var context = authManager.GetWebLoginClientContext(siteUrl);

                context.RequestTimeout = AppConfig.RequestTimeout;

                context.Load(context.Web, web => web.Title);
                context.ExecuteQuery();

                LoggingService.LogInfo($"Successfully connected to site: {context.Web.Title}");
                return context;
            }
            catch (Exception ex)
            {
                string error = ExceptionHandler.HandleException(ex, "CreateClientContext");
                throw new Exception($"Failed to connect to SharePoint: {error}", ex);
            }
        }

        public void Connect(string username, string password)
        {
            try
            {
                _context = CreateClientContext(_siteUrl, username, password);
                OnStatusChanged($"Connected to SharePoint site: {_context.Web.Title}");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Connection failed: {ex.Message}");
                throw;
            }
        }

        public List<string> GetListNames()
        {
            try
            {
                LoggingService.LogInfo("Retrieving list names from SharePoint site");

                var lists = _context.Web.Lists;
                _context.Load(lists, l => l.Include(list => list.Title, list => list.Hidden));
                _context.ExecuteQuery();

                var listNames = lists.Where(l => !l.Hidden).Select(l => l.Title).OrderBy(t => t).ToList();

                LoggingService.LogInfo($"Retrieved {listNames.Count} lists from SharePoint");
                return listNames;
            }
            catch (Exception ex)
            {
                string error = ExceptionHandler.HandleException(ex, "GetListNames");
                throw new Exception($"Failed to retrieve lists: {error}", ex);
            }
        }

        public List<string> GetListColumns(string listName)
        {
            try
            {
                LoggingService.LogInfo($"Retrieving columns for list: {listName}");

                var list = _context.Web.Lists.GetByTitle(listName);
                _context.Load(list.Fields, fields => fields.Include(
                    f => f.InternalName,
                    f => f.Title,
                    f => f.Hidden,
                    f => f.ReadOnlyField,
                    f => f.FieldTypeKind));

                _context.ExecuteQuery();

                var columns = list.Fields
                    .Where(f => !f.Hidden && !f.ReadOnlyField && f.FieldTypeKind != FieldType.Computed)
                    .Select(f => f.InternalName)
                    .OrderBy(c => c)
                    .ToList();

                LoggingService.LogInfo($"Retrieved {columns.Count} columns for list: {listName}");
                return columns;
            }
            catch (Exception ex)
            {
                string error = ExceptionHandler.HandleException(ex, $"GetListColumns - {listName}");
                throw new Exception($"Failed to retrieve columns: {error}", ex);
            }
        }

        public async Task<CopyResult> CopyColumnDataAsync(string listName, string sourceColumn, string destinationColumn, bool skipEmpty = true, CancellationToken cancellationToken = default)
        {
            var result = new CopyResult();
            var startTime = DateTime.Now;

            try
            {
                LoggingService.LogInfo($"Starting column copy operation - List: {listName}, Source: {sourceColumn}, Destination: {destinationColumn}");
                OnStatusChanged($"Starting copy operation for list: {listName}");

                var list = _context.Web.Lists.GetByTitle(listName);
                _context.Load(list);
                _context.Load(list, l => l.ItemCount);
                _context.ExecuteQuery();

                result.TotalItems = list.ItemCount;
                OnStatusChanged($"Total items in list: {result.TotalItems}");

                await ProcessListItemsAsync(list, sourceColumn, destinationColumn, skipEmpty, result, cancellationToken);

                result.Duration = DateTime.Now - startTime;
                LoggingService.LogInfo($"Copy operation completed: {result}");
                OnStatusChanged($"Operation completed: {result}");

                return result;
            }
            catch (Exception ex)
            {
                result.Duration = DateTime.Now - startTime;
                string error = ExceptionHandler.HandleException(ex, "CopyColumnData");
                result.ErrorMessages.Add(error);
                OnStatusChanged($"Operation failed: {error}");
                throw;
            }
        }

        private async Task ProcessListItemsAsync(List list, string sourceColumn, string destinationColumn, bool skipEmpty, CopyResult result, CancellationToken cancellationToken)
        {
            ListItemCollectionPosition position = null;
            int processedCount = 0;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var camlQuery = new CamlQuery
                {
                    ListItemCollectionPosition = position,
                    ViewXml = $@"<View Scope='RecursiveAll'>
                                    <RowLimit>{AppConfig.BatchSize}</RowLimit>
                                </View>"
                };

                var items = list.GetItems(camlQuery);
                _context.Load(items);
                _context.ExecuteQuery();

                position = items.ListItemCollectionPosition;

                if (items.Count > 0)
                {
                    await ProcessBatchAsync(items, sourceColumn, destinationColumn, skipEmpty, result, cancellationToken);
                }

                processedCount += items.Count;
                int progressPercentage = result.TotalItems > 0 ? (processedCount * 100) / result.TotalItems : 0;
                OnProgressChanged(progressPercentage);
                OnStatusChanged($"Processed {processedCount} of {result.TotalItems} items...");

            } while (position != null);
        }

        private async Task ProcessBatchAsync(ListItemCollection items, string sourceColumn, string destinationColumn, bool skipEmpty, CopyResult result, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                foreach (ListItem item in items)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        if (!item.FieldValues.ContainsKey(sourceColumn))
                        {
                            result.SkippedCount++;
                            LoggingService.LogWarning($"Item ID {item.Id}: Source column '{sourceColumn}' not found");
                            continue;
                        }

                        var sourceValue = item[sourceColumn];

                        if (skipEmpty && (sourceValue == null || string.IsNullOrWhiteSpace(sourceValue.ToString())))
                        {
                            result.SkippedCount++;
                            continue;
                        }

                        item[destinationColumn] = sourceValue;
                        item.Update();

                        result.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        result.FailedItemIds.Add(item.Id.ToString());
                        string error = ExceptionHandler.HandleException(ex, $"Item ID: {item.Id}");
                        result.ErrorMessages.Add($"Item {item.Id}: {error}");
                    }
                }

                ExecuteQueryWithRetry();

            }, cancellationToken);
        }

        private void ExecuteQueryWithRetry()
        {
            int retryCount = 0;
            int maxRetries = AppConfig.RetryAttempts;

            while (retryCount < maxRetries)
            {
                try
                {
                    _context.ExecuteQuery();
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (ExceptionHandler.IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        int delay = AppConfig.RetryDelaySeconds * retryCount;
                        LoggingService.LogWarning($"Retrying after {delay} seconds (Attempt {retryCount}/{maxRetries})...");
                        OnStatusChanged($"Throttled or error occurred. Retrying in {delay} seconds...");
                        Thread.Sleep(delay * 1000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Disconnect()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
                OnStatusChanged("Disconnected from SharePoint");
                LoggingService.LogInfo("Disconnected from SharePoint");
            }
        }

        protected virtual void OnStatusChanged(string status)
        {
            StatusChanged?.Invoke(this, status);
        }

        protected virtual void OnProgressChanged(int percentage)
        {
            ProgressChanged?.Invoke(this, percentage);
        }
    }
}
