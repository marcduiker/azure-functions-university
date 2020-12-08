# Deployment to Azure

## Goal 🎯

The goal of this lesson is to learn about how to deploy your Function App to Azure. In order to complete this lesson you need an Azure Subscription.

Before you can deploy your functions, the required Azure resources need to be created first. This can be done in many different ways. It can be done straight from an IDE such as VSCode or full Visual Studio, via command line tooling, or via a CI/CD pipeline. We'll cover various deployment options in this lesson.  

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|1|[Understanding the Azure Resources](#1-understanding-the-azure-resources)
|2|[Deployment using VSCode](#2-deployment-using-vscode)
|3|[Creating Azure Resources using Azure CLI](#3-creating-azure-resources-using-azure-cli)
|4|[Deployment using Azure Functions CLI](#4-deployment-using-azure-functions-cli)
|5|[Deployment using GitHub Actions](#5-deployment-using-github-actions)

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

> 📝 __Tip__ - < TIP >

> 🔎 __Observation__ - < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 3. Creating Azure Resources using Azure CLI

The goal of this exercise is to create Azure resources using the Azure CLI.

You can either use the Azure CLI from the terminal in VSCode or use a separate terminal such as [Windows Terminal](https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab) or the built in command prompt of your OS.

### Steps

1. Type `az` in the terminal.
    > 🔎 __Observation__ - When you see output such as this the Azure CLI is available. If not please check the [prerequisites](prerequisites.md).

    ```text
         /\
        /  \    _____   _ _  ___ _
       / /\ \  |_  / | | | \'__/ _\
      / ____ \  / /| |_| | | |  __/
     /_/    \_\/___|\__,_|_|  \___|


    Welcome to the cool new Azure CLI!

    Use `az --version` to display the current version.
    ```

2. Before you can create or manage Azure resources, you need to authenticate yourself. Type `az login` and follow the instructions.
    > 🔎 __Observation__ - A browser window will open where you can login using your Azure account. Once logged in you can close this browser window.

    > 🔎 __Observation__ - Once logged in, you should see a json output with your subscription info, it could be that you have several subscriptions so you see an array of objects.

3. If you have multiple subscriptions choose the one you'll use to create the Azure resources. Copy the `id` of the subscription from th `az login` output and use in the following command:

    ```text
    az account set -s <SUBSCRIPTION ID>
    ```

    > 📝 __Tip__ - If you need help with the Azure CLI or just want to explore the functionality append `-h` at the end of the command, such as `az account set -h`.

4. Now we can start with creating the first resource, the Resource Group. Since we'll be executing several commands it's useful to use variables for the values which we'll be using often. We're using PowerShell syntax in these examples, so variables start with `$`.

    ```text
    $location="westeurope"

    $rgname="myfirstfunction-rg"

    az group create --name $rgname --location $location --tags type=temp
    ```

    > 🔎 __Observation__ - Here we're using variables for the region we're creating the resource in, and for the Resource Group name.

    > 📝 __Tip__ - Always try to use a location which is close to you (and your users) to minimize latency. You can view all the possible locations by typing:

    ```text
    az account list-locations --query [].name
    ```

    > 🔎 __Observation__ - The `--tags type=temp` part is optional, however it's a good practice to label your resources so you can manage them better. In this case the Resource Group is labelled as `temp` which indicates it is temporary and can be deleted without problems.

    > ❔ __Question__ - Inspect the json output of the command.Is the resource completed successfully?

5. Now let's create a Storage Account in this Resource Group:

    ```text
    $stname="myfirstfunctionst"

    az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot
    ```

    > 📝 __Tip__ - Storage Account names need to be unique within Azure and have quite some [restrictions on the length and the characters](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview#naming-storage-accounts) that can be used. You can check the Storage Account name before you create the account via: `az storage account check-name --name "myfirstfunctionst"`.

    > ❔ __Question__ - Investigate the other arguments of this command, such as, `sku`, `kind` and `access-tier`. What do they mean?

    > ❔ __Question__ - What does the output look like? Is the Storage Account created successfully?

6. Now we can create the Function App & App Service Plan resources. This can be done using one command:

    ```text
    az functionapp create --name "myfirstfunction-fa" --resource-group $rgname --consumption-plan-location $location --storage-account $stname --runtime dotnet --os-type Windows
    ```

    > 🔎 __Observation__ - Notice that we're creating a .NET based Function App based on Windows.

    > ❔ __Question__ - What does the output look like? Is the Function App resource created successfully?

     > 🔎 __Observation__ - At this point we have the required Azure resources but we still need to deploy our function code to the Function App in the cloud.

## 4. Deployment using Azure Functions CLI

The goal of this exercise is to deploy the Function App project to the cloud using the Azure Functions CLI.

### Steps

1.
2.
3.

> 📝 __Tip__ - < TIP >

> 🔎 __Observation__ - < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 5. Deployment using GitHub Actions

The goal of this exercise is to create Azure resources and deploy the Function App using GitHub Actions.

https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-github-actions?tabs=dotnet
### Steps

1.
2.
3.

> 📝 __Tip__ - < TIP >

> 🔎 __Observation__ - < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## More info

---
[◀ Previous lesson](<previous>.md) | [🔼 Index](_index.md) | [Next lesson ▶](<next>.md)