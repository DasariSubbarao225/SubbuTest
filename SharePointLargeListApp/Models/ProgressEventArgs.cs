namespace SharePointLargeListApp.Models
{
    public class ProgressEventArgs : EventArgs
    {
        public int Current { get; }
        public int Total { get; }
        public int Percentage => Total > 0 ? (int)((double)Current / Total * 100) : 0;
        public string Phase { get; set; } = string.Empty;

        public ProgressEventArgs(int current, int total, string phase = "")
        {
            Current = current;
            Total = total;
            Phase = phase;
        }
    }
}
