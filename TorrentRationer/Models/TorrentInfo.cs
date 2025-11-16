namespace TorrentRationer.Models
{
    public class TorrentInfo
    {
        public string Name { get; set; } = "";
        public string Hash { get; set; } = "";
        public long Size { get; set; }
        public double UploadRatio { get; set; }
        public double FakeRatio { get; set; }
        public int UploadSpeed { get; set; }
        public int DownloadSpeed { get; set; }
        public string Status { get; set; } = "Stopped";
        public DateTime AddedDate { get; set; }
        public List<string> Trackers { get; set; } = new();
    }
}
