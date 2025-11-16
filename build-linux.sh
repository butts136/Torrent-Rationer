#!/bin/bash
# Build script for Linux executable

CONFIGURATION="${1:-Release}"

echo "Building Torrent Rationer for Linux..."

# Build for Linux x64
dotnet publish TorrentRationer/TorrentRationer.csproj \
    -c "$CONFIGURATION" \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=false \
    -o publish/linux-x64

if [ $? -eq 0 ]; then
    echo ""
    echo "Build successful!"
    echo "Output location: publish/linux-x64/TorrentRationer"
    chmod +x publish/linux-x64/TorrentRationer
else
    echo ""
    echo "Build failed!"
    exit 1
fi
