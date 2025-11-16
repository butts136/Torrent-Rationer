# Torrent Rationer - Troubleshooting Guide

## Application Not Showing Window?

**IMPORTANT**: This is a DESKTOP application, NOT a web application. It does not run on a port or in a browser. When you launch it, a window should open directly on your desktop.

### If No Window Appears:

1. **Check the Log Files**
   
   The application creates detailed log files. Open these locations:
   
   - **Windows**: `%APPDATA%\TorrentRationer\`
     - Copy and paste this in Windows Explorer: `%APPDATA%\TorrentRationer\`
   
   - **Linux**: `~/.config/TorrentRationer/` or `$HOME/.local/share/TorrentRationer/`
   
   Log files:
   - `startup.log` - Shows application startup process
   - `error.log` - Shows any initialization errors

2. **What to Look For in Logs**
   
   If the application is crashing or failing to start, the logs will show:
   - Which component failed to initialize
   - Error messages and stack traces
   - System information

3. **Common Issues**

   **Missing .NET Runtime**
   - If you downloaded the installer or portable version, .NET runtime should be included
   - If not, download from: https://dotnet.microsoft.com/download/dotnet/8.0

   **Antivirus Blocking**
   - Some antivirus software may block the application
   - Check your antivirus logs and whitelist TorrentRationer.exe

   **Graphics Driver Issues**
   - Ensure your graphics drivers are up to date
   - The application uses hardware acceleration for rendering

4. **Getting Help**

   If you still can't get the window to show:
   1. Check the log files in `%APPDATA%\TorrentRationer\`
   2. Take screenshots of any error messages
   3. Report the issue with the log file contents

## This is NOT a Web Application

- **No Browser Required**: The application runs as a standalone desktop program
- **No Ports**: It does not listen on any network ports
- **Direct Window**: When launched, it should open a window on your screen immediately
- **Similar to**: Microsoft Word, Notepad, or any other desktop application

## What Should Happen When You Launch?

1. Double-click `TorrentRationer.exe`
2. A window titled "Torrent Rationer" should open on your screen
3. You'll see two tabs: "Dashboard" and "Configuration"
4. The window should be centered on your screen at 1200x700 pixels

If this doesn't happen, check the logs as described above.
