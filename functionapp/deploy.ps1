az group create --name rg-keylessauth-dev --location uksouth --tags demo=true demoname=keyless-auth
az storage account create --name stkeylessauthdev --resource-group rg-keylessauth-dev
az extension add --name application-insights
az monitor app-insights component create --app appi-keylessauth-dev --location uksouth --resource-group rg-keylessauth-dev --application-type web
az functionapp create --name func-keylessauth-dev --resource-group rg-keylessauth-dev --storage-account stkeylessauthdev --functions-version 3 --runtime dotnet --os-type Windows --app-insights appi-keylessauth-dev --consumption-plan-location uksouth
az functionapp identity assign --name func-keylessauth-dev --resource-group rg-keylessauth-dev
dotnet clean .\KeylessAuthDotNetCore\KeylessAuthDotNetCore.sln
dotnet restore .\KeylessAuthDotNetCore\KeylessAuthDotNetCore.sln
dotnet publish .\KeylessAuthDotNetCore\KeylessAuthDotNetCore.sln -c Release
$currentPath = (Get-Location).Path
if (Test-path .\publish.zip) { Remove-item .\publish.zip }
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory("$currentPath\KeylessAuthDotNetCore\bin\Release\netcoreapp3.1\publish\","$currentPath/publish.zip")
az functionapp deployment source config-zip --name func-keylessauth-dev --resource-group rg-keylessauth-dev --src .\publish.zip
m365 aad approleassignment add --displayName "func-keylessauth-dev" --resource "Microsoft Graph" --scope "Group.Read.All"