# SkiaSharp Initialization Error - SOLUTION

## Error Message
```
The type initializer for 'SkiaSharp.SKImageInfo' threw an exception
```

## What This Means
This error occurs when SkiaSharp (the graphics rendering library used by Avalonia) cannot initialize. This is typically caused by missing Visual C++ Runtime libraries on Windows.

## SOLUTION (Windows)

### Option 1: Install Visual C++ Redistributable (RECOMMENDED)

1. Download the Visual C++ Redistributable from Microsoft:
   - Direct link: https://aka.ms/vs/17/release/vc_redist.x64.exe
   - Or search for "Visual C++ Redistributable latest"

2. Run the installer

3. Restart your computer (recommended)

4. Try running Torrent Rationer again

### Option 2: Install ALL Visual C++ Redistributables

If Option 1 doesn't work, you may need multiple versions:

1. Download from: https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads

2. Install both:
   - vc_redist.x64.exe (64-bit)
   - vc_redist.x86.exe (32-bit, for compatibility)

### Option 3: Update Graphics Drivers

Outdated graphics drivers can also cause this issue:

1. Visit your GPU manufacturer's website:
   - NVIDIA: https://www.nvidia.com/download/index.aspx
   - AMD: https://www.amd.com/en/support
   - Intel: https://www.intel.com/content/www/us/en/download-center/home.html

2. Download and install the latest driver for your GPU

3. Restart your computer

## Technical Details

The error occurs because:
- SkiaSharp is a cross-platform 2D graphics library
- It relies on native C++ libraries (libSkiaSharp.dll on Windows)
- These native libraries depend on Visual C++ Runtime
- If the runtime is missing or corrupted, SkiaSharp cannot initialize
- Without SkiaSharp, Avalonia UI cannot render graphics

## Verification

After installing the Visual C++ Redistributable:

1. Check if these files exist in `C:\Windows\System32\`:
   - `vcruntime140.dll`
   - `msvcp140.dll`

2. If they exist, the redistributable is installed correctly

3. Run Torrent Rationer again - it should now work!

## Alternative: Non-Single-File Build

If you continue to have issues, try the non-single-file portable build which extracts native libraries properly.

## Still Having Issues?

1. Check the application logs at: `%APPDATA%\TorrentRationer\startup.log`
2. Look for specific error messages
3. Report the issue with the log contents
