# Deployment to Azure

## Goal 🎯

The goal of this lesson is to learn about how to deploy your Function App to Azure. In order to complete this lesson you need an Azure Subscription.

Before you can deploy your functions, the required Azure resources need to be created first. This can be done in many different ways. It can be done straight from an IDE such as VSCode or full Visual Studio, via command line tooling, or via a CI/CD pipeline. We'll cover various deployment options in this lesson.  

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|1|[Understanding the Azure Resources](#1-understanding-the-azure-resources)
|2|[Deployment using VSCode](#2-deployment-using-vscode)
|3|[Deployment using the Azure CLI & Functions CLI](#3-deployment-using-the-azure-cli-&-functions-cli)
|4|[Deployment using GitHub Actions](#4-deployment-using-github-actions)

---

## 1. Understanding the Azure Resources

The goal of this exercise is understand the resources that are required for an Azure Function App.

In the diagram below the resources are shown:

```text
+------------------------------+
|                              |
|  +------------------------+  |
|  |                        |  |
|  |  +------------------+  |  |
|  |  |                  |  |  |
|  |  |   Function App   |  |  |
|  |  |                  |  |  |
|  |  +------------------+  |  |
|  |                        |  |
|  |    App Service Plan    |  |
|  |                        |  |
|  +------------------------+  |
|                              |
|  +------------------------+  |
|  |                        |  |
|  |    Storage Account     |  |
|  |                        |  |
|  +------------------------+  |
|                              |
|        Resource Group        |
|                              |
+------------------------------+
```

From the outside to the inside these resources are:

- Resource Group: A logical grouping of related Azure resources.
- Storage Account: An Azure Storage Account where the Function App files are stored. When Azure Functions is scaling out, the files are copied from this storage account to the virtual machine instances which host your Function App.
- App Service Plan: An App Service Plan resource defines a set of compute resources used for App Services or Function Apps. For App Services or Azure Functions Premium plan, you get the option to select the size of the VM instances and how much they can scale. For the Azure Function consumption plan, you don't have these options.
- Function App: The Function App resource which runs the Azure Functions Runtime and executes your code. The Function App resource also has application settings (since the `local.settings.json` are only used on your local development environment).

## 2. Deployment using VSCode

The goal of this exercise is to create Azure resources and deploy the Function App using VSCode.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 3. Deployment using the Azure CLI & Functions CLI

The goal of this exercise is to create Azure resources and deploy the Function App using the Azure CLI and the Azure Functions CLI.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 4. Deployment using GitHub Actions

The goal of this exercise is to create Azure resources and deploy the Function App using GitHub Actions.

https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-github-actions?tabs=dotnet
### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## More info

---
[◀ Previous lesson](<previous>.md) | [🔼 Index](_index.md) | [Next lesson ▶](<next>.md)