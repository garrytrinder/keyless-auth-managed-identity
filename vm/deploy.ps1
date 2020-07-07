az group create --name rg-keylessauth-dev --location uksouth --tags demo=true demoname=keyless-auth
az vm create --resource-group rg-keylessauth-dev --location uksouth --name vm-keylessauth-dev --image UbuntuLTS --generate-ssh-keys --admin-username azureuser
az vm open-port --port 80 --resource-group rg-keylessauth-dev --name vm-keylessauth-dev
az keyvault create --name vlt-keylessauth-dev --resource-group rg-keylessauth-dev --location uksouth --retention-days 90
$secret = -join ((65..90) + (97..122) | Get-Random -Count 5 | % {[char]$_})
Write-Host $secret
az keyvault secret set --vault-name vlt-keylessauth-dev --name supersecret --value $secret
$vaultUri = az keyvault show --name vlt-keylessauth-dev --resource-group rg-keylessauth-dev --query "properties.vaultUri" --output tsv
$publicIpAddress = az vm show --name vm-keylessauth-dev --resource-group rg-keylessauth-dev --show-details --query "publicIps" --output tsv
Write-Host $vaultUri
Write-Host $publicIpAddress