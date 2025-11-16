using Avalonia;
using System;
using System.IO;
using System.Runtime.InteropServices;

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
                LogStartup($"Runtime: {RuntimeInformation.FrameworkDescription}");
                LogStartup($"Architecture: {RuntimeInformation.ProcessArchitecture}");
                LogStartup($"Log path: {GetLogPath()}");
                
                // Check for SkiaSharp native libraries
                CheckSkiaSharpDependencies();
                
                LogStartup("Building Avalonia app...");
                var app = BuildAvaloniaApp();
                
                LogStartup("Starting classic desktop lifetime...");
                app.StartWithClassicDesktopLifetime(args);
                
                LogStartup("=== APPLICATION EXITED NORMALLY ===");
            }
            catch (Exception ex)
            {
                var errorMsg = $"FATAL ERROR in Main:\n{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
                
                // Check for SkiaSharp specific error
                if (ex.ToString().Contains("SkiaSharp") || ex.ToString().Contains("SKImageInfo"))
                {
                    errorMsg += "\n\n=== SKIASHARP INITIALIZATION ERROR ===\n";
                    errorMsg += "This error indicates missing graphics dependencies.\n";
                    errorMsg += "Solutions:\n";
                    errorMsg += "1. Install Visual C++ Redistributable from:\n";
                    errorMsg += "   https://aka.ms/vs/17/release/vc_redist.x64.exe\n";
                    errorMsg += "2. Update your graphics drivers\n";
                    errorMsg += "3. Try running the application as Administrator\n";
                }
                
                LogStartup(errorMsg);
                Console.WriteLine(errorMsg);
                Console.WriteLine($"\nLog file location: {GetLogPath()}");
                Console.WriteLine("\nPress any key to exit...");
                
                try { Console.ReadKey(); } catch { }
                
                throw;
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static void CheckSkiaSharpDependencies()
        {
            try
            {
                LogStartup("Checking SkiaSharp dependencies...");
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Check for Visual C++ Runtime
                    var systemDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
                    var vcRuntime = Path.Combine(systemDir, "vcruntime140.dll");
                    var msvcp = Path.Combine(systemDir, "msvcp140.dll");
                    
                    LogStartup($"Checking for vcruntime140.dll: {File.Exists(vcRuntime)}");
                    LogStartup($"Checking for msvcp140.dll: {File.Exists(msvcp)}");
                    
                    if (!File.Exists(vcRuntime) || !File.Exists(msvcp))
                    {
                        LogStartup("WARNING: Visual C++ Runtime files not found!");
                        LogStartup("Download from: https://aka.ms/vs/17/release/vc_redist.x64.exe");
                    }
                }
                
                LogStartup("SkiaSharp dependency check completed");
            }
            catch (Exception ex)
            {
                LogStartup($"Error checking dependencies: {ex.Message}");
            }
        }

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
