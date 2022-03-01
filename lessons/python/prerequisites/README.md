# Prerequisites (Python)

## Frameworks & Tooling 🧰

In order to complete the the lessons using Python you need to install the following:

|Prerequisite|Lessons|Description
|-|-|-
|[Node.js](https://nodejs.org/en/download/)|All| The Node.js runtime including the node package manager NPM. Install an _LTS_ version.
|[Python](https://www.python.org/downloads/)|All| Python releases. Install 3.9 version.
|[VSCode](https://code.visualstudio.com/Download)|All|A great cross platform code editor.
|[VSCode Azure Functions extension](https://github.com/Microsoft/vscode-azurefunctions)|All|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|All|Azure Functions runtime and CLI for local development.
|[RESTClient for VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or [Postman](https://www.postman.com/)|All|An extension or  application to make HTTP requests.
|[Virtual Environment](https://docs.python.org/3/library/venv.html)|All|Virtual environment for Python. After installing it, run _python -m venv venv_ in your terminal. After running this you will see a pop up in VSCode: _We noticed a new virtual environment has been created. Do you want to select it for the workspace folder?_ Click _Yes_ and your _venv_ is ready to use.
|[Azure CLI](https://docs.microsoft.com/cli/azure/what-is-azure-cli)|Deployment|Command line interface used to manage Azure resources. Can be run on your local dev environment, in a deployment pipeline or in the [Azure Cloud Shell](https://docs.microsoft.com/azure/cloud-shell/overview).

> 📝 **Tip** - Azure Functions only supports the following [Python versions](https://docs.microsoft.com/azure/azure-functions/functions-reference-python?tabs=azurecli-linux%2Capplication-level#python-version).

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
