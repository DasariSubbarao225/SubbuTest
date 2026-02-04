using Microsoft.SharePoint.Client;
using SharePointLargeListApp.Models;
using SharePointLargeListApp.Utilities;

namespace SharePointLargeListApp.Services
{
    public class SharePointService
    {
        private readonly ClientContext _context;
        private readonly SharePointConfig _config;
        private readonly Logger _logger;

        public SharePointService(ClientContext context, SharePointConfig config, Logger logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Get total item count in the list
        /// </summary>
        public int GetListItemCount()
        {
            int itemCount = 0;
            RetryHelper.ExecuteWithRetry(() =>
            {
                var list = _context.Web.Lists.GetByTitle(_config.ListName);
                _context.Load(list, l => l.ItemCount);
                _context.ExecuteQuery();
                _logger.Log($"List '{_config.ListName}' has {list.ItemCount} items");
                itemCount = list.ItemCount;
            }, _config.MaxRetryAttempts, onRetry: (attempt, ex) =>
            {
                _logger.Log($"Retry attempt {attempt} for GetListItemCount: {ex.Message}", LogLevel.Warning);
            });
            return itemCount;
        }

        /// <summary>
        /// Retrieve all items in batches using CAML query with pagination
        /// This approach handles list view threshold by using pagination
        /// </summary>
        public IEnumerable<ListItem> GetAllItemsInBatches(Action<int, int>? progressCallback = null)
        {
            var list = _context.Web.Lists.GetByTitle(_config.ListName);
            var totalItems = GetListItemCount();
            int totalProcessed = 0;

            var camlQuery = new CamlQuery
            {
                ViewXml = $@"
                    <View Scope='RecursiveAll'>
                        <Query>
                            <OrderBy>
                                <FieldRef Name='ID' Ascending='TRUE'/>
                            </OrderBy>
                        </Query>
                        <RowLimit>{_config.BatchSize}</RowLimit>
                    </View>"
            };

            do
            {
                ListItemCollection items = null!;

                RetryHelper.ExecuteWithRetry(() =>
                {
                    items = list.GetItems(camlQuery);
                    _context.Load(items,
                        itms => itms.ListItemCollectionPosition,
                        itms => itms.Include(
                            item => item.Id,
                            item => item[_config.CalculatedColumnName],
                            item => item[_config.TargetColumnName]
                        ));

                    _context.ExecuteQuery();
                    _logger.Log($"Retrieved batch of {items.Count} items");
                }, _config.MaxRetryAttempts, onRetry: (attempt, ex) =>
                {
                    _logger.Log($"Retry attempt {attempt} for GetItems: {ex.Message}", LogLevel.Warning);
                });

                foreach (var item in items)
                {
                    totalProcessed++;
                    progressCallback?.Invoke(totalProcessed, totalItems);
                    yield return item;
                }

                // Set pagination token for next batch
                camlQuery.ListItemCollectionPosition = items.ListItemCollectionPosition;

            } while (camlQuery.ListItemCollectionPosition != null);

            _logger.Log($"Completed retrieving all {totalProcessed} items", LogLevel.Success);
        }

        /// <summary>
        /// Alternative approach: Get items using ID-based range queries
        /// Most reliable for avoiding threshold - ID column is always indexed
        /// </summary>
        public IEnumerable<ListItem> GetItemsByIdRange(int startId, int endId)
        {
            var list = _context.Web.Lists.GetByTitle(_config.ListName);

            var camlQuery = new CamlQuery
            {
                ViewXml = $@"
                    <View Scope='RecursiveAll'>
                        <Query>
                            <Where>
                                <And>
                                    <Geq>
                                        <FieldRef Name='ID'/>
                                        <Value Type='Number'>{startId}</Value>
                                    </Geq>
                                    <Lt>
                                        <FieldRef Name='ID'/>
                                        <Value Type='Number'>{endId}</Value>
                                    </Lt>
                                </And>
                            </Where>
                            <OrderBy>
                                <FieldRef Name='ID' Ascending='TRUE'/>
                            </OrderBy>
                        </Query>
                        <RowLimit>{_config.BatchSize}</RowLimit>
                    </View>"
            };

            ListItemCollection items = null!;

            RetryHelper.ExecuteWithRetry(() =>
            {
                items = list.GetItems(camlQuery);
                _context.Load(items,
                    itms => itms.Include(
                        item => item.Id,
                        item => item[_config.CalculatedColumnName],
                        item => item[_config.TargetColumnName]
                    ));

                _context.ExecuteQuery();
            }, _config.MaxRetryAttempts, onRetry: (attempt, ex) =>
            {
                _logger.Log($"Retry attempt {attempt} for GetItemsByIdRange: {ex.Message}", LogLevel.Warning);
            });

            return items;
        }

        /// <summary>
        /// Get the maximum ID in the list for range-based processing
        /// </summary>
        public int GetMaxItemId()
        {
            var list = _context.Web.Lists.GetByTitle(_config.ListName);

            var camlQuery = new CamlQuery
            {
                ViewXml = @"
                    <View>
                        <Query>
                            <OrderBy>
                                <FieldRef Name='ID' Ascending='FALSE'/>
                            </OrderBy>
                        </Query>
                        <RowLimit>1</RowLimit>
                    </View>"
            };

            int maxId = 0;
            RetryHelper.ExecuteWithRetry(() =>
            {
                var items = list.GetItems(camlQuery);
                _context.Load(items, itms => itms.Include(item => item.Id));
                _context.ExecuteQuery();

                maxId = items.Count > 0 ? items[0].Id : 0;
            }, _config.MaxRetryAttempts);
            return maxId;
        }

        /// <summary>
        /// Update items in batch with proper error handling
        /// </summary>
        public List<FailedItem> UpdateItemsBatch(List<ListItem> items, FieldType targetFieldType)
        {
            var failedItems = new List<FailedItem>();

            try
            {
                var list = _context.Web.Lists.GetByTitle(_config.ListName);

                foreach (var item in items)
                {
                    try
                    {
                        var calculatedValue = item[_config.CalculatedColumnName];
                        
                        // Convert the value to the appropriate type
                        var convertedValue = ColumnTypeHandler.ConvertColumnValue(calculatedValue, targetFieldType);
                        
                        item[_config.TargetColumnName] = convertedValue;

                        // Use SystemUpdate to avoid changing Modified/ModifiedBy and triggering workflows
                        item.SystemUpdate();

                        _logger.Log($"Prepared update for item ID: {item.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error preparing item ID {item.Id} for update", ex);
                        failedItems.Add(new FailedItem
                        {
                            ItemId = item.Id,
                            ErrorMessage = ex.Message,
                            CalculatedValue = item[_config.CalculatedColumnName]?.ToString() ?? string.Empty
                        });
                    }
                }

                // Execute batch update with retry logic
                RetryHelper.ExecuteWithRetry(() =>
                {
                    _context.ExecuteQuery();
                    _logger.Log($"Successfully updated batch of {items.Count - failedItems.Count} items", LogLevel.Success);
                }, _config.MaxRetryAttempts, onRetry: (attempt, ex) =>
                {
                    _logger.Log($"Retry attempt {attempt} for batch update: {ex.Message}", LogLevel.Warning);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Critical error in batch update", ex);
                
                // If the entire batch failed, mark all items as failed
                foreach (var item in items)
                {
                    if (!failedItems.Any(f => f.ItemId == item.Id))
                    {
                        failedItems.Add(new FailedItem
                        {
                            ItemId = item.Id,
                            ErrorMessage = $"Batch update failed: {ex.Message}",
                            CalculatedValue = item[_config.CalculatedColumnName]?.ToString() ?? string.Empty
                        });
                    }
                }
            }

            return failedItems;
        }

        /// <summary>
        /// Get field type for the target column
        /// </summary>
        public FieldType GetTargetFieldType()
        {
            FieldType fieldType = FieldType.Text;
            RetryHelper.ExecuteWithRetry(() =>
            {
                fieldType = ColumnTypeHandler.GetFieldType(_context, _config.ListName, _config.TargetColumnName);
            }, _config.MaxRetryAttempts);
            return fieldType;
        }

        /// <summary>
        /// Validate that both columns exist and are accessible
        /// </summary>
        public bool ValidateColumns(out string errorMessage)
        {
            try
            {
                var list = _context.Web.Lists.GetByTitle(_config.ListName);
                var calculatedField = list.Fields.GetByInternalNameOrTitle(_config.CalculatedColumnName);
                var targetField = list.Fields.GetByInternalNameOrTitle(_config.TargetColumnName);

                _context.Load(calculatedField, f => f.Title, f => f.InternalName, f => f.FieldTypeKind, f => f.ReadOnlyField);
                _context.Load(targetField, f => f.Title, f => f.InternalName, f => f.FieldTypeKind, f => f.ReadOnlyField);
                _context.ExecuteQuery();

                // Check if calculated field is actually calculated
                if (calculatedField.FieldTypeKind != FieldType.Calculated)
                {
                    _logger.Log($"Warning: '{_config.CalculatedColumnName}' is not a calculated column (Type: {calculatedField.FieldTypeKind})", LogLevel.Warning);
                }

                // Check if target field is read-only
                if (targetField.ReadOnlyField)
                {
                    errorMessage = $"Target column '{_config.TargetColumnName}' is read-only and cannot be updated.";
                    _logger.Log(errorMessage, LogLevel.Error);
                    return false;
                }

                _logger.Log($"Columns validated - Source: {calculatedField.Title} ({calculatedField.FieldTypeKind}), Target: {targetField.Title} ({targetField.FieldTypeKind})", LogLevel.Success);
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Column validation failed: {ex.Message}";
                _logger.LogError(errorMessage, ex);
                return false;
            }
        }
    }
}
