using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using TorrentRationer.Models;
using TorrentRationer.Services;

namespace TorrentRationer.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly TorrentService _torrentService;
        private readonly ConfigurationService _configService;
        private readonly System.Timers.Timer _updateTimer;

        public DashboardViewModel(TorrentService torrentService, ConfigurationService configService)
        {
            _torrentService = torrentService;
            _configService = configService;

            LoadTorrentsCommand = ReactiveCommand.CreateFromTask(LoadTorrents);
            StartSeedingCommand = ReactiveCommand.Create(StartSeeding);
            StopSeedingCommand = ReactiveCommand.Create(StopSeeding);

            // Update statistics every 5 seconds
            _updateTimer = new System.Timers.Timer(5000);
            _updateTimer.Elapsed += (s, e) => 
            {
                try
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => UpdateStatistics());
                }
                catch { /* Ignore timer errors */ }
            };
            _updateTimer.Start();
            
            // Initial statistics update
            UpdateStatistics();
        }

        public ObservableCollection<TorrentInfo> Torrents => _torrentService.Torrents;

        private int _totalSeeding;
        public int TotalSeeding
        {
            get => _totalSeeding;
            set => this.RaiseAndSetIfChanged(ref _totalSeeding, value);
        }

        private double _averageRatio;
        public double AverageRatio
        {
            get => _averageRatio;
            set => this.RaiseAndSetIfChanged(ref _averageRatio, value);
        }

        private int _totalUploadSpeed;
        public int TotalUploadSpeed
        {
            get => _totalUploadSpeed;
            set => this.RaiseAndSetIfChanged(ref _totalUploadSpeed, value);
        }

        public ReactiveCommand<Unit, Unit> LoadTorrentsCommand { get; }
        public ReactiveCommand<Unit, Unit> StartSeedingCommand { get; }
        public ReactiveCommand<Unit, Unit> StopSeedingCommand { get; }

        private async Task LoadTorrents()
        {
            var config = _configService.GetConfiguration();
            if (!string.IsNullOrEmpty(config.DefaultTorrentPath))
            {
                await _torrentService.LoadTorrentsFromDirectory(config.DefaultTorrentPath);
                UpdateStatistics();
            }
        }

        private void StartSeeding()
        {
            _torrentService.StartAutoSeeding();
            UpdateStatistics();
        }

        private void StopSeeding()
        {
            foreach (var torrent in Torrents.Where(t => t.Status == "Seeding"))
            {
                _torrentService.StopSeeding(torrent);
            }
            UpdateStatistics();
        }

        public void UpdateStatistics()
        {
            TotalSeeding = Torrents.Count(t => t.Status == "Seeding");
            AverageRatio = Torrents.Any() ? Torrents.Average(t => t.FakeRatio) : 0;
            TotalUploadSpeed = Torrents.Sum(t => t.UploadSpeed);
        }
    }
}

