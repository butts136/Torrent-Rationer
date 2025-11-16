using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TorrentRationer.ViewModels;
using TorrentRationer.Views;
using TorrentRationer.Services;
using Avalonia.Styling;
using ReactiveUI;

namespace TorrentRationer
{
    public partial class App : Application
    {
        private ConfigurationService? _configService;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Initialize services
                _configService = new ConfigurationService();
                var torrentService = new TorrentService(_configService);
                var trackerService = new TrackerAnnounceService(_configService);

                // Initialize ViewModels
                var dashboardViewModel = new DashboardViewModel(torrentService, _configService);
                var configurationViewModel = new ConfigurationViewModel(_configService);
                var mainViewModel = new MainWindowViewModel(dashboardViewModel, configurationViewModel);

                // Apply theme based on configuration
                var config = _configService.GetConfiguration();
                ApplyTheme(config.DarkMode);

                // Subscribe to dark mode changes
                configurationViewModel.WhenAnyValue(x => x.DarkMode)
                    .Subscribe(darkMode => ApplyTheme(darkMode));

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };

                // Setup auto-start if enabled
                SetupAutoStart(config.AutoStartWithWindows);
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ApplyTheme(bool darkMode)
        {
            RequestedThemeVariant = darkMode ? ThemeVariant.Dark : ThemeVariant.Light;
        }

        private void SetupAutoStart(bool enabled)
        {
            if (OperatingSystem.IsWindows())
            {
                SetupWindowsAutoStart(enabled);
            }
            // Linux auto-start could be implemented with .desktop files in ~/.config/autostart/
        }

        private void SetupWindowsAutoStart(bool enabled)
        {
            try
            {
                var appPath = Environment.ProcessPath;
                if (appPath == null) return;

                var keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                var appName = "TorrentRationer";

                if (OperatingSystem.IsWindows())
                {
                    using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath, true);
                    if (key != null)
                    {
                        if (enabled)
                        {
                            key.SetValue(appName, $"\"{appPath}\"");
                        }
                        else
                        {
                            key.DeleteValue(appName, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to setup auto-start: {ex.Message}");
            }
        }
    }
}
