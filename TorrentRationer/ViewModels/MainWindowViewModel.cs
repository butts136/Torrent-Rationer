using ReactiveUI;
using System.Reactive;
using TorrentRationer.Services;

namespace TorrentRationer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly ConfigurationViewModel _configurationViewModel;

        public MainWindowViewModel(
            DashboardViewModel dashboardViewModel,
            ConfigurationViewModel configurationViewModel)
        {
            _dashboardViewModel = dashboardViewModel;
            _configurationViewModel = configurationViewModel;
            _currentView = _dashboardViewModel;

            ShowDashboardCommand = ReactiveCommand.Create(() => { CurrentView = _dashboardViewModel; });
            ShowConfigurationCommand = ReactiveCommand.Create(() => { CurrentView = _configurationViewModel; });
        }

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public DashboardViewModel DashboardViewModel => _dashboardViewModel;
        public ConfigurationViewModel ConfigurationViewModel => _configurationViewModel;

        public ReactiveCommand<Unit, Unit> ShowDashboardCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowConfigurationCommand { get; }
    }
}
