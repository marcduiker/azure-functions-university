# HTTP Trigger

## Goal

The goal of this lesson is to create your first function which can be triggered by doing an HTTP GET or POST to the function endpoint. 

## Creating a Function App

First you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. Create the Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_). 
> Tip: Create a folder with a descriptive name since that will be used as the name for the project.
3. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
4. Select `HTTPTrigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
7. Select `Function` for the AccesssRights.

Now a new Azure Functions project is generated and once it's done you should see the HTTPTrigger function in the code editor.

> Question: Review the template HTTPTrigger function. What is it doing?

## Changing the template

### Steps

1.

## More info

See the [Azure Functons HTTP Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=csharp) documentation.

[Back to the index](_index.md)