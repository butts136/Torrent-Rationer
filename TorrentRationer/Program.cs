using Avalonia;
using System;
using System.IO;

namespace TorrentRationer
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Clear previous logs on startup
                ClearStartupLog();
                
                // Log startup
                LogStartup("=== APPLICATION STARTING ===");
                LogStartup($"Command line args: {string.Join(" ", args)}");
                LogStartup($"Current directory: {Environment.CurrentDirectory}");
                LogStartup($"OS: {Environment.OSVersion}");
                LogStartup($".NET: {Environment.Version}");
                LogStartup($"Log path: {GetLogPath()}");
                
                LogStartup("Building Avalonia app...");
                var app = BuildAvaloniaApp();
                
                LogStartup("Starting classic desktop lifetime...");
                app.StartWithClassicDesktopLifetime(args);
                
                LogStartup("=== APPLICATION EXITED NORMALLY ===");
            }
            catch (Exception ex)
            {
                var errorMsg = $"FATAL ERROR in Main:\n{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
                LogStartup(errorMsg);
                Console.WriteLine(errorMsg);
                Console.WriteLine($"\nLog file location: {GetLogPath()}");
                throw;
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static void ClearStartupLog()
        {
            try
            {
                var logPath = GetLogPath();
                if (File.Exists(logPath))
                {
                    File.Delete(logPath);
                }
            }
            catch { /* Ignore */ }
        }

        private static void LogStartup(string message)
        {
            try
            {
                var logPath = GetLogPath();
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
                File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}\n");
            }
            catch { /* Ignore logging errors */ }
        }

        private static string GetLogPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TorrentRationer", "startup.log");
        }
    }
}
