# Prerequisites (.NET Core)

## Frameworks & Tooling 🧰

In order to complete the the lessons you need to install the following:

|Prerequisite|Lessons|Description
|-|-|-
|[.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)|All|The .NET runtime and SDK.
|[VSCode](https://code.visualstudio.com/Download)|All|A great cross platform code editor.
|[VSCode AzureFunctions extension](https://github.com/Microsoft/vscode-azurefunctions)|All|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|All|Azure Functions runtime and CLI for local development.
|[RESTClient for VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or [Postman](https://www.postman.com/)|All|An extension or  application to make HTTP requests.
|[Azure Storage Explorer](https://azure.microsoft.com/features/storage-explorer/)|Blob, Queue, Table|Application to manage Azure Storage resources (both in the cloud and local emulated).
|[Azure Storage Emulator](https://docs.microsoft.com/azure/storage/common/storage-use-emulator) (Windows only) or [Azurite](https://docs.microsoft.com/azure/storage/common/storage-use-azurite)|Blob, Queue, Table|Emulator for using Azure Storage services if you want to develop locally without connecting to a Storage Account in the cloud. If you can't use an emulator you need an [Azure Storage Account](https://docs.microsoft.com/azure/storage/common/storage-account-create?tabs=azure-portal).
|[Azure CLI](https://docs.microsoft.com/cli/azure/what-is-azure-cli)|Deployment, Configuration|Command line interface used to manage Azure resources. Can be run on your local dev environment, in a deployment pipeline or in the [Azure Cloud Shell](https://docs.microsoft.com/azure/cloud-shell/overview).

## Creating your local workspace 👩‍💻

We strongly suggest you create a new folder (local git repository) for each lesson and use this Azure Functions University repository for reference only (for when you're stuck).

- Create a new folder to work in:

    ```cmd
    C:\dev\mkdir azfuncuni
    C:\dev\cd .\azfuncuni\
    ```

- Turn this into a git repository:

    ```cmd
    C:\dev\azfuncuni\git init
    ```

- Add subfolders for the source code and test files:

    ```cmd
    C:\dev\azfuncuni\mkdir src
    C:\dev\azfuncuni\mkdir tst
    ```

You should be good to go now!

## Feedback

We love to hear from you! Was this section useful to you? Is anything missing? Let us know in a [Feedback discussion post](https://github.com/marcduiker/azure-functions-university/discussions/new?category=feedback&title=.NET%20Core%20Prerequisites) here on GitHub.

---
[🔼 Lessons Index](../../README.md) |
