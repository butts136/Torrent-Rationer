# Torrent-Rationer

A cross-platform C# application that runs on both Windows and Linux.

## Features

- ✅ Cross-platform compatibility (Windows and Linux)
- ✅ Self-contained executable (no .NET runtime required)
- ✅ Windows installer (.exe) support
- ✅ Simple command-line interface
- ✅ Platform detection and information display

## System Requirements

### For Users
- **Windows**: Windows 10/11 (x64)
- **Linux**: Any modern x64 Linux distribution

### For Developers
- .NET 8.0 SDK or later
- For Windows installer creation: [Inno Setup 6.0+](https://jrsoftware.org/isinfo.php)

## Installation

### Windows

#### Option 1: Using the Installer (Recommended)
1. Download `TorrentRationer-Setup-1.0.0.exe` from the releases page
2. Run the installer
3. Follow the installation wizard
4. Optionally add to system PATH during installation

#### Option 2: Standalone Executable
1. Download `TorrentRationer.exe` from the releases page
2. Place it in your desired location
3. Run directly - no installation required

### Linux

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

## Building from Source

### Automated Builds (GitHub Actions)

This repository includes a GitHub Actions workflow that automatically builds the application and creates installers on every push:

- **Workflow**: `.github/workflows/build-and-deploy.yml`
- **Triggers**: Push to main or PR branches, manual workflow dispatch
- **Outputs**:
  - Windows executable (`TorrentRationer.exe`)
  - Windows installer (`TorrentRationer-Setup-1.0.0.exe`)
  - Linux executable (`TorrentRationer`)

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