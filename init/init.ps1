$ipAddresses = (Get-NetIPConfiguration | Where-Object {$_.IPv4DefaultGateway -ne $null -and $_.NetAdapter.status -ne "Disconnected"}).IPv4Address.IPAddress

if($ipAddresses.Count -gt 1) {
 $ipAddress = $ipAddresses[0]
}
elseif ($ipAddresses.Count -eq 1){
    $ipAddress = $ipAddresses
}
else {
    throw [System.Exception] "IpAddress not found."
}

$matchString = '(?:[0-9]{1,3}\.){3}[0-9]{1,3}'
$replaceString = $ipAddress
$files = Get-ChildItem -Path '..\' -Include appsettings.prod.json,docker-compose.yml -Recurse

foreach ($file in $files) {
    (Get-Content $file.FullName)  -replace $matchString, $ipAddress |  Set-Content -Path $file.FullName
}

docker-compose down -v --rmi all
docker-compose up -d
pause