# Torrent-Rationer

A cross-platform GUI application for managing torrent ratios through fake seeding. Built with C# and Avalonia UI for Windows and Linux.

## Features

- ✅ **Cross-platform GUI** (Windows and Linux) using Avalonia UI
- ✅ **Fake Seeding**: Announce to trackers with fake upload ratios without actual data transfer
- ✅ **Two-Tab Interface**: Dashboard for monitoring and Configuration for settings
- ✅ **Tracker Management**: Support for multiple trackers with individual 0 KB/s seeding mode
- ✅ **Torrent Client Emulation**: Mimics popular clients (qBittorrent, Transmission, Deluge, uTorrent)
- ✅ **Ratio Control**: Set target ratios or use infinite seeding mode
- ✅ **Dark Mode**: Built-in theme support with live switching
- ✅ **Auto-start**: Windows startup integration
- ✅ **Configuration Persistence**: Automatic save/load of settings

## Important Disclaimer

⚠️ **Warning**: This application is designed to send fake announce data to torrent trackers. Using this tool may violate the terms of service of many private trackers and could result in account bans. This software is provided for educational purposes only. Use at your own risk and only on trackers where you have explicit permission to do so.

## How It Works

Torrent-Rationer works by:
1. Loading `.torrent` files from a specified directory
2. Parsing torrent metadata to extract tracker URLs and info hashes
3. Generating peer IDs that match popular torrent clients
4. Sending HTTP announce requests to trackers with fake upload/download statistics
5. Periodically re-announcing (every 30 minutes) to maintain presence on the tracker
6. Optionally using "0 KB/s seeding mode" to stay connected without reporting any uploads

This allows users to improve their tracker ratios without actually seeding files.

## System Requirements

### For Users
- **Windows**: Windows 10/11 (x64)
- **Linux**: Any modern x64 Linux distribution with X11 or Wayland

### For Developers
- .NET 8.0 SDK or later
- For Windows installer creation: [Inno Setup 6.0+](https://jrsoftware.org/isinfo.php)

## Installation

### Windows

#### Option 1: Using the Installer (Recommended)
1. Download `TorrentRationer-Setup-1.0.0.exe` from the releases page
2. Run the installer
3. Follow the installation wizard
4. Launch "Torrent Rationer" from the Start Menu

#### Option 2: Portable Version (No Installation)
1. Download `TorrentRationer-Portable-Windows-x64.zip` from the releases page
2. Extract the ZIP file to your desired location
3. Run `TorrentRationer.exe` directly - no installation required
4. All settings and data will be stored in your AppData folder

#### Option 3: Standalone Executable
1. Download `TorrentRationer.exe` from the releases page
2. Place it in your desired location
3. Run directly - no installation required

### Linux

#### Option 1: Portable Package
1. Download `TorrentRationer-Portable-Linux-x64.tar.gz` from the releases page
2. Extract the archive:
   ```bash
   tar -xzf TorrentRationer-Portable-Linux-x64.tar.gz
   ```
3. Navigate to the extracted folder:
   ```bash
   cd linux-x64
   ```
4. Make it executable:
   ```bash
   chmod +x TorrentRationer
   ```
5. Run it:
   ```bash
   ./TorrentRationer
   ```

#### Option 2: Standalone Executable
1. Download the `TorrentRationer` executable for Linux
2. Make it executable:
   ```bash
   chmod +x TorrentRationer
   ```
3. Run it:
   ```bash
   ./TorrentRationer
   ```
4. (Optional) Move to a system path:
   ```bash
   sudo mv TorrentRationer /usr/local/bin/
   ```

## Usage

### Getting Started

1. **Launch the application**: The GUI will open with two tabs: Dashboard and Configuration

2. **Configure Settings** (Configuration Tab):
   - Set minimum and maximum upload rates (KB/s)
   - Select a torrent client to emulate (e.g., qBittorrent 4.5.0)
   - Set the number of simultaneous seeds
   - Configure upload ratio target (-1 for infinite seeding)
   - Set the default path for `.torrent` files
   - Add trackers and enable "0 KB/s seeding" for specific ones
   - Enable dark mode if desired
   - Enable auto-start with Windows (Windows only)
   - Click "Save Configuration" to persist settings

3. **Load and Seed Torrents** (Dashboard Tab):
   - Click "Load Torrents" to scan the configured directory for `.torrent` files
   - Click "Start Seeding" to begin fake seeding (announces to trackers)
   - Monitor statistics: total seeding, average ratio, upload speed
   - Click "Stop All" to stop all seeding

### Configuration Options

- **Minimum/Max Upload Rate**: Controls the simulated upload speed range
- **Torrent Client**: Determines the peer ID and user agent sent to trackers
- **Simultaneous Seeds**: Maximum number of torrents to seed at once
- **Upload Ratio Target**: Target ratio to report (-1 for infinite)
- **Keep Seeding with No Peers**: Continue announcing even without peers
- **0 KB/s Seeding**: Maintain tracker presence without reporting uploads
- **Auto-start**: Launch automatically when Windows starts

### Dashboard

The dashboard displays:
- **Statistics**: Total seeding torrents, average ratio, total upload speed
- **Torrent List**: Name, size, real ratio, fake ratio, status, upload speed
- **Controls**: Load, Start, and Stop buttons

## Building from Source

### Automated Builds (GitHub Actions)

This repository includes a GitHub Actions workflow that automatically builds the application and creates installers on every push:

- **Workflow**: `.github/workflows/build-and-deploy.yml`
- **Triggers**: Push to main or PR branches, manual workflow dispatch
- **Outputs**:
  - Windows executable (`TorrentRationer.exe`)
  - Windows portable package (`TorrentRationer-Portable-Windows-x64.zip`)
  - Windows installer (`TorrentRationer-Setup-1.0.0.exe`)
  - Linux executable (`TorrentRationer`)
  - Linux portable package (`TorrentRationer-Portable-Linux-x64.tar.gz`)

To download artifacts:
1. Go to the [Actions tab](../../actions) in the repository
2. Click on the latest workflow run
3. Download the artifacts from the "Artifacts" section

### Prerequisites
- Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- Clone this repository

### Build for All Platforms (Recommended)

Using PowerShell (works on Windows, Linux, and macOS):
```powershell
pwsh build-all.ps1
```

This will create executables for both Windows and Linux in the `publish/` directory.

### Build for Windows Only

Using PowerShell:
```powershell
pwsh build-windows.ps1
```

Or manually:
```bash
dotnet publish TorrentRationer/TorrentRationer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/win-x64
```

### Build for Linux Only

Using Bash:
```bash
bash build-linux.sh
```

Or manually:
```bash
dotnet publish TorrentRationer/TorrentRationer.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o publish/linux-x64
```

### Creating Windows Installer

After building for Windows:

1. Install [Inno Setup](https://jrsoftware.org/isinfo.php)
2. Run the Inno Setup Compiler on the installer script:
   ```bash
   iscc installer.iss
   ```
3. The installer will be created in the `installer/` directory as `TorrentRationer-Setup-1.0.0.exe`

## Usage

Run the application from command line:

```bash
# Windows
TorrentRationer.exe [options]

# Linux
./TorrentRationer [options]
```

The application will display:
- Application title and version
- Current platform information
- Operating system details
- .NET Framework version
- Any command-line arguments passed

## Development

### Project Structure
```
Torrent-Rationer/
├── TorrentRationer/           # Main application source code
│   ├── Program.cs             # Entry point
│   └── TorrentRationer.csproj # Project file
├── build-windows.ps1          # Windows build script
├── build-linux.sh             # Linux build script
├── build-all.ps1              # Build for all platforms
├── installer.iss              # Inno Setup installer script
└── README.md                  # This file
```

### Building and Testing

1. **Build the project:**
   ```bash
   dotnet build TorrentRationer/TorrentRationer.csproj
   ```

2. **Run in development mode:**
   ```bash
   dotnet run --project TorrentRationer/TorrentRationer.csproj
   ```

3. **Run with arguments:**
   ```bash
   dotnet run --project TorrentRationer/TorrentRationer.csproj -- arg1 arg2
   ```

### Cross-Platform Considerations

The application uses:
- `RuntimeInformation` to detect the current platform
- `Environment.UserInteractive` to handle console availability
- Self-contained deployment to avoid runtime dependencies
- Single-file publishing for easy distribution

## License

[Your License Here]

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For issues and questions, please use the GitHub Issues page.