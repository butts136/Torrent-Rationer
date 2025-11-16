using ReactiveUI;
using System.Reactive;
using TorrentRationer.Models;
using TorrentRationer.Services;

namespace TorrentRationer.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly ConfigurationService _configService;
        private AppConfiguration _config;
        private int _minUploadRate;
        private int _maxUploadRate;
        private string _torrentClient;
        private int _simultaneousSeeds;
        private double _uploadRatioTarget;
        private bool _keepSeedingWithNoPeers;
        private string _defaultTorrentPath;
        private bool _autoStartWithWindows;
        private bool _darkMode;
        private List<TrackerConfig> _trackers;

        public ConfigurationViewModel(ConfigurationService configService)
        {
            _configService = configService;
            _config = configService.GetConfiguration();
            
            // Initialize fields from config
            _minUploadRate = _config.MinUploadRate;
            _maxUploadRate = _config.MaxUploadRate;
            _torrentClient = _config.TorrentClient;
            _simultaneousSeeds = _config.SimultaneousSeeds;
            _uploadRatioTarget = _config.UploadRatioTarget;
            _keepSeedingWithNoPeers = _config.KeepSeedingWithNoPeers;
            _defaultTorrentPath = _config.DefaultTorrentPath;
            _autoStartWithWindows = _config.AutoStartWithWindows;
            _darkMode = _config.DarkMode;
            _trackers = _config.Trackers;
            
            SaveCommand = ReactiveCommand.Create(SaveConfiguration);
            BrowseTorrentPathCommand = ReactiveCommand.CreateFromTask(BrowseTorrentPath);
            AddTrackerCommand = ReactiveCommand.Create(AddTracker);
        }

        public int MinUploadRate
        {
            get => _minUploadRate;
            set
            {
                this.RaiseAndSetIfChanged(ref _minUploadRate, value);
                _config.MinUploadRate = value;
            }
        }

        public int MaxUploadRate
        {
            get => _maxUploadRate;
            set
            {
                this.RaiseAndSetIfChanged(ref _maxUploadRate, value);
                _config.MaxUploadRate = value;
            }
        }

        public string TorrentClient
        {
            get => _torrentClient;
            set
            {
                this.RaiseAndSetIfChanged(ref _torrentClient, value);
                _config.TorrentClient = value;
            }
        }

        public int SimultaneousSeeds
        {
            get => _simultaneousSeeds;
            set
            {
                this.RaiseAndSetIfChanged(ref _simultaneousSeeds, value);
                _config.SimultaneousSeeds = value;
            }
        }

        public double UploadRatioTarget
        {
            get => _uploadRatioTarget;
            set
            {
                this.RaiseAndSetIfChanged(ref _uploadRatioTarget, value);
                _config.UploadRatioTarget = value;
            }
        }

        public bool KeepSeedingWithNoPeers
        {
            get => _keepSeedingWithNoPeers;
            set
            {
                this.RaiseAndSetIfChanged(ref _keepSeedingWithNoPeers, value);
                _config.KeepSeedingWithNoPeers = value;
            }
        }

        public string DefaultTorrentPath
        {
            get => _defaultTorrentPath;
            set
            {
                this.RaiseAndSetIfChanged(ref _defaultTorrentPath, value);
                _config.DefaultTorrentPath = value;
            }
        }

        public bool AutoStartWithWindows
        {
            get => _autoStartWithWindows;
            set
            {
                this.RaiseAndSetIfChanged(ref _autoStartWithWindows, value);
                _config.AutoStartWithWindows = value;
            }
        }

        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                this.RaiseAndSetIfChanged(ref _darkMode, value);
                _config.DarkMode = value;
            }
        }

        public List<TrackerConfig> Trackers
        {
            get => _trackers;
            set => this.RaiseAndSetIfChanged(ref _trackers, value);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> BrowseTorrentPathCommand { get; }
        public ReactiveCommand<Unit, Unit> AddTrackerCommand { get; }

        private void SaveConfiguration()
        {
            _configService.SaveConfiguration(_config);
        }

        private async Task BrowseTorrentPath()
        {
            // This will be implemented with platform-specific file picker
            await Task.CompletedTask;
        }

        private void AddTracker()
        {
            _config.Trackers.Add(new TrackerConfig 
            { 
                Name = "New Tracker",
                Url = "",
                ZeroKbSeeding = false,
                IsActive = true
            });
            this.RaisePropertyChanged(nameof(Trackers));
        }
    }
}
