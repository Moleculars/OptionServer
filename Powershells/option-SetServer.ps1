
param(
    [Parameter(Mandatory=$true)][string]$uri
)


    [System.Environment]::SetEnvironmentVariable("option_server_url", $uri)
    Write-Host "option server setted on : " $uri
    Write-Host ""