using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TorrentRationer.ViewModels;
using TorrentRationer.Views;
using TorrentRationer.Services;
using Avalonia.Styling;
using ReactiveUI;
using System.IO;

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
                // Configure shutdown mode
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;
                
                try
                {
                    LogMessage("Starting initialization...");
                    
                    // Initialize services
                    _configService = new ConfigurationService();
                    LogMessage("ConfigurationService created");
                    
                    var torrentService = new TorrentService(_configService);
                    LogMessage("TorrentService created");
                    
                    var trackerService = new TrackerAnnounceService(_configService);
                    LogMessage("TrackerAnnounceService created");

                    // Initialize ViewModels
                    var dashboardViewModel = new DashboardViewModel(torrentService, _configService);
                    LogMessage("DashboardViewModel created");
                    
                    var configurationViewModel = new ConfigurationViewModel(_configService);
                    LogMessage("ConfigurationViewModel created");
                    
                    var mainViewModel = new MainWindowViewModel(dashboardViewModel, configurationViewModel);
                    LogMessage("MainWindowViewModel created");

                    // Apply theme based on configuration
                    var config = _configService.GetConfiguration();
                    ApplyTheme(config.DarkMode);
                    LogMessage($"Theme applied: {(config.DarkMode ? "Dark" : "Light")}");

                    // Subscribe to dark mode changes
                    configurationViewModel.WhenAnyValue(x => x.DarkMode)
                        .Subscribe(darkMode => ApplyTheme(darkMode));

                    // Create and assign the main window - it will be shown automatically
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = mainViewModel
                    };
                    
                    LogMessage("MainWindow created and assigned");

                    // Setup auto-start if enabled
                    SetupAutoStart(config.AutoStartWithWindows);
                    
                    LogMessage("Initialization completed successfully");
                }
                catch (Exception ex)
                {
                    // Log any initialization errors
                    var errorMsg = $"FATAL ERROR during initialization:\n{ex.Message}\n{ex.StackTrace}";
                    LogMessage(errorMsg);
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    
                    // Create a simple error window that will definitely show
                    try
                    {
                        desktop.MainWindow = CreateErrorWindow(ex);
                        LogMessage("Error window created");
                    }
                    catch (Exception ex2)
                    {
                        LogMessage($"Failed to create error window: {ex2.Message}");
                        // Last resort - try to create the most basic window possible
                        desktop.MainWindow = new Avalonia.Controls.Window
                        {
                            Title = "Torrent Rationer - Critical Error",
                            Width = 500,
                            Height = 300,
                            Content = new Avalonia.Controls.TextBlock 
                            { 
                                Text = $"Critical Error:\n{ex.Message}\n\nCheck logs in:\n{GetLogDirectory()}"
                            }
                        };
                    }
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        private Avalonia.Controls.Window CreateErrorWindow(Exception ex)
        {
            var window = new Avalonia.Controls.Window
            {
                Title = "Torrent Rationer - Initialization Error",
                Width = 600,
                Height = 400,
                WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterScreen
            };

            var scrollViewer = new Avalonia.Controls.ScrollViewer();
            var textBlock = new Avalonia.Controls.TextBlock
            {
                Text = $"An error occurred during initialization:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}\n\nLog directory:\n{GetLogDirectory()}",
                Margin = new Avalonia.Thickness(10),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };
            
            scrollViewer.Content = textBlock;
            window.Content = scrollViewer;
            
            return window;
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
                LogMessage($"Failed to setup auto-start: {ex.Message}");
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                var logPath = Path.Combine(GetLogDirectory(), "error.log");
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
                File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n");
            }
            catch { /* Ignore log errors */ }
        }

        private string GetLogDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TorrentRationer");
        }
    }
}
