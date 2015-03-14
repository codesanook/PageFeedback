#Modified and simplified version of https://www.windowsazure.com/en-us/develop/net/common-tasks/continuous-delivery/
#From: #https://gist.github.com/4539567
Param( 
$subscription = "", #this the name from your .publishsettings file
$service = "", #this is the name of the cloud service you created
$storageAccount = "", #this is the name of the storage service you created
$slot = "", #staging or production
$package = "",
$configuration = "",
$publishSettingsFile = "",
$deploymentLabel = ""
)

Write-Output "Subscription: $subscription"
Write-Output "Service: $service"
Write-Output "StorageAccount: $storageAccount"
Write-Output "Slot: $slot"
Write-Output "Package: $package"
Write-Output "ConfigurationFile: $configuration"
Write-Output "PublishSettingFile: $publishSettingsFile" 
Write-Output "DeploymentLabel: $deploymentLabel" 

Write-Output "Running Azure Imports"

<#
for older version of Azure PowerShell
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1"#>
#new Azure SDK 2.4 change Azure.psd1 to new folder to Load Commandlet Azure to PowerShell
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Azure.psd1"
Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFile

Set-AzureSubscription -SubscriptionName $subscription -CurrentStorageAccountName $storageAccount 
Select-AzureSubscription -SubscriptionName $subscription

# to create azure storage New-AzureStorageAccount -Location "" -StorageAccountName ""

Set-AzureService -ServiceName $service -Label $deploymentLabel
 
function Publish(){
 $deployment = Get-AzureDeployment -ServiceName $service -Slot $slot -ErrorVariable a -ErrorAction silentlycontinue 
 
 if ($a[0] -ne $null) {
    Write-Output "$(Get-Date -f $timeStampFormat) - No deployment is detected. Creating a new deployment. "
 }
 
 if ($deployment.Name -ne $null) {
    #Update deployment inplace (usually faster, cheaper, won't destroy VIP)
    Write-Output "$(Get-Date -f $timeStampFormat) - Deployment exists in $servicename.  Upgrading deployment."
    UpgradeDeployment
 } else {
    CreateNewDeployment
 }
}
 
function CreateNewDeployment()
{
    write-progress -id 3 -activity "Creating New Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: In progress"
 
    $opstat = New-AzureDeployment -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service
 
    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid
 
    write-progress -id 3 -activity "Creating New Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: Complete, Deployment ID: $completeDeploymentID"
}
 
function UpgradeDeployment()
{
    write-progress -id 3 -activity "Upgrading Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: In progress"
 
    # perform Update-Deployment
    $setdeployment = Set-AzureDeployment -Upgrade -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service -Force
 
    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid
 
    write-progress -id 3 -activity "Upgrading Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: Complete, Deployment ID: $completeDeploymentID"
}
 
Write-Output "Create Azure Deployment"
Publish
