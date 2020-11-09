# Blob Bindings

## **Goal**

The goal of this lesson is to use Blob storage input and output bindings which lets you easily read & write blob data in your functions. In addition you'll create a Blob triggered function that reacts to changes in blob storage data.

This lessons consists of the following exercises:


|Nr|Exercise
|-|-
|1|Using the Microsoft Azure Storage Explorer and Storage Emulator
|2|Using `string` Blob ouput bindings
|3|Using `CloudBlobContainer` Blob ouput bindings
|4|Using `dynamic` Blob ouput bindings
|5|Using `Stream` Blob input bindings
|6|Using `CloudBlobContainer` Blob input bindings
|7|Using `dynamic` Blob input bindings
|8|Creating a Blob triggered function

> 📝 __Tip__ - If you're stuck at any point you can have a look at the [source code](../src/AzureFunctions.Blob) in this repository.


## 1. Using the Microsoft Azure Storage Explorer and Storage Emulator 

We're going to be using local storage instead of creating a storage account in Azure, this is great for local development.  

### Steps

1. Install [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) if you are on windows, if you are using Mac OS or Linux, use [Azurite](https://github.com/Azure/Azurite)
2. Install [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)
3. Run Azure Storage Emulator.
4. Open Azure Storage Explorer, expand Local & Attached > Storage Accounts > (Emulator - Default Ports) (Keys) > Right click on Blob containers and create a new `player` container.
5. 
   ![Storage Explorer sample-items](/img/lessons/blob/storage-explorer-sample-items.png)
6. In the `player` container create a folder called `in`.
   ![In folder](/img/lessons/blob/in-folder.png) 
7. Drag [player-1.json](src/azurefunctions.blob/../../../src/AzureFunctions.Blob/player-1.json) there. You can create more player json files and add them here if you'd like, we've provided one example.
   ![player-1 In folder](/img/lessons/blob/player-1-in-folder.png)  
8. You're now all set to work with local storage.

> 📝 __Tip__ - Read about [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) and [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)
   

## 2..7 TODO
## 5. Using `Stream` Blob input bindings
Let's see how we can use the `Stream` type to work with Blobs. We will create an HTTP Trigger function that expects a player ID in the URL, and with that ID it will return the content from the Blob that matches it. 

### Steps

1. Create a new HTTP Trigger Function App, we will name it GetPlayerWithStreamInput.cs
   
2. We're going to make some changes to the method definition: 
   1. Change the HTTPTrigger Route value, set it to 
      ```csharp
      Route = "GetPlayerWithStreamInput/{id}
      ``` 
   2. Add a parameter to the method
       ```csharp
      string id
       ``` 
   3. Add the Blob Input Binding
      ```csharp
      [Blob("players/in/player-{id}.json", FileAccess.Read)] Stream playerStream
      ``` 
   4. Your method definition should should look like this now:
      ```csharp
      [FunctionName(nameof(GetPlayerWithStreamInput))]
      public static async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetPlayerWithStreamInput/{id}")] HttpRequest request,
         string id,
         [Blob("players/in/player-{id}.json", FileAccess.Read)] Stream playerStream)
      ```
   5. Let's make some edits to the body of the method.
   6. Remove all the code in the body.
   7. Create an object to store our IActionResult
      ```csharp
      IActionResult result;
      ``` 
   8. Let's make sure the id is not empty or null, if it is, return a BadRequestObjectResult with a custom message.
      ```csharp
      if (string.IsNullOrEmpty(id))
      {
         result = new BadRequestObjectResult("No player id route.");
      }
      ``` 
   9.  If we do have a value for id, use StreamReader to get the contents of playerStream and return it
         ```csharp
         else
         {
            using var reader = new StreamReader(playerStream);
            var content = await reader.ReadToEndAsync();
            result = new ContentResult 
            { 
               Content = content,
               ContentType = MediaTypeNames.Application.Json,
               StatusCode = 200
            };
         }
         return result;
         ```  
   > 🔎 __Observation__ -  StreamReader reads characters from a byte stream in a particular encoding. In this demo we are creating a new StreamReader from the playerStream. The ReadToEndAsync() method reads all characters from the playerStream (which is the content of the blob). We then create a result with the content of the blob, json as the ContentType and StatusCode 200 to indicate success.  

   10.   Execute the Function App and provide an ID in the URL. As long as there is a blob with the name matching the ID you provided, you will see the contents of the blob output.
         1.    URL: http://localhost:7071/api/GetPlayerWithStreamInput/1
         2.    Output: (this is the contents of [player-1.json](/src/AzureFunctions.Blob/player-1.json) make sure it's in your local storage blob container, we covered this in the first step of this lesson.)
               ```json
               {
                  "id":"1",
                  "nickName":"gwyneth",
                  "email":"gwyneth@game.com",
                  "region": "usa"
               }
               ``` 

## 8.1 Creating a Blob triggered Function App

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

## 8.2 Examining the Function App

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


## 8.3 Run the function

Okay now it actually is time to fun the function, go ahead and run it, and then add a file to the blob container that the function is monitoring. You should see output similar to this. The name and size of the tile you uploaded will appear in your Visual Studio terminal output.

![Storage Explorer sample-items](/img/lessons/blob/samples-workitems-output.png)

> 🔎 __Observation__ - Great! That's how the BlobTrigger works, can you start to see how useful this trigger could be in your work? 


## Homework

## More info

[Back to the index](_index.md)