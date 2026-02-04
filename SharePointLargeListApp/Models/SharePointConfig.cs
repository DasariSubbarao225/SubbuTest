namespace SharePointLargeListApp.Models
{
    public class SharePointConfig
    {
        public string SiteUrl { get; set; } = string.Empty;
        public string ListName { get; set; } = string.Empty;
        public string CalculatedColumnName { get; set; } = string.Empty;
        public string TargetColumnName { get; set; } = string.Empty;
        public int BatchSize { get; set; } = 2000;
        public int UpdateBatchSize { get; set; } = 100;
        public int MaxRetryAttempts { get; set; } = 3;
    }
}
