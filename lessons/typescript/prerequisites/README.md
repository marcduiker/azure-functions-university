# Prerequisites (TypeScript)

## Frameworks & Tooling 🧰

In order to complete the the lessons using TypeScript you need to install the following:

|Prerequisite|Lessons|Description
|-|-|-
|[Node.js](https://nodejs.org/en/download/)|All| The Node.js runtime including the node package manager NPM. Install an _LTS_ version.
|[TypeScript](https://www.typescriptlang.org/download)|All| The TypeScript extension of JavaScript
|[VSCode](https://code.visualstudio.com/Download)|All|A great cross platform code editor.
|[VSCode Azure Functions extension](https://github.com/Microsoft/vscode-azurefunctions)|All|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|All|Azure Functions runtime and CLI for local development.
|[RESTClient for VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or [Postman](https://www.postman.com/)|All|An extension or  application to make HTTP requests.
|[Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)|Blob, Queue, Table|Application to manage Azure Storage resources (both in the cloud and local emulated).
|[Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (Windows only) or [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)|Blob, Queue, Table|Emulator for using Azure Storage services if you want to develop locally without connecting to a Storage Account in the cloud. If you can't use an emulator you need an [Azure Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).
|[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/what-is-azure-cli)|Deployment|Command line interface used to manage Azure resources. Can be run on your local dev environment, in a deployment pipeline or in the [Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/overview).

> 📝 **Tip** - Azure Functions only support the long term support (LTS) versions of Node.js as shown in the [Azure Functions runtime versions overview](https://docs.microsoft.com/en-us/azure/azure-functions/functions-versions). Consequently, there is always a bit of a lag between the official availability of a Node.js LTS release and the official support of this release in Azure Functions.

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

---
[🔼 Lessons Index](../../README.md)
