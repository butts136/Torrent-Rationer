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
        private readonly Random _random = new();

        public ObservableCollection<TorrentInfo> Torrents => _torrents;

        public TorrentService(ConfigurationService configService)
        {
            _configService = configService;
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

        public void StartSeeding(TorrentInfo torrent)
        {
            torrent.Status = "Seeding";
            // Fake seeding logic will be implemented here
        }

        public void StopSeeding(TorrentInfo torrent)
        {
            torrent.Status = "Stopped";
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
    }
}
