namespace TorrentRationer.Models
{
    public class AppConfiguration
    {
        public int MinUploadRate { get; set; } = 0;
        public int MaxUploadRate { get; set; } = 1000;
        public string TorrentClient { get; set; } = "qBittorrent-4.5.0";
        public int SimultaneousSeeds { get; set; } = 5;
        public double UploadRatioTarget { get; set; } = -1;
        public bool KeepSeedingWithNoPeers { get; set; } = true;
        public string DefaultTorrentPath { get; set; } = "";
        public bool AutoStartWithWindows { get; set; } = false;
        public bool DarkMode { get; set; } = true;
        public List<TrackerConfig> Trackers { get; set; } = new();
    }

    public class TrackerConfig
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public bool ZeroKbSeeding { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
