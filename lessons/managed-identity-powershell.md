# Managed Identity (PowerShell)

Watch the recording of this lesson [on YouTube]().

## Goal 🎯

The goal of this lesson is to understand how you can create and use a system assigned managed to call an Azure function in order to obtain a Microsoft Graph access token with the right permission scope. Microsoft Graph is THE API for all things Microsoft 365. 

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Create an Azure Functions](#1-create-an-azure-functions)
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

See [{language} prerequisites](../prerequisites/prerequisites-{language}.md) for more details.

## 1. Create an Azure Functions

before we will deploy our app to Azure, we will develop it locally in Visual Studio Code. This comes with some great advantages such as 

* we don’t need to adjust to ever changing UI in Azure portal
* we can benefit from source control
* we can collaborate with others

### Steps

1. Install the Core Tools package with `npm install -g azure-functions-core-tools@3 --unsafe-perm true`
2. Install the [Azure Functions extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
3. Select **New Project**
4. Select a folder for your project
5. Select a language – I will use PowerShell
6. Select **HTTP trigger** as a template
7. Type in a better name like `GetGraphToken`
8. Select Authorization level **Function**
9. Select how you want to open your project – I prefer **Add to workspace**
10. Open `run.ps1`
11. Replace the default code by this: 

```
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
> 📝 **Tip** - Watch out – the Graph API will this way return up to 100 groups – Please adjust with query parameters as needed like `https://graph.microsoft.com/v1.0/groups?$top=42` or use pagination, which is described here: [Paging Microsoft Graph data in your app](https://docs.microsoft.com/en-us/graph/paging)

Take a moment to understand what the code does: 

* log that a request was received
* define the scope (if not stated, it’s Microsoft Graph)
* obtain the token from the environment variables IDENTITY_ENDPOINT and IDENTITY_HEADER (more about that in the next step!)
* pass that token in the header of the REST call towards the group endpoint of Microsoft Graph
* get the status code (hopefully 200) 🤞

## 2. Create Azure resources

Now we want to create all resources that we need in Azure: 

For testing purposes, I pseudo-randomized a number to not always need to come up with new names:

```
#Get a random number between 100 and 300 to more easily be able to distinguish between several trials
$rand = Get-Random -Minimum 100 -Maximum 300
```
We will now set some variables, this reduces risk of typos and makes our code better readable – also we can reuse it better – this is a courtesy to future-self
```
#Set values
$resourceGroup = "DemoPlay$rand"
$location = "westeurope"
$storage = "luisedemostorage$rand"
$functionapp = "LuiseDemo-functionapp$rand"

```
Let’s create a resource-group that will later hold our Azure Functions App
```
#create group
az group create -n $resourceGroup -l $location
```

As our Functions App will need a storage account, we will create this as well:

```
#create storage account
az storage account create `
  -n $storage `
  -l $location `
  -g $resourceGroup `
  --sku Standard_LRS
```
Now create the Azure Functions App which later holds our function (remember we created that earlier locally, but will later deploy it to Azure)

```
#create function
az functionapp create `
  -n $functionapp `
  --storage-account $storage `
  --consumption-plan-location $location `
  --runtime powershell `
  -g $resourceGroup `
  --functions-version 3
```

> 🔎 **Observation** - < It will take a few moments for everything to be set, once this step is completed, you will be prompted with a message, that you also can benefit from Application Insights. >

## 3. Create Managed Identity and assign permissions

We want things to be super secure – this is why we want to enable a system assigned Managed Identity for our new function:

### Steps

```
az functionapp identity assign -n $functionapp -g $resourceGroup
```
Our Managed Identity shall have the right permission scope to access Graph API for Group.Read.All, and to eventually be able to make the required REST call, we will need

- the Graph API service Provider
- permission scope, expressed as App role
Let’s do this:

```
#Get Graph Api service provider (that's later needed for --api) 
az ad sp list --query "[?appDisplayName=='Microsoft Graph'].{Name:appDisplayName, Id:appId}" --output table --all
#Save that service provider 
$graphId = az ad sp list --query "[?appDisplayName=='Microsoft Graph'].appId | [0]" --all 
# Get permission scope for "Group.Read.All"
$appRoleId = az ad sp show --id $graphId --query "appRoles[?value=='Group.Read.All'].id | [0]" 
```
Time to make the REST call to assign the permissions as shown above to the Managed Identity:
```
#Set values
$webAppName="LuiseDemo-functionapp$rand"
$principalId=$(az resource list -n $webAppName --query [*].identity.principalId --out tsv)
$graphResourceId=$(az ad sp list --display-name "Microsoft Graph" --query [0].objectId --out tsv)
$appRoleId=$(az ad sp list --display-name "Microsoft Graph" --query "[0].appRoles[?value=='Group.Read.All' && contains(allowedMemberTypes, 'Application')].id" --out tsv)
$body="{'principalId':'$principalId','resourceId':'$graphResourceId','appRoleId':'$appRoleId'}"

#the actual REST call 
az rest --method post --uri https://graph.microsoft.com/v1.0/servicePrincipals/$principalId/appRoleAssignments --body $body --headers Content-Type=application/json
```

> 📝 **Tip** - < you may control if everything worked as intended in Azure portal: Azure Active Directory --> Enterprise applications --> Managed Identity

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < how would you several permissions in one go? >

To see things work, we will need to deploy our function into our Functions App. 

1. head over to Visual Studio Code again
2. select **deploy to Functions App** 
3. Select the Functions App yuou already created
4. Confirm the Pop up window by selecting **Deploy**

> 🔎 **Observation** - < this might take a minute >

<!-- check with Marc if I need to explain how to test in Azure portal -->

## 4. Obtain an access token

In this step we want to learn how we could ontain an access token which we needed for a successful HTTP request against Microsoft Graph API: 

### Steps

1. Although we would usually load an authentication library such as Azure.Identity and then  to obtain a token, there is an easier, but not documented way get the token in an Azure  Functions: Following and extrapolating [Obtain tokens for Azure resources](https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity?tabs=powershell#obtain-tokens-for-azure-resources) to Microsoft Graph surprisingly works: 

```
$resourceURI = "https://<AAD-resource-URI-for-resource-to-obtain-token>"
$tokenAuthURI = $env:IDENTITY_ENDPOINT + "?resource=$resourceURI&api-version=2019-08-01"
$tokenResponse = Invoke-RestMethod -Method Get -Headers @{"X-IDENTITY-HEADER"="$env:IDENTITY_HEADER"} -Uri $tokenAuthURI
$accessToken = $tokenResponse.access_token
```

2. have a look at your **IDENTITY_ENDPOINT** and **IDENTITY_HEADER** environment variables at `https://<your-functionappname-here>.scm.azurewebsites.net/ENV.cshtml#envVariables`

## 5. Homework

<!-- check with Marc what would be appropriate homework -->

## 6. More info
<!-- 
check with Marc about what should go in this section -->

---
[🔼 Lessons Index](../../README.md)
