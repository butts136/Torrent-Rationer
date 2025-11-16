using ReactiveUI;
using System.Collections.ObjectModel;
using TorrentRationer.Models;
using TorrentRationer.Services;

namespace TorrentRationer.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly TorrentService _torrentService;
        private readonly ConfigurationService _configService;

        public DashboardViewModel(TorrentService torrentService, ConfigurationService configService)
        {
            _torrentService = torrentService;
            _configService = configService;
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

        public void UpdateStatistics()
        {
            TotalSeeding = Torrents.Count(t => t.Status == "Seeding");
            AverageRatio = Torrents.Any() ? Torrents.Average(t => t.FakeRatio) : 0;
            TotalUploadSpeed = Torrents.Sum(t => t.UploadSpeed);
        }
    }
}
