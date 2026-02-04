using System;
using System.Collections.Generic;

namespace SharePointListCopyTool.Models
{
    public class CopyResult
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> FailedItemIds { get; set; }
        public List<string> ErrorMessages { get; set; }
        public TimeSpan Duration { get; set; }

        public CopyResult()
        {
            FailedItemIds = new List<string>();
            ErrorMessages = new List<string>();
        }

        public override string ToString()
        {
            return $"Total: {TotalItems}, Success: {SuccessCount}, Failed: {FailureCount}, Skipped: {SkippedCount}, Duration: {Duration.TotalSeconds:F2}s";
        }
    }
}
