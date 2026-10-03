Add-Type -AssemblyName System.Drawing

$iconsDir = "D:\Unity\Projects\My2DGame1\tools\unity-editor-discord-rich-presence\icons"
if (-not (Test-Path $iconsDir)) {
    New-Item -ItemType Directory -Path $iconsDir -Force | Out-Null
}

function Create-BaseBadge512 {
    $bmp = New-Object System.Drawing.Bitmap 512, 512
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.Clear([System.Drawing.Color]::Transparent)

    # Outer subtle shadow
    $shadowBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(70, 0, 0, 0))
    $g.FillEllipse($shadowBrush, 24, 28, 464, 464)
    $shadowBrush.Dispose()

    # Dark background circle with smooth vertical gradient (Unity dark editor theme)
    $rect = New-Object System.Drawing.Rectangle 24, 24, 464, 464
    $gradBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        $rect,
        [System.Drawing.Color]::FromArgb(255, 48, 48, 48),
        [System.Drawing.Color]::FromArgb(255, 24, 24, 24),
        [System.Drawing.Drawing2D.LinearGradientMode]::Vertical
    )
    $g.FillEllipse($gradBrush, $rect)
    $gradBrush.Dispose()

    # Outer border ring
    $penOuter = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(255, 75, 75, 75), 12)
    $g.DrawEllipse($penOuter, 28, 28, 456, 456)
    $penOuter.Dispose()

    # Inner subtle highlight ring
    $penInner = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(35, 255, 255, 255), 4)
    $g.DrawEllipse($penInner, 34, 34, 444, 444)
    $penInner.Dispose()

    return @{ Bitmap = $bmp; Graphics = $g }
}

# 1. PLAY ICON (512x512)
$base = Create-BaseBadge512
$g = $base.Graphics
$playBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 44, 165, 224)) # Unity Play Blue/Cyan
$pts = @(
    New-Object System.Drawing.PointF 192, 144
    New-Object System.Drawing.PointF 192, 368
    New-Object System.Drawing.PointF 372, 256
)
$path = New-Object System.Drawing.Drawing2D.GraphicsPath
$path.AddPolygon($pts)
$g.FillPath($playBrush, $path)

# Subtle inner gloss on play arrow
$glossBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
    (New-Object System.Drawing.Rectangle 192, 144, 180, 224),
    [System.Drawing.Color]::FromArgb(80, 255, 255, 255),
    [System.Drawing.Color]::FromArgb(0, 255, 255, 255),
    [System.Drawing.Drawing2D.LinearGradientMode]::Vertical
)
$g.FillPath($glossBrush, $path)
$glossBrush.Dispose()

$base.Bitmap.Save((Join-Path $iconsDir "play.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$base.Bitmap.Dispose(); $g.Dispose()

# 2. PAUSE ICON (512x512)
$base = Create-BaseBadge512
$g = $base.Graphics
$pauseBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 245, 166, 35)) # Unity Amber/Gold
# Bar 1
$g.FillEllipse($pauseBrush, 168, 152, 56, 56)
$g.FillEllipse($pauseBrush, 168, 304, 56, 56)
$g.FillRectangle($pauseBrush, 168, 180, 56, 152)
# Bar 2
$g.FillEllipse($pauseBrush, 288, 152, 56, 56)
$g.FillEllipse($pauseBrush, 288, 304, 56, 56)
$g.FillRectangle($pauseBrush, 288, 180, 56, 152)

$base.Bitmap.Save((Join-Path $iconsDir "pause.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$base.Bitmap.Dispose(); $g.Dispose()

# 3. COMPILE ICON (512x512)
$base = Create-BaseBadge512
$g = $base.Graphics
$compileColor = [System.Drawing.Color]::FromArgb(255, 56, 225, 176) # Unity Teal / Cyan
$compilePen = New-Object System.Drawing.Pen ($compileColor, 36)
$compilePen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
$compilePen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
$g.DrawArc($compilePen, 128, 128, 256, 256, 205, 145)
$g.DrawArc($compilePen, 128, 128, 256, 256, 25, 145)

$compileBrush = New-Object System.Drawing.SolidBrush ($compileColor)
# Arrow 1 (top)
$arrow1 = @(
    New-Object System.Drawing.PointF 276, 96
    New-Object System.Drawing.PointF 216, 160
    New-Object System.Drawing.PointF 316, 160
)
$g.FillPolygon($compileBrush, $arrow1)
# Arrow 2 (bottom)
$arrow2 = @(
    New-Object System.Drawing.PointF 236, 416
    New-Object System.Drawing.PointF 296, 352
    New-Object System.Drawing.PointF 196, 352
)
$g.FillPolygon($compileBrush, $arrow2)

$base.Bitmap.Save((Join-Path $iconsDir "compile.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$base.Bitmap.Dispose(); $g.Dispose()

# 4. EDIT ICON (512x512)
$base = Create-BaseBadge512
$g = $base.Graphics

$topFace = @(
    New-Object System.Drawing.PointF 256, 146
    New-Object System.Drawing.PointF 346, 198
    New-Object System.Drawing.PointF 256, 250
    New-Object System.Drawing.PointF 166, 198
)
$leftFace = @(
    New-Object System.Drawing.PointF 166, 198
    New-Object System.Drawing.PointF 256, 250
    New-Object System.Drawing.PointF 256, 354
    New-Object System.Drawing.PointF 166, 302
)
$rightFace = @(
    New-Object System.Drawing.PointF 256, 250
    New-Object System.Drawing.PointF 346, 198
    New-Object System.Drawing.PointF 346, 302
    New-Object System.Drawing.PointF 256, 354
)

$topBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 235, 235, 235))
$leftBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 140, 140, 140))
$rightBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 185, 185, 185))

$g.FillPolygon($topBrush, $topFace)
$g.FillPolygon($leftBrush, $leftFace)
$g.FillPolygon($rightBrush, $rightFace)

$cubeEdgePen = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(255, 34, 34, 34), 6)
$g.DrawPolygon($cubeEdgePen, $topFace)
$g.DrawPolygon($cubeEdgePen, $leftFace)
$g.DrawPolygon($cubeEdgePen, $rightFace)

# Scene Gizmo RGB Axis indicators (Y = Green, X = Red, Z = Blue)
$brushGreen = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 76, 217, 100))
$brushRed = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 255, 59, 48))
$brushBlue = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 0, 122, 255))

$g.FillEllipse($brushGreen, 244, 186, 24, 24)
$g.FillEllipse($brushRed, 196, 262, 24, 24)
$g.FillEllipse($brushBlue, 292, 262, 24, 24)

$base.Bitmap.Save((Join-Path $iconsDir "edit.png"), [System.Drawing.Imaging.ImageFormat]::Png)
$base.Bitmap.Dispose(); $g.Dispose()

Write-Output "Successfully generated 512x512 play.png, pause.png, compile.png, edit.png in $iconsDir"
