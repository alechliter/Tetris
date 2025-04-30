param([String]$basePath=$PSScriptroot)

$host.ui.RawUI.WindowTitle = "build: TWA Core Library"

Write-Host "Going to $basePath";

cd $basePath;

Write-Host "Building TWA Core Library...";

npm run build:core