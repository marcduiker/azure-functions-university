# Managed Identity (PowerShell)

Watch the recording of this lesson [on YouTube]().

## Goal 🎯

The goal of this lesson is to understand how you can create and use a system-assigned managed identity to call an Azure Function in order to obtain a Microsoft Graph access token with the right permission scope. We prefer Managed Identities over an App registration with an app secret because it's more secure. Secrets can potentially be leaked or expire, and therefore they are an additional workload to handle. When we use a managed identity, we don't need an app registration in Azure Active Directory, and we don't need access to any secret. Learn more here: [What are managed identities for Azure resources?](https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/overview) Microsoft Graph is THE API for all things Microsoft 365.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Create an Azure Functions App](#1-create-an-azure-functions-app)
|2|[Create Azure resources](#2-create-azure-resources)
|3|[Create Managed Identity and assign permissions](#3-create-managed-identity-and-assign-permissions)
|4|[Obtain an access token](#4-obtain-an-access-token)
|5|[Homework](#5-homework)
|6|[More info](#6-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../src/{language}/{topic}) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -

See [{PowerShell} prerequisites](../prerequisites/prerequisites-{powershell}.md) for more details.

## 1. Create an Azure Functions App

before we will deploy our app to Azure, we will develop it locally in Visual Studio Code. The goal of this exercise is to understand how to  make an HTTP request to Microsoft Graph API, as we want to return all groups of the tenant.

### Steps

1. Select _New Project_
2. Select a folder for your project
3. Select a language – I will use PowerShell
4. Select _HTTP trigger_ as a template
5. Type in a better name like `GetGraphToken`
6. Select Authorization level _Function_. Select how you want to open your project – I prefer _Add to workspace_
7. Open `run.ps1`
8. Replace the default code by this:

```powershell
using namespace System.Net

# Input bindings are passed in via param block
param($Request, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

# Interact with query parameters or the body of the request
.$Scope = $Request.Query.Scope
if (-not $Scope) {
    $Scope = $Request.Body.Scope
}
#If parameter "Scope" has not been provided, we assume that graph.microsoft.com is the target resource
If (!$Scope) {
    $Scope = "https://graph.microsoft.com/"
}

$tokenAuthUri = $env:IDENTITY_ENDPOINT + "?resource=$Scope&api-version=2019-08-01"
$response = Invoke-RestMethod -Method Get -Headers @{"X-IDENTITY-HEADER"="$env:IDENTITY_HEADER"} -Uri $tokenAuthUri -UseBasicParsing

$accessToken = $response.access_token

#Invoke REST call to Graph API
$uri = 'https://graph.microsoft.com/v1.0/groups'
$authHeader = @{    
'Content-Type'='application/json'
'Authorization'='Bearer ' +  $accessToken
}

$result = (Invoke-RestMethod -Uri $uri -Headers $authHeader -Method Get -ResponseHeadersVariable RES).value

If ($result) {
    $body = $result
    $StatusCode = '200'
}
Else {
    $body = $RES
    $StatusCode = '400'}

# Associate values to output bindings by calling 'Push-OutputBinding'
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = $StatusCode
    Body = $body
})
```

> 📝 **Tip** - Watch out – the Graph API will this way return up to 100 groups – Please adjust with query parameters as needed like `https://graph.microsoft.com/v1.0/groups?$top=42` or use pagination, which is described here: [Paging Microsoft Graph data in your app](https://docs.microsoft.com/graph/paging)

Take a moment to understand what the code does:

* log that a request was received
* define the scope (if not stated, it’s Microsoft Graph)
* obtain the token from the environment variables IDENTITY_ENDPOINT and IDENTITY_HEADER (more about that in the next step!)
* pass that token in the header of the REST call towards the group endpoint of Microsoft Graph
* get the status code (hopefully 200) 🤞

## 2. Create Azure resources

Now we want to create all resources that we need in Azure:

Use the Azure CLI or VSCode to create:

* A resource group
* A storage account
* A PowerShell Function App

We'll need the names of the resource group and Function App in the next step, so you might want to use variables for those (e.g. `$resourcegroup` and `$functionApp`) in case you use the Azure CLI.

If you are unfamiliar with this process, please find more info in the [Deployment lesson](https://github.com/marcduiker/azure-functions-university/blob/main/lessons/deployment/deployment-lesson.md)

## 3. Create managed identity and assign permissions

We want things to be super secure – this is why we want to enable a system assigned managed identity for our new function:

### Steps

Use the PowerShell terminal and type

```powershell
az functionapp identity assign -n $functionApp -g $resourceGroup
```

Our managed identity needs the correct permission scope to access Graph API for Group.Read.All, and to eventually be able to make the required REST call, we will need

* the Graph API service Provider
* permission scope, expressed as App role

Let’s do this - use the PowerShell terminal and type:

```powershell
#Get Graph Api service provider (that's later needed for --api) 
az ad sp list --query "[?appDisplayName=='Microsoft Graph'].{Name:appDisplayName, Id:appId}" --output table --all
#Save that service provider in a variable
$graphId = az ad sp list --query "[?appDisplayName=='Microsoft Graph'].appId | [0]" --all 
# Get permission scope for "Group.Read.All"
$appRoleId = az ad sp show --id $graphId --query "appRoles[?value=='Group.Read.All'].id | [0]" 
```

Time to make the REST call to assign the permissions as shown above to the managed identity:

Use the PowerShell terminal and type

```powershell
#Set values
$webAppName="LuiseDemo-functionapp$rand"
$principalId=$(az resource list -n $webAppName --query [*].identity.principalId --out tsv)
$graphResourceId=$(az ad sp list --display-name "Microsoft Graph" --query [0].objectId --out tsv)
$appRoleId=$(az ad sp list --display-name "Microsoft Graph" --query "[0].appRoles[?value=='Group.Read.All' && contains(allowedMemberTypes, 'Application')].id" --out tsv)
$body="{'principalId':'$principalId','resourceId':'$graphResourceId','appRoleId':'$appRoleId'}"

#The actual REST call to assign the app role to the service principal
az rest --method post --uri https://graph.microsoft.com/v1.0/servicePrincipals/$principalId/appRoleAssignments --body $body --headers Content-Type=application/json
```

> 📝 **Tip** - You can verify if everything worked as intended in Azure portal: Azure Active Directory --> Enterprise applications --> Managed Identity

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < how would you several permissions in one go? >

To see things work, we will need to deploy our function into our Functions App.

1. head over to Visual Studio Code again
2. select _deploy to Functions App_
3. Select the Functions App you already created
4. Confirm the Pop up window by selecting _Deploy_

> 🔎 **Observation** - < this might take a minute >

<!-- check with Marc if I need to explain how to test in Azure portal -->

## 4. Obtain an access token

In this step we want to learn how we could obtain an access token which we needed for a successful HTTP request against Microsoft Graph API:

### Steps

1. Although we would usually load an authentication library such as Azure.Identity and then  to obtain a token, there is an easier, but not documented way get the token in an Azure  Functions: Following and extrapolating [Obtain tokens for Azure resources](https://docs.microsoft.com/azure/app-service/overview-managed-identity?tabs=powershell#obtain-tokens-for-azure-resources) to Microsoft Graph surprisingly works:

Review the code in `Run.ps1`:

```
$resourceURI = "https://<AAD-resource-URI-for-resource-to-obtain-token>"
$tokenAuthURI = $env:IDENTITY_ENDPOINT + "?resource=$resourceURI&api-version=2019-08-01"
$tokenResponse = Invoke-RestMethod -Method Get -Headers @{"X-IDENTITY-HEADER"="$env:IDENTITY_HEADER"} -Uri $tokenAuthURI
$accessToken = $tokenResponse.access_token
```

2. have a look at your _IDENTITY_ENDPOINT_ and _IDENTITY_HEADER_ environment variables at `https://<your-functionappname-here>.scm.azurewebsites.net/ENV.cshtml#envVariables`

## 5. Homework

<!-- check with Marc what would be appropriate homework -->

## 6. More info

[What are managed identities for Azure resources?](https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/overview)

---
[🔼 Lessons Index](../../README.md)
