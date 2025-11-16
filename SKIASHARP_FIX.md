# SkiaSharp Initialization Error - SOLUTION

## Error Message
```
The type initializer for 'SkiaSharp.SKImageInfo' threw an exception
```

## What This Means
This error occurs when SkiaSharp (the graphics rendering library used by Avalonia) cannot initialize. This was caused by native libraries being bundled inside the single-file executable, preventing SkiaSharp from loading them properly.

## FIXED IN LATEST VERSION

**This issue has been fixed!** The application now correctly extracts native libraries (libSkiaSharp.dll/so and libHarfBuzzSharp.dll/so) as separate files during publishing. If you're still seeing this error, please ensure you're using the latest version.

### How the Fix Works

The application is published as a single-file executable for convenience, but SkiaSharp's native graphics libraries need to be available as separate files. The build process now:

1. Bundles the .NET code and managed assemblies into `TorrentRationer.exe` (Windows) or `TorrentRationer` (Linux)
2. Extracts native graphics libraries as separate files in the same directory:
   - Windows: `libSkiaSharp.dll`, `libHarfBuzzSharp.dll`
   - Linux: `libSkiaSharp.so`, `libHarfBuzzSharp.so`

When you download the application, you'll see these files alongside the main executable. **Do not delete them** - they are required for the application to run.

## IF YOU STILL HAVE ISSUES

If you're still experiencing this error after downloading the latest version, it may be due to missing system dependencies:

## SOLUTION FOR INSTALLER USERS

**The installer now automatically checks for Visual C++ Redistributable!**

When you run the installer:
1. It will check if Visual C++ Runtime is installed
2. If missing, it will prompt you to download it
3. Click "Yes" to open the download page
4. Install the VC++ Redistributable
5. Run the Torrent Rationer installer again

## SOLUTION FOR PORTABLE VERSION USERS

### Step 1: Download and Install Visual C++ Redistributable (Windows only)

1. Download the installer from Microsoft:
   - **Direct link**: https://aka.ms/vs/17/release/vc_redist.x64.exe
   - Or search for "Visual C++ Redistributable latest"

2. Run the installer

3. Restart your computer (recommended)

4. Try running Torrent Rationer again

### Step 2: If That Doesn't Work - Install ALL Redistributables (Windows only)

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

## Why This Happened

The error occurred because:
- SkiaSharp is a cross-platform 2D graphics library
- It relies on native C++ libraries (libSkiaSharp.dll on Windows, libSkiaSharp.so on Linux)
- In earlier versions, these were bundled inside the single-file executable
- The .NET runtime couldn't extract and load them properly from the bundle
- Without SkiaSharp, Avalonia UI cannot render graphics

The fix ensures these native libraries are always available as separate files, allowing SkiaSharp to initialize correctly.

## For Basic Users

**TL;DR**: The latest version should work out of the box. If not:

**On Windows:**
1. Make sure all files from the download are in the same folder (don't delete any .dll files)
2. If still not working, install this: https://aka.ms/vs/17/release/vc_redist.x64.exe
3. Restart your computer
4. Run Torrent Rationer again - it will now work!

**On Linux:**
All necessary libraries are included. If you have issues, ensure you have basic graphics drivers installed.

This is a one-time installation. After this, the application will work without any issues on your computer.

## Verification

After installing the latest version:

**On Windows:**
1. Check if these files exist in the application folder:
   - `TorrentRationer.exe` (main executable)
   - `libSkiaSharp.dll` (graphics library)
   - `libHarfBuzzSharp.dll` (text rendering library)

2. If they exist, the fix is properly applied

**On Linux:**
1. Check if these files exist in the application folder:
   - `TorrentRationer` (main executable)
   - `libSkiaSharp.so` (graphics library)
   - `libHarfBuzzSharp.so` (text rendering library)

2. If they exist, the fix is properly applied

## Still Having Issues?

1. Check the application logs at:
   - Windows: `%APPDATA%\TorrentRationer\startup.log`
   - Linux: `~/.config/TorrentRationer/startup.log` or `~/.local/share/TorrentRationer/startup.log`
2. Look for specific error messages
3. Report the issue with the log contents

## Technical Details (For Developers)

The fix involved:

1. **Updated `runtimeconfig.template.json`:**
   - Added `System.Runtime.Loader.UseRidGraph: true` to enable proper RID-based native library resolution
   - Added `System.Runtime.Loader.NativeLibraryProbingDirectories` configuration

2. **Updated `TorrentRationer.csproj`:**
   - Added `RuntimeHostConfigurationOption` for RID graph usage
   - Implemented `ExcludeNativeAssets` MSBuild target that marks all native assets (`AssetType='native'`) to be excluded from the single-file bundle
   - This ensures native libraries are published as separate files alongside the main executable

These changes ensure that SkiaSharp's native dependencies are always available at runtime, preventing the initialization error.
