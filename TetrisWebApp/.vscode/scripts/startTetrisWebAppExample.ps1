$host.ui.RawUI.WindowTitle = "start: TWA Example App"

wt -w 0 new-tab --profile "PowerShell" -c pwsh (Join-Path $PSScriptroot buildTetrisWebAppCore.ps1); 

Start-Sleep -Seconds 1

wt -w 0 new-tab --profile "PowerShell" -c pwsh (Join-Path $PSScriptroot buildTetrisWebAppExample.ps1);