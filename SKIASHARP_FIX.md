# SkiaSharp Initialization Error - SOLUTION

## Error Message
```
The type initializer for 'SkiaSharp.SKImageInfo' threw an exception
```

## What This Means
This error occurs when SkiaSharp (the graphics rendering library used by Avalonia) cannot initialize. This is caused by missing Visual C++ Runtime libraries on Windows.

## SOLUTION FOR INSTALLER USERS

**The installer now automatically checks for Visual C++ Redistributable!**

When you run the installer:
1. It will check if Visual C++ Runtime is installed
2. If missing, it will prompt you to download it
3. Click "Yes" to open the download page
4. Install the VC++ Redistributable
5. Run the Torrent Rationer installer again

## SOLUTION FOR PORTABLE VERSION USERS

### Step 1: Download and Install Visual C++ Redistributable

1. Download the installer from Microsoft:
   - **Direct link**: https://aka.ms/vs/17/release/vc_redist.x64.exe
   - Or search for "Visual C++ Redistributable latest"

2. Run the installer

3. Restart your computer (recommended)

4. Try running Torrent Rationer again

### Step 2: If That Doesn't Work - Install ALL Redistributables

If Step 1 doesn't work, you may need multiple versions:

1. Download from: https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads

2. Install both:
   - vc_redist.x64.exe (64-bit)
   - vc_redist.x86.exe (32-bit, for compatibility)

### Step 3: Update Graphics Drivers

Outdated graphics drivers can also cause this issue:

1. Visit your GPU manufacturer's website:
   - NVIDIA: https://www.nvidia.com/download/index.aspx
   - AMD: https://www.amd.com/en/support
   - Intel: https://www.intel.com/content/www/us/en/download-center/home.html

2. Download and install the latest driver for your GPU

3. Restart your computer

## Why This Happens

The error occurs because:
- SkiaSharp is a cross-platform 2D graphics library
- It relies on native C++ libraries (libSkiaSharp.dll on Windows)
- These native libraries depend on Visual C++ Runtime
- If the runtime is missing or corrupted, SkiaSharp cannot initialize
- Without SkiaSharp, Avalonia UI cannot render graphics

## For Basic Users

**TL;DR**: This application needs a free Windows component to work. 

1. Download this: https://aka.ms/vs/17/release/vc_redist.x64.exe
2. Double-click to install it
3. Restart your computer
4. Run Torrent Rationer again - it will now work!

This is a one-time installation. After this, the application will work without any issues on your computer.

## Verification

After installing the Visual C++ Redistributable:

1. Check if these files exist in `C:\Windows\System32\`:
   - `vcruntime140.dll`
   - `msvcp140.dll`

2. If they exist, the redistributable is installed correctly

3. Run Torrent Rationer again - it should now work!

## Still Having Issues?

1. Check the application logs at: `%APPDATA%\TorrentRationer\startup.log`
2. Look for specific error messages
3. Report the issue with the log contents
