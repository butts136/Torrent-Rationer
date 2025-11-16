#!/bin/bash
# Installation script for Torrent Rationer on Linux

set -e

INSTALL_DIR="/usr/local/bin"
APP_NAME="TorrentRationer"
SOURCE_FILE="publish/linux-x64/$APP_NAME"

echo "======================================"
echo "Torrent Rationer Linux Installer"
echo "======================================"
echo ""

# Check if running as root
if [ "$EUID" -ne 0 ]; then 
    echo "This script requires root privileges to install to $INSTALL_DIR"
    echo "Please run with sudo: sudo ./install-linux.sh"
    echo ""
    echo "Alternatively, you can manually copy the executable:"
    echo "  cp $SOURCE_FILE ~/bin/$APP_NAME"
    exit 1
fi

# Check if source file exists
if [ ! -f "$SOURCE_FILE" ]; then
    echo "Error: Source file not found: $SOURCE_FILE"
    echo "Please build the application first using: bash build-linux.sh"
    exit 1
fi

# Copy executable
echo "Installing $APP_NAME to $INSTALL_DIR..."
cp "$SOURCE_FILE" "$INSTALL_DIR/$APP_NAME"
chmod +x "$INSTALL_DIR/$APP_NAME"

echo ""
echo "======================================"
echo "Installation completed successfully!"
echo "======================================"
echo ""
echo "You can now run the application from anywhere using:"
echo "  $APP_NAME"
echo ""
echo "To uninstall, run:"
echo "  sudo rm $INSTALL_DIR/$APP_NAME"
