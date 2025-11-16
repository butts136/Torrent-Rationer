using System.Text;
using System.Web;
using TorrentRationer.Models;

namespace TorrentRationer.Services
{
    public class TrackerAnnounceService
    {
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configService;
        private readonly Random _random = new();

        public TrackerAnnounceService(ConfigurationService configService)
        {
            _configService = configService;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> AnnounceToTracker(TorrentInfo torrent, string trackerUrl, string peerId, int port)
        {
            try
            {
                var config = _configService.GetConfiguration();
                var isZeroKbSeeding = IsZeroKbSeedingEnabled(trackerUrl);
                
                var announceParams = BuildAnnounceParams(torrent, peerId, port, isZeroKbSeeding, config);
                var announceUrl = $"{trackerUrl}?{announceParams}";

                var response = await _httpClient.GetAsync(announceUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tracker announce failed: {ex.Message}");
                return false;
            }
        }

        private bool IsZeroKbSeedingEnabled(string trackerUrl)
        {
            var config = _configService.GetConfiguration();
            var tracker = config.Trackers.FirstOrDefault(t => 
                trackerUrl.Contains(t.Url) || t.Url.Contains(trackerUrl));
            
            return tracker?.ZeroKbSeeding ?? false;
        }

        private string BuildAnnounceParams(TorrentInfo torrent, string peerId, int port, bool zeroKb, AppConfiguration config)
        {
            var uploaded = CalculateFakeUploaded(torrent, zeroKb, config);
            var downloaded = torrent.Size; // Assume full download
            var left = 0; // We're seeding, so nothing left to download

            var parameters = new Dictionary<string, string>
            {
                { "info_hash", UrlEncodeInfoHash(torrent.Hash) },
                { "peer_id", peerId },
                { "port", port.ToString() },
                { "uploaded", uploaded.ToString() },
                { "downloaded", downloaded.ToString() },
                { "left", left.ToString() },
                { "compact", "1" },
                { "numwant", "50" },
                { "event", "started" }
            };

            return string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        private long CalculateFakeUploaded(TorrentInfo torrent, bool zeroKb, AppConfiguration config)
        {
            if (zeroKb)
            {
                return 0;
            }

            // Calculate fake upload based on target ratio
            if (config.UploadRatioTarget > 0)
            {
                return (long)(torrent.Size * config.UploadRatioTarget);
            }

            // For infinite seeding, calculate a realistic upload amount
            var timeSeeding = (DateTime.Now - torrent.AddedDate).TotalHours;
            var avgUploadRate = _random.Next(config.MinUploadRate, config.MaxUploadRate) * 1024; // Convert to bytes
            return (long)(avgUploadRate * timeSeeding * 3600); // Upload in bytes
        }

        private string UrlEncodeInfoHash(string hash)
        {
            // Convert hex string to bytes
            var bytes = new byte[hash.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hash.Substring(i * 2, 2), 16);
            }

            // URL encode for tracker announce
            var encoded = new StringBuilder();
            foreach (var b in bytes)
            {
                encoded.Append($"%{b:X2}");
            }
            return encoded.ToString();
        }

        public string GeneratePeerId(string clientName)
        {
            // Generate peer ID in Azureus style: -XX####-############
            var prefix = GetClientPrefix(clientName);
            var randomPart = GenerateRandomString(12);
            return $"-{prefix}-{randomPart}";
        }

        private string GetClientPrefix(string clientName)
        {
            // Map client names to their peer ID prefixes
            return clientName switch
            {
                var c when c.Contains("qBittorrent-4.5") => "qB4500",
                var c when c.Contains("qBittorrent-4.4") => "qB4450",
                var c when c.Contains("Transmission") => "TR3000",
                var c when c.Contains("Deluge") => "DE2030",
                var c when c.Contains("uTorrent") => "UT3550",
                _ => "qB4500"
            };
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[_random.Next(chars.Length)])
                .ToArray());
        }
    }
}
