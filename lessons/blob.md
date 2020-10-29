# Blob Bindings

## Goal

The goal of this lesson is to create a function that reacts to changes in blob storage data as well as read and write data with input and output bindings.

## Blob Trigger

## 1. Creating a Function App

First, you'll be creating a Function App with the BlobTrigger and review the generated code.

### Steps

1. Create the Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).

   > 📝 __Tip__ - Create a folder with a descriptive name since that will be used as the name for the project.

2. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
3. Select `BlobTrigger` as the template.
4. Give the function a name (e.g. `HelloWorldBlobTrigger`).
5. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
6. Select `Create a new local app setting` 


   > 🔎 __Observation__ - The local app setting file is used to store environment variables and other useful configuration.

7. Select the Azure subscription you will be using.
8. Since we are using the BlobTrigger, we need to provide a storage account, select one or create a new storage account.
   1. If you select a new one, provide a name. The name you provide must be unique to all Azure.
9.  Select a resource group or create a new one.
   2.  If you create a new one, you must select a region. Use the one closet to you.
10. Enter the path that the trigger will monitor, you can leave the default value `samples-workitems` if you'd like or change it. Make sure to keep this in mind as we will be referencing it later on.
11. Hit enter and your project will begin to create.

## 2. Examining the Function App

Great, we've got our Function Project and Blob Trigger created, let's examine what has been generated for us.

```csharp

public static void Run([BlobTrigger("samples-workitems/{name}", Connection = "azfunctionsuniversitygps_STORAGE")]Stream myBlob, string name, ILogger log)
{
   log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
}

```

This is the function with BlobTrigger created for us. A few things in here were generated and set for us thanks to the wizard. Let's look at the binding.

```csharp
[BlobTrigger("samples-workitems/{name}", Connection = "azfunctionsuniversitygps_STORAGE")]Stream myBlob
```

We can see this BlobTrigger has a few parts:

- **"samples-workitems/{name}"**: This is the path we set that the function will monitor.
- **Connection = "azfunctionsuniversitygps_STORAGE"**: This is the value in our local.settings.json file where our connection string to our storage account is stored.
- **Stream myBlob**: This is the object where the blob that triggered the function will be stored and can be used in code.

As for the body of the function:

```csharp
log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
```
the name and size of the blob that triggered the function will print to console. 

## 3. Install Azure Storage explorer

Now it's time to test, but first, make sure you have Azure Storage Explorer installed and setup with the account you used to create this Azure function. 
[Instructions here](https://azure.microsoft.com/en-us/features/storage-explorer/)
> 📝 __Tip__ - Azure Storage Explorer is a fantastic tool, spend a bit of time poking around before you dive into the next step. It's useful for visualizing the data in your storage accounts.

Once you've installed Storage Explorer, make sure to create a blob container that matches the one you set when creating the function in the visual code wizard, the default is samples-workitems. You can do this by expanding the storage account you selected, right click on blob containers, and select create blob container.
![Storage Explorer sample-items](/img/lessons/blob/storage-explorer-sample-items.png)

## 4. Run the function

Okay now it actually is time to fun the function, go ahead and run it, and then add a file to the path that the function is monitoring. 

## Blob Input Binding

### Steps

## Blob Output Binding

### Steps

## More info

[Back to the index](_index.md)