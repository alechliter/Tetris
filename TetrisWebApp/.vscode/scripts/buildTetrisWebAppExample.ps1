param([String]$basePath=$PSScriptroot)

$host.ui.RawUI.WindowTitle = "build: TWA Example App"

Write-Host "Going to $basePath";

cd $basePath;

Write-Host "Starting TWA Example App...";

npm run start:example