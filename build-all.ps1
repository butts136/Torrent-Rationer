#!/usr/bin/env pwsh
# Build script for both Windows and Linux

param(
    [string]$Configuration = "Release"
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Building Torrent Rationer for all platforms" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Build for Windows x64
Write-Host "Building for Windows x64..." -ForegroundColor Green
dotnet publish TorrentRationer/TorrentRationer.csproj `
    -c $Configuration `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o publish/win-x64

if ($LASTEXITCODE -ne 0) {
    Write-Host "Windows build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Windows build successful!" -ForegroundColor Green
Write-Host ""

# Build for Linux x64
Write-Host "Building for Linux x64..." -ForegroundColor Green
dotnet publish TorrentRationer/TorrentRationer.csproj `
    -c $Configuration `
    -r linux-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o publish/linux-x64

if ($LASTEXITCODE -ne 0) {
    Write-Host "Linux build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Linux build successful!" -ForegroundColor Green
Write-Host ""

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "All builds completed successfully!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Output locations:" -ForegroundColor Yellow
Write-Host "  Windows: publish/win-x64/TorrentRationer.exe" -ForegroundColor Cyan
Write-Host "  Linux:   publish/linux-x64/TorrentRationer" -ForegroundColor Cyan
Write-Host ""
Write-Host "To create Windows installer:" -ForegroundColor Yellow
Write-Host "  1. Install Inno Setup from https://jrsoftware.org/isinfo.php" -ForegroundColor Cyan
Write-Host "  2. Run: iscc installer.iss" -ForegroundColor Cyan
Write-Host "  3. Installer will be in installer/ folder" -ForegroundColor Cyan
