#!/usr/bin/env pwsh
# Build script for Windows executable

param(
    [string]$Configuration = "Release"
)

Write-Host "Building Torrent Rationer for Windows..." -ForegroundColor Green

# Build for Windows x64
dotnet publish TorrentRationer/TorrentRationer.csproj `
    -c $Configuration `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -o publish/win-x64

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild successful!" -ForegroundColor Green
    Write-Host "Output location: publish/win-x64/TorrentRationer.exe" -ForegroundColor Cyan
} else {
    Write-Host "`nBuild failed!" -ForegroundColor Red
    exit 1
}
