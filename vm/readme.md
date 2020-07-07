# Access Azure Key Vault from Linux Azure VM using Azure Managed Identity

This scenario demonstrates how you can use Azure Managed Identity to obtain a secret from Azure Key Vault from a Linux based Azure VM without the need for handling credentials.

The deployment script creates a secret in the Key Vault called `supersecret` with a random value created a deployment time.

## Prerequisites

This scenario requires

- Active Azure Subscription
- PowerShell
- Ubuntu (WSL)
- Azure CLI v2.8.0

## Deploy Azure resources

1. Open PowerShell prompt
1. Execute `.\deploy.ps1` to deploy Azure resources

## Enable Managed Identity on Azure VM 

1. Navigate to [Azure Portal](https://portal.azure.com)
1. Go to `rg-keylessauth-dev` blade
1. Go to `vm-keylessauth-dev` blade
1. Go to `Identity`
1. On `System assigned` tab switch `Status` to `On` and click `Save`

## Configure Azure Key Vault

1. Go to `vlt-keylessauth-dev` blade
1. Go to `Access Policies`
1. Click `Add Access Policy`
1. Select `Get` permission in `Secret permissions` dropdown
1. Click `None selected` in `Select principal` section
1. Search for `vm-keylessauth-dev`, select in list and click `Select`
1. Click `Add`

## Remotely connect to Azure VM using SSH

1. Execute `ssh azureuser@{publicIpAddress}` in shell to connect to running VM, replace `{publicIpAdress}` with value provided by deployment script in the console

## Install jq for handling JSON

1. Execute `sudo apt-get update` to update `apt` index
1. Execute `sudo apt-get install jq` to install `jq` library for parsing JSON

## Get token and call Azure Key Vault API to obtain secret

1. Execute ```token=`curl 'http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://vault.azure.net' -H Metadata:true -s | jq -r '.access_token'``` in SSH console to obtain access token for Azure Key Vault and store in variable
1. Execute ```value=`curl https://vlt-keylessauth-dev.vault.azure.net/secrets/supersecret?api-version=2016-10-01 -H "Authorization: Bearer ${token}" -s | jq -r ".value"` ``` to obtain secret value and store in variable
1. Execute `echo $value` to show secret value
