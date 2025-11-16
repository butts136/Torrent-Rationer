using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TorrentRationer.ViewModels;
using TorrentRationer.Views;
using TorrentRationer.Services;
using Avalonia.Styling;

namespace TorrentRationer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Initialize services
                var configService = new ConfigurationService();
                var torrentService = new TorrentService(configService);

                // Initialize ViewModels
                var dashboardViewModel = new DashboardViewModel(torrentService, configService);
                var configurationViewModel = new ConfigurationViewModel(configService);
                var mainViewModel = new MainWindowViewModel(dashboardViewModel, configurationViewModel);

                // Apply theme based on configuration
                var config = configService.GetConfiguration();
                RequestedThemeVariant = config.DarkMode ? ThemeVariant.Dark : ThemeVariant.Light;

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
