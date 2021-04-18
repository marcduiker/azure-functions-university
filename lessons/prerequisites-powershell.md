# Prerequisites

## Frameworks & Tooling 🧰

In order to complete the the lessons you need to install the following:

|Prerequisite|Lessons|Description
|-|-|-
|[PowerShell 7+](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)| All | PowerShell 7 or higher
|[.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core)|All|The .NET runtime and SDK.
|[VSCode](https://code.visualstudio.com/Download)|All|A great cross platform code editor.
|[VSCode AzureFunctions extension](https://github.com/Microsoft/vscode-azurefunctions)|All|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|All|Azure Functions runtime and CLI for local development.
|[VSCode PowerShell Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.PowerShell)| All| Extension for working with PowerShell
|[Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)|Blob, Queue, Table|Application to manage Azure Storage resources (both in the cloud and local emulated).
|[Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (Windows only) or [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)|Blob, Queue, Table|Emulator for using Azure Storage services if you want to develop locally without connecting to a Storage Account in the cloud. If you can't use an emulator you need an [Azure Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).
|[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/what-is-azure-cli)|Deployment, Configuration|Command line interface used to manage Azure resources. Can be run on your local dev environment, in a deployment pipeline or in the [Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/overview).

## Creating your local workspace 👩‍💻

We strongly suggest you create a new folder (local git repository) and use this Azure Functions University repository for reference only (for when you're stuck).

- Create a new folder to work in:

    ```PowerShell
    New-Item -Type Directory -Name azfuncuniversity
    cd .\azfuncuniversity\
    ```

- Turn this into a git repository:

    ```PowerShell
    git init
    ```

- Add subfolders for the source code and test files:

    ```PowerShell
    New-Item -Type Directory -Name src
    New-Item -Type Directory -Name tst
    ```

You should be good to go now!

---
[🔼 Index](_index.md) | [Next (HTTP Lesson) ▶](PowerShell/http/http-lesson-powershell.md)
