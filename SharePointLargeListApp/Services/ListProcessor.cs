using Microsoft.SharePoint.Client;
using SharePointLargeListApp.Models;
using SharePointLargeListApp.Utilities;

namespace SharePointLargeListApp.Services
{
    public class ListProcessor
    {
        private readonly SharePointService _spService;
        private readonly SharePointConfig _config;
        private readonly Logger _logger;
        private CancellationToken _cancellationToken;

        public event EventHandler<ProgressEventArgs>? ProgressChanged;
        public event EventHandler<string>? LogMessage;

        public ListProcessor(SharePointService spService, SharePointConfig config, Logger logger)
        {
            _spService = spService;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Main processing method with full error handling and progress tracking
        /// </summary>
        public async Task<ProcessResult> ProcessListAsync(CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
            var result = new ProcessResult
            {
                StartTime = DateTime.Now
            };

            try
            {
                _logger.Log("=== Starting SharePoint List Processing ===", LogLevel.Info);
                OnLogMessage("Starting processing...");

                // Step 1: Validate columns
                OnLogMessage("Validating columns...");
                OnProgressChanged(0, 100, "Validating");
                
                if (!_spService.ValidateColumns(out string validationError))
                {
                    throw new Exception(validationError);
                }

                // Step 2: Get target field type
                var targetFieldType = _spService.GetTargetFieldType();
                _logger.Log($"Target field type: {targetFieldType}");

                // Step 3: Get total count
                OnLogMessage("Getting total item count...");
                result.TotalItems = _spService.GetListItemCount();
                OnLogMessage($"Total items to process: {result.TotalItems}");

                if (result.TotalItems == 0)
                {
                    OnLogMessage("No items to process.");
                    result.EndTime = DateTime.Now;
                    return result;
                }

                // Step 4: Process items in batches
                OnProgressChanged(0, result.TotalItems, "Processing");
                await ProcessItemsInBatchesAsync(result, targetFieldType);

                result.EndTime = DateTime.Now;
                _logger.Log($"=== Processing Complete ===", LogLevel.Success);
                _logger.Log($"Total: {result.TotalItems}, Success: {result.ProcessedItems}, Failed: {result.FailedItems}, Duration: {result.Duration}");
                OnLogMessage($"Processing complete! Success: {result.ProcessedItems}, Failed: {result.FailedItems}");
            }
            catch (Exception ex)
            {
                result.EndTime = DateTime.Now;
                _logger.LogError("Critical error during processing", ex);
                OnLogMessage($"Error: {ex.Message}");
                result.Errors.Add(new FailedItem
                {
                    ItemId = -1,
                    ErrorMessage = $"Critical error: {ex.Message}"
                });
            }

            return result;
        }

        /// <summary>
        /// Process items using pagination-based approach
        /// </summary>
        private async Task ProcessItemsInBatchesAsync(ProcessResult result, FieldType targetFieldType)
        {
            var batchBuffer = new List<ListItem>();
            int itemsRetrieved = 0;

            foreach (var item in _spService.GetAllItemsInBatches(
                (processed, total) => OnProgressChanged(processed, total, "Retrieving")))
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    _logger.Log("Processing cancelled by user", LogLevel.Warning);
                    OnLogMessage("Processing cancelled.");
                    break;
                }

                batchBuffer.Add(item);
                itemsRetrieved++;

                // Update in batches
                if (batchBuffer.Count >= _config.UpdateBatchSize)
                {
                    await Task.Run(() => UpdateBatch(batchBuffer, result, targetFieldType));
                    
                    OnProgressChanged(result.ProcessedItems + result.FailedItems, result.TotalItems, "Updating");
                    batchBuffer.Clear();
                }
            }

            // Process remaining items
            if (batchBuffer.Any() && !_cancellationToken.IsCancellationRequested)
            {
                await Task.Run(() => UpdateBatch(batchBuffer, result, targetFieldType));
            }
        }

        /// <summary>
        /// Update a batch of items
        /// </summary>
        private void UpdateBatch(List<ListItem> items, ProcessResult result, FieldType targetFieldType)
        {
            try
            {
                _logger.Log($"Updating batch of {items.Count} items...");
                OnLogMessage($"Updating batch of {items.Count} items...");

                var failedItems = _spService.UpdateItemsBatch(items, targetFieldType);

                result.ProcessedItems += items.Count - failedItems.Count;
                result.FailedItems += failedItems.Count;
                result.Errors.AddRange(failedItems);

                if (failedItems.Any())
                {
                    OnLogMessage($"Batch completed with {failedItems.Count} failures");
                }
                else
                {
                    OnLogMessage($"Batch of {items.Count} items updated successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Batch update failed", ex);
                result.FailedItems += items.Count;
                
                foreach (var item in items)
                {
                    result.Errors.Add(new FailedItem
                    {
                        ItemId = item.Id,
                        ErrorMessage = $"Batch failed: {ex.Message}",
                        CalculatedValue = item[_config.CalculatedColumnName]?.ToString() ?? string.Empty
                    });
                }
            }
        }

        /// <summary>
        /// Alternative: Process items using ID-based range queries
        /// More efficient for very large lists
        /// </summary>
        public async Task<ProcessResult> ProcessListByIdRangeAsync(CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
            var result = new ProcessResult
            {
                StartTime = DateTime.Now
            };

            try
            {
                _logger.Log("=== Starting ID Range-Based Processing ===", LogLevel.Info);

                // Validate columns
                if (!_spService.ValidateColumns(out string validationError))
                {
                    throw new Exception(validationError);
                }

                var targetFieldType = _spService.GetTargetFieldType();
                result.TotalItems = _spService.GetListItemCount();
                int maxId = _spService.GetMaxItemId();

                OnLogMessage($"Processing {result.TotalItems} items (ID range: 1 to {maxId})");

                // Process in ID ranges
                for (int startId = 1; startId <= maxId; startId += _config.BatchSize)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    int endId = Math.Min(startId + _config.BatchSize, maxId + 1);
                    OnLogMessage($"Processing ID range: {startId} to {endId - 1}");

                    var items = _spService.GetItemsByIdRange(startId, endId).ToList();
                    
                    if (items.Any())
                    {
                        await Task.Run(() => UpdateBatch(items, result, targetFieldType));
                        OnProgressChanged(result.ProcessedItems + result.FailedItems, result.TotalItems, "Processing");
                    }
                }

                result.EndTime = DateTime.Now;
                _logger.Log($"=== ID Range Processing Complete ===", LogLevel.Success);
            }
            catch (Exception ex)
            {
                result.EndTime = DateTime.Now;
                _logger.LogError("Error during ID range processing", ex);
                OnLogMessage($"Error: {ex.Message}");
            }

            return result;
        }

        protected virtual void OnProgressChanged(int current, int total, string phase)
        {
            ProgressChanged?.Invoke(this, new ProgressEventArgs(current, total, phase));
        }

        protected virtual void OnLogMessage(string message)
        {
            LogMessage?.Invoke(this, message);
        }
    }
}
