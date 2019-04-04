
param(
    [Parameter(Mandatory=$true)][string]$login,
    [Parameter(Mandatory=$true)][string]$password,
    [Parameter(Mandatory=$true)][string]$Pseudo,
    [Parameter(Mandatory=$true)][string]$Mail
)


$RootUrl = [System.Environment]::GetEnvironmentVariable("option_server_url")
if ($RootUrl -eq $null)
{
    throw New-Object Exception "option server not setted. please concidere use option-server.ps1 -uri 'http://optionserver:80'"
}



$url = $RootUrl + "/api/user/add"
$msg = @{login = $login; Password = $password; Pseudo = $Pseudo ; Mail = $Mail}

Write-Host (ConvertTo-Json $msg) 
Write-Host "calling configuration server : " $url

try
{

    $result = Invoke-RestMethod -Uri $url -Method Post -ContentType "application/json;charset=UTF-8"  -Body (ConvertTo-Json $msg) 

    if ($result.Valid)
    {
        Write-Host "  Id : " $result.id
        Write-Host "  Name : " $result.Username
        Write-Host "  Pseudo : " $result.Pseudo
        Write-Host ""
    }
    else
    {
        Write-Error $result.Result
    }

}
catch [Exception]
{
    Write-Error $Error[0].Exception.Message
}

