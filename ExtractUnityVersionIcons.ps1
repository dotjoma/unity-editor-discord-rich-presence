<#
.SYNOPSIS
    Extracts high-resolution (256x256) icons directly from installed Unity Editor executables.
.DESCRIPTION
    Scans common Unity Hub installation directories or user-specified paths, extracts the embedded
    high-res icons from Unity.exe via Windows Shell API (SHDefExtractIcon), and saves them as PNG.
#>

[CmdletBinding()]
param(
    [string[]]$SearchPaths = @(
        "D:\Unity\Hub\Editor",
        "C:\Program Files\Unity\Hub\Editor",
        "C:\Program Files\Unity"
    ),
    [string]$OutputDir = ""
)

if ([string]::IsNullOrWhiteSpace($OutputDir)) {
    $scriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Definition }
    if (-not $scriptDir) { $scriptDir = (Get-Location).Path }
    $OutputDir = Join-Path $scriptDir "icons"
}

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Add-Type -TypeDefinition @"
using System;
using System.Drawing;
using System.Runtime.InteropServices;

public class ShellIconExtractor
{
    [DllImport("shell32.dll", EntryPoint = "SHDefExtractIconW")]
    public static extern int SHDefExtractIcon(
        [MarshalAs(UnmanagedType.LPWStr)] string pszIconFile,
        int iIndex,
        uint uFlags,
        out IntPtr phiconLarge,
        out IntPtr phiconSmall,
        uint nIconSize
    );

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyIcon(IntPtr hIcon);

    public static Bitmap Extract(string filePath, int iconIndex = 0, int size = 256)
    {
        IntPtr hLarge, hSmall;
        int hr = SHDefExtractIcon(filePath, iconIndex, 0, out hLarge, out hSmall, (uint)((size << 16) | size));
        if (hr != 0 || hLarge == IntPtr.Zero) return null;

        try
        {
            using (Icon ico = Icon.FromHandle(hLarge))
            {
                return ico.ToBitmap();
            }
        }
        finally
        {
            if (hLarge != IntPtr.Zero) DestroyIcon(hLarge);
            if (hSmall != IntPtr.Zero) DestroyIcon(hSmall);
        }
    }
}
"@ -ReferencedAssemblies System.Drawing

Write-Host "Scanning for Unity Editor installations..." -ForegroundColor Cyan

$foundCount = 0
foreach ($path in $SearchPaths) {
    if (-not (Test-Path $path)) { continue }

    Get-ChildItem -Path $path -Recurse -Filter "Unity.exe" -ErrorAction SilentlyContinue | ForEach-Object {
        $exe = $_.FullName
        $version = $_.VersionInfo.ProductVersion
        if (-not $version) {
            $version = $_.Directory.Name
        }

        # Determine target filename
        $name = "unity_custom"
        if ($version -match "^6000|\b6\.") {
            $name = "unity_6"
        } elseif ($version -match "^2023") {
            $name = "unity_2023"
        } elseif ($version -match "^2022") {
            $name = "unity_2022"
        } elseif ($version -match "^2021") {
            $name = "unity_2021"
        } elseif ($version -match "^2020") {
            $name = "unity_2020"
        } elseif ($version -match "^2019") {
            $name = "unity_2019"
        } elseif ($version -match "^5\.") {
            $name = "unity_5"
        }

        $outFile = Join-Path $OutputDir "$name.png"
        Write-Host "Extracting 256x256 icon from: $exe ($version) -> $outFile" -ForegroundColor Yellow

        $bmp = [ShellIconExtractor]::Extract($exe, 0, 256)
        if ($bmp -ne $null) {
            # Scale to 512x512 (Discord Developer Portal minimum requirement)
            $dest = New-Object System.Drawing.Bitmap 512, 512
            $g = [System.Drawing.Graphics]::FromImage($dest)
            $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
            $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
            $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
            $g.Clear([System.Drawing.Color]::Transparent)
            $g.DrawImage($bmp, 0, 0, 512, 512)
            $g.Dispose()
            $bmp.Dispose()

            $dest.Save($outFile, [System.Drawing.Imaging.ImageFormat]::Png)
            $dest.Dispose()
            Write-Host "  Saved 512x512: $outFile" -ForegroundColor Green
            $foundCount++
        } else {
            Write-Warning "  Failed to extract icon from $exe"
        }
    }
}

Write-Host "Done. Extracted $foundCount icon(s) to $OutputDir." -ForegroundColor Cyan
