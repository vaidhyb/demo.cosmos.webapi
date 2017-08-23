Add-Type -AssemblyName System.Web

$currentServerName = $env:computername
Write-Host $currentServerName

$csvPath = $PSScriptRoot + '\ServerConfig.csv';
$csvObjects = Import-CSV $csvPath;
$serverObject = $csvObjects | Where { $_.ServerName -eq $currentServerName}
If ($serverObject) {
    $serverName =  $serverObject.ServerName;
    $hospitalID =  $serverObject.HospitalId;
    $connectionString =  $serverObject.ConnectionString;
}
Else {
    Write-Host "variable is null" 
}

$webConfigPath = 'C:\inetpub\cosmosdb.webapi\Web.config'
$webConfig = (Get-Content $webConfigPath) -as [Xml]


$appSettingHospitalID = $webConfig.configuration.appSettings.add | where {$_.Key -eq 'HospitalID'}
if($appSettingHospitalID) {
    $appSettingHospitalID.value = $hospitalID;
}
else {
    $newAppSetting = $webConfig.CreateElement("add")
    $webConfig.configuration.appSettings.AppendChild($newAppSetting)
    $newAppSetting.SetAttribute("key","HospitalID");
    $newAppSetting.SetAttribute("value",$hospitalID);
}

$appSettingServerName = $webConfig.configuration.appSettings.add | where {$_.Key -eq 'ServerName'}
if($appSettingServerName) {
    $appSettingServerName.value = $serverName;
}
else {
    $newAppSetting = $webConfig.CreateElement("add")
    $webConfig.configuration.appSettings.AppendChild($newAppSetting)
    $newAppSetting.SetAttribute("key","ServerName");
    $newAppSetting.SetAttribute("value",$serverName);
}
$webConfig.Save($webConfigPath)