namespace SharePointLargeListApp.Models
{
    public class ProcessResult
    {
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public int FailedItems { get; set; }
        public List<FailedItem> Errors { get; set; } = new List<FailedItem>();
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
    }

    public class FailedItem
    {
        public int ItemId { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string CalculatedValue { get; set; } = string.Empty;
        public DateTime ErrorTime { get; set; } = DateTime.Now;
    }
}
