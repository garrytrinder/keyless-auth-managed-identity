# Call Microsoft Graph from Azure Function using Azure Managed Identity

This scenario demonstrates how you can use Azure Managed Identity to return a list of Office 365 Groups from the Microsoft Graph without needing to pass any credentials.

It also guides you through how you can create and configure a service principal for local development.

## Prerequisites

This scenario requires

- Active Azure Subscription
- Azure PowerShell Module
- Windows PowerShell
- Azure CLI v2.8.0
- Office 365 CLI v2.11.0

## Azure Function App Deployment

1. Open PowerShell prompt
1. Execute `.\deploy.ps1` to deploy Azure resources

## Azure Function App Managed Identity

1. Managed Identity is enabled as part of the deployment

## Grant Microsoft Graph App Role to Managed Identity Service Principal

1. Connect to Office 365 tenant using Office 365 CLI using `m365 login` command
1. Execute `m365 aad approleassignment add --displayName "func-keylessauth-dev" --resource "Microsoft Graph" --scope "Group.Read.All"` to assign the Microsoft Graph `Group.Read.All` application role to the Managed Identity service principal

## Create Service Principal & Self Signed Certificate for Local Development (Windows)

1. Connect to Azure using `Connect-AzAccount` cmdlet
1. Execute `./localcert.ps1`
1. Take a copy of the output values

> Do not use PowerShell Core for this script, it will complete but the identity will not work

## Update Development Settings

1. Open `local.settings.json`
1. Update `LocalDevAppId` value with `App Id`
1. Update `LocalTenantId` value with `Tenant Id`
1. Update `LocalThumbprint` value with `Thumbprint` 

