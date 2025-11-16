using System.Collections.ObjectModel;
using TorrentRationer.Models;
using BencodeNET.Parsing;
using BencodeNET.Torrents;
using System.Security.Cryptography;
using System.Text;

namespace TorrentRationer.Services
{
    public class TorrentService
    {
        private readonly ObservableCollection<TorrentInfo> _torrents = new();
        private readonly ConfigurationService _configService;
        private readonly TrackerAnnounceService _trackerService;
        private readonly Random _random = new();
        private readonly Dictionary<string, System.Threading.Timer> _announceTimers = new();
        private readonly string _peerId;
        private readonly int _port;

        public ObservableCollection<TorrentInfo> Torrents => _torrents;

        public TorrentService(ConfigurationService configService)
        {
            _configService = configService;
            _trackerService = new TrackerAnnounceService(configService);
            
            var config = configService.GetConfiguration();
            _peerId = _trackerService.GeneratePeerId(config.TorrentClient);
            _port = _random.Next(6881, 6999); // Random port in typical torrent range
        }

        public async Task LoadTorrentsFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                return;

            var torrentFiles = Directory.GetFiles(directory, "*.torrent");
            
            foreach (var file in torrentFiles)
            {
                try
                {
                    await LoadTorrentFile(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading torrent {file}: {ex.Message}");
                }
            }
        }

        public async Task LoadTorrentFile(string filePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    var parser = new BencodeParser();
                    var torrent = parser.Parse<Torrent>(filePath);

                    var torrentInfo = new TorrentInfo
                    {
                        Name = torrent.DisplayName ?? Path.GetFileNameWithoutExtension(filePath),
                        Hash = CalculateInfoHash(torrent),
                        Size = torrent.TotalSize,
                        UploadRatio = 0,
                        FakeRatio = 0,
                        Status = "Loaded",
                        AddedDate = DateTime.Now,
                        Trackers = torrent.Trackers?.SelectMany(t => t.Select(u => u.ToString())).ToList() ?? new()
                    };

                    _torrents.Add(torrentInfo);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to parse torrent file: {ex.Message}");
                }
            });
        }

        public async void StartSeeding(TorrentInfo torrent)
        {
            torrent.Status = "Seeding";
            
            // Start announcing to trackers
            foreach (var tracker in torrent.Trackers)
            {
                await AnnounceToTracker(torrent, tracker);
            }
            
            // Set up periodic announces (every 30 minutes)
            var timer = new System.Threading.Timer(
                async _ => await PeriodicAnnounce(torrent),
                null,
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMinutes(30)
            );
            
            _announceTimers[torrent.Hash] = timer;
        }

        public void StopSeeding(TorrentInfo torrent)
        {
            torrent.Status = "Stopped";
            
            // Stop announce timer
            if (_announceTimers.TryGetValue(torrent.Hash, out var timer))
            {
                timer.Dispose();
                _announceTimers.Remove(torrent.Hash);
            }
        }

        private async Task PeriodicAnnounce(TorrentInfo torrent)
        {
            foreach (var tracker in torrent.Trackers)
            {
                await AnnounceToTracker(torrent, tracker);
            }
        }

        private async Task AnnounceToTracker(TorrentInfo torrent, string tracker)
        {
            try
            {
                var success = await _trackerService.AnnounceToTracker(torrent, tracker, _peerId, _port);
                if (success)
                {
                    UpdateTorrentStats(torrent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Announce failed for {torrent.Name} to {tracker}: {ex.Message}");
            }
        }

        private void UpdateTorrentStats(TorrentInfo torrent)
        {
            var config = _configService.GetConfiguration();
            
            // Simulate upload speed
            torrent.UploadSpeed = _random.Next(config.MinUploadRate, config.MaxUploadRate);
            
            // Update fake ratio
            if (config.UploadRatioTarget > 0)
            {
                torrent.FakeRatio = config.UploadRatioTarget;
            }
            else
            {
                // Gradually increase ratio
                torrent.FakeRatio += _random.NextDouble() * 0.1;
            }
        }

        public void UpdateFakeRatio(TorrentInfo torrent, double ratio)
        {
            torrent.FakeRatio = ratio;
        }

        private string CalculateInfoHash(Torrent torrent)
        {
            try
            {
                // Calculate SHA1 hash of the info dictionary
                var infoDict = torrent.OriginalInfoHashBytes;
                if (infoDict != null)
                {
                    return BitConverter.ToString(infoDict).Replace("-", "").ToLower();
                }
            }
            catch { }
            
            // Fallback to random hash if calculation fails
            return Guid.NewGuid().ToString("N").Substring(0, 40);
        }

        public void StartAutoSeeding()
        {
            var config = _configService.GetConfiguration();
            var torrentsToSeed = _torrents
                .Where(t => t.Status == "Loaded")
                .Take(config.SimultaneousSeeds)
                .ToList();

            foreach (var torrent in torrentsToSeed)
            {
                StartSeeding(torrent);
            }
        }
    }
}
