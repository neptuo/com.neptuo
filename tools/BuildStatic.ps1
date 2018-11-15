param([string]$buildFolder)

$outputPath = "..\output";
$sitePath = "$($buildFolder)\src\Neptuo.WebSite"
$port = 7287
$delay = 3000

# Start IIS.
Write-Host "Running IIS Express from '$($sitePath)' at '$($port)'."
$iis = Start-Process "C:\Program Files (x86)\IIS Express\iisexpress.exe" -NoNewWindow -ArgumentList "/path:$($sitePath) /port:$($port)"

Write-Host "Waiting $($delay)."
Start-Sleep -Milliseconds $delay

# Crawl site.
Write-Host "Running StaticSiteCrawler."
.\Tools\StaticSiteCrawler.exe http://localhost:$($port)/ $outputPath / /404.html

# Fail build when any URL download failed.
if (!($LastExitCode -eq 0))
{
    Write-Error -Message "Crawler result: $LastExitCode" -ErrorAction Stop
    # throw "Crawler result: $LastExitCode";
}

# Copy assets.
If (Test-Path "$($sitePath)\Content\Images") 
{
    New-Item "$($outputPath)\Content\Images" -ItemType Directory
    Copy-Item "$($sitePath)\Content\Images\*" -Destination "$($outputPath)\Content\Images" -Force -Recurse
}

If (!(Test-Path "$($outputPath)\Content"))
{
	New-Item "$($outputPath)\Content" -ItemType Directory
	Copy-Item "$($sitePath)\Content\*.css" -Destination "$($outputPath)\Content" -Force -Recurse
}

If (!(Test-Path "$($outputPath)\fonts"))
{
	New-Item "$($outputPath)\fonts" -ItemType Directory
	Copy-Item "$($sitePath)\fonts\*" -Destination "$($outputPath)\fonts" -Force -Recurse
}

If (!(Test-Path "$($outputPath)\Scripts"))
{
	New-Item "$($outputPath)\Scripts" -ItemType Directory
	Copy-Item "$($sitePath)\Scripts\*.js" -Destination "$($outputPath)\Scripts" -Force -Recurse
}

# Debug print output.
Write-Host "Content of output:"
$items = Get-ChildItem -Path $outputPath
ForEach ($item in $items) 
{ 
    $size = (Get-Item "$($outputPath)\$($item)").Length
    Write-Host "'$($item)' - $($size)B"
}

# Stop IIS.
Write-Host "Stopping IIS Express."
Stop-Process -Name iisexpress