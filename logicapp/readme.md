# Call Microsoft Graph from Azure Logic App using Azure Managed Identity

This scenario demonstrates how you can use Azure Managed Identity to return a list of Office 365 Groups from the Microsoft Graph without needing to pass any credentials.

The deployment script will automatically enable managed identity and create a system assigned identity.

## Prerequisites

This scenario requires

- Active Azure Subscription
- PowerShell
- Azure CLI v2.8.0
- Office 365 CLI v2.11.0

## Deploy Azure Resources

1. Open PowerShell prompt
1. Execute `.\deploy.ps1` to deploy Azure resources

## Grant Microsoft Graph App Role to Managed Identity Service Principal

1. Connect to Office 365 tenant using Office 365 CLI using `m365 login` command
2. Execute `m365 aad approleassignment add --displayName "la-keylessauth-dev" --resource "Microsoft Graph" --scope "Group.Read.All"` to assign the Microsoft Graph `Group.Read.All` application role to the Managed Identity service principal

## Update and execute the Logic App to call Microsoft Graph

1. Go to `la-keylessauth-dev` blade
1. Click `Recurence` on the `Logic Apps Designer` blade
1. Click `New step` and select `HTTP` action
1. Choose `GET` in method dropdown
1. Set the URI to `https://graph.microsoft.com/v1.0/groups`
1. Add Headers key `accepts` with the value `application/json`
1. Click `Add new parameter` dropdown and check `Authentication` box, click off to reveal the `Authentication` section
1. Select `Managed Identity` in `Authentication type` dropdown
1. Select `System Assigned Managed Identity`
1. Add `https://graph.microsoft.com` to `Audience` field
1. Click `Save` and `Run`