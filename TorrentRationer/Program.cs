using System;
using System.Runtime.InteropServices;

namespace TorrentRationer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("   Torrent Rationer v1.0");
            Console.WriteLine("   Cross-Platform Application");
            Console.WriteLine("=================================");
            Console.WriteLine();
            
            // Display platform information
            string platform = GetPlatformName();
            Console.WriteLine($"Running on: {platform}");
            Console.WriteLine($"OS Description: {RuntimeInformation.OSDescription}");
            Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
            Console.WriteLine();
            
            Console.WriteLine("This is a Torrent Rationer application.");
            Console.WriteLine("Designed to work on both Windows and Linux.");
            Console.WriteLine();
            
            if (args.Length > 0)
            {
                Console.WriteLine("Arguments received:");
                foreach (var arg in args)
                {
                    Console.WriteLine($"  - {arg}");
                }
            }
            else
            {
                Console.WriteLine("No arguments provided.");
                Console.WriteLine("Usage: TorrentRationer [options]");
            }
            
            Console.WriteLine();
            
            // Only wait for key if running interactively
            try
            {
                if (Environment.UserInteractive && !Console.IsInputRedirected)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
            }
            catch
            {
                // Ignore if console is not available
            }
        }
        
        static string GetPlatformName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "macOS";
            else
                return "Unknown";
        }
    }
}
