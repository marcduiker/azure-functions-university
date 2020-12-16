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
    > 🔎 __Observation__ - When you see output such as this, the Azure CLI is available. If not please check the [prerequisites](prerequisites.md) and install the Azure CLI.

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

    ```ps
    az account set -s {SUBSCRIPTION ID}
    ```

    > 📝 __Tip__ - If you need help with the Azure CLI or just want to explore the functionality append `-h` at the end of the command, such as `az account set -h`.

4. Now we can start with creating the first resource, the Resource Group. Since we'll be executing several commands it's useful to use variables for the values which we'll be using often. We're using PowerShell syntax in these examples, so variables start with `$`.

    ```ps
    $location="{LOCATION_NAME}"
    # e.g. $location="westeurope"

    $rgname="{RESOURCE_GROUP_NAME}"
    # e.g. $rgname="myfirstfunction-rg"

    az group create --name $rgname --location $location --tags type=temp
    ```

    > 🔎 __Observation__ - Here we're using variables for the region we're creating the resource in, and for the Resource Group name.

    > 📝 __Tip__ - Always try to use a location which is close to you (and your users) to minimize latency. You can view all the possible locations by typing:

    ```ps
    az account list-locations --query [].name
    ```

    > 🔎 __Observation__ - The `--tags type=temp` part is optional, however it's a good practice to label your resources so you can manage them better. In this case the Resource Group is labelled as `temp` which indicates it is temporary and can be deleted without problems.

    > ❔ __Question__ - Inspect the json output of the command.Is the resource completed successfully?

5. Now let's create a Storage Account in this Resource Group:

    ```ps
    $stname="{STORAGE_NAME}"
    # e.g. $stname="myfirstfunctionst"

    az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot
    ```

    > 📝 __Tip__ - Storage Account names need to be unique within Azure and have quite some [restrictions on the length and the characters](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview#naming-storage-accounts) that can be used. You can check the Storage Account name before you create the account via: `az storage account check-name --name "myfirstfunctionst"`.

    > ❔ __Question__ - Investigate the other arguments of this command, such as, `sku`, `kind` and `access-tier`. What do they mean?

    > ❔ __Question__ - What does the output look like? Is the Storage Account created successfully?

6. Now we can create the Function App & App Service Plan resources. This can be done using one command:

    ```ps
    az functionapp create --name "{FUNCTION_APP_NAME}" --resource-group $rgname --consumption-plan-location $location --storage-account $stname --runtime dotnet --os-type Windows --functions-version 3
    ```

    > 🔎 __Observation__ - Notice that we're creating a .NET based Function App based on Windows using the Azure Function Runtime v3.

    > ❔ __Question__ - What does the output look like? Is the Function App resource created successfully?

     > 🔎 __Observation__ - At this point we have the required Azure resources but we still need to deploy our function code to the Function App in the cloud.

7. To verify that the Function App and App Service Plan are available you can run this command to list all the Function Apps:

    ```ps
    az functionapp list --out table
    ```

    > 📝 __Tip__ - Note that we're using the `table` output formatting to make the output more readable.

## 4. Deployment using Azure Functions CLI

The goal of this exercise is to deploy the Function App project to the cloud using the Azure Functions CLI. We'll deploy the Function App we created in the [HTTP Lesson](http.md) but you can choose any Function App you wish to deploy.

The Azure Functions CLI is part of the Azure Functions Core Tools which you probably already have installed if you've completed one of the other lessons. As with the previous exercise you can either use the Azure CLI from the terminal in VSCode or use a separate terminal/command prompt.

### Steps

1. Type `func` in the terminal.

    > 🔎 __Observation__ - When you see output as shown below, the Azure Functions CLI is available. If not please check the [prerequisites](prerequisites.md) and install the Azure Functions Core Tools.

    ```text
                  %%%%%%
                 %%%%%%
            @   %%%%%%    @
          @@   %%%%%%      @@
       @@@    %%%%%%%%%%%    @@@
     @@      %%%%%%%%%%        @@
       @@         %%%%       @@
         @@      %%%       @@
           @@    %%      @@
                %%
                %

    Azure Functions Core Tools (3.0.2931 Commit hash: d552c6741a37422684f0efab41d541ebad2b2bd2)
    Function Runtime Version: 3.0.14492.0
    Usage: func [context] [context] <action> [-/--options]
    ...
    ```

2. To publish your local Function App to the Azure make sure you're in the folder that contains the project file of the Function App.
3. Type the following command, and make sure you use the exact same Function App name as you did in the previous exercise (Exercise 3, Step 6) when the resource was created:

    ```text
    func azure functionapp publish "{FUNCTION_APP_NAME}" --publish-local-settings -i
    ```

    > 🔎 __Observation__ - Look closely at the output so you can see what this command is doing. It should be similar to the following output.

    ```text
    Microsoft (R) Build Engine version 16.8.0+126527ff1 for .NET
    Copyright (C) Microsoft Corporation. All rights reserved.

    Determining projects to restore...
    All projects are up-to-date for restore.
    MyFirstAzureFunction -> {LOCAL PATH TO THE FUNCTION DLL}

    Build succeeded.        
        0 Warning(s)        
        0 Error(s)

    Time Elapsed 00:00:11.33


    Getting site publishing info...
    Creating archive for current directory...
    Uploading 2,3 MB [################################################################################]
    Upload completed successfully.
    Deployment completed successfully.
    ```

4. After the deployment step there will be a question if you want to replace the value for the `AzureWebJobsStorage` setting with the value from `local.settings.json`. The `AzureWebJobsStorage` setting contains the connection string to the Azure Storage Account the Function App is using. If you have completed Exercise 3 Step 6 than this is set correctly, therefore don't overwrite it with the (empty) local value. Type `no` and press enter.

    ```text
    App setting AzureWebJobsStorage is different between azure and local.settings.json
    Would you like to overwrite value in azure? [yes/no/show]
    no
    Setting FUNCTIONS_WORKER_RUNTIME = ****
    Syncing triggers...
    Functions in {FUNCTION_APP_NAME}:
        HelloWorldHttpTrigger - [httpTrigger]
            Invoke url: {URL TO HTTP FUNCTION}
    ```

    > ❔ __Question__ - Try to invoke the deployed function app. Does it work as expected?

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