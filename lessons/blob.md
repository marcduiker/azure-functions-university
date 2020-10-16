# Blob Bindings

## Goal

The goal of this lesson is to create a function that reacts to changes in blob storage data as well as read and write  data with input and output bindings.

## Blob Trigger

## 1. Creating a Function App

First, you'll be creating a Function App with the BlobTrigger and review the generated code.

### Steps

1. Create the Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).
   > 📝 Tip: Create a folder with a descriptive name since that will be used as the name for the project.
2. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
3. Select `BlobTrigger` as the template.
4. Give the function a name (e.g. `HelloWorldBlobTrigger`).
5. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
6. Select `Create a new local app setting` 
   > 💡 Did you know?: The local app setting file is used to store enviornment variables and other useful configuration.
7. Select the Azure subscription you will be using.
8. Since we are using the BlobTrigger, we need to provide a storage account, select one or create a new storage account.
   1. If you select a new one, provide a name. The name you provide must be unique to all Azure.
9. Select a resource group or create a new one.
   1.  If you create a new one, you must select a region. Use the one closet to you.
10. Enter the path that the trigger will monitor, you can leave the default value `samples-workitems` if you'd like or change it. Make sure to keep this in mind as we will be referencing it later on.
11. Hit enter and your project will begin to create.

## 2. Examining the Function App

Great, we've got our Function Project and Blob Trigger created, let's examine what has been generated for us.

### FunctionName.cs 
### local.settings.json
### host.json


## Blob Input Binding

### Steps

## Blob Output Binding

### Steps

## More info

[Back to the index](_index.md)