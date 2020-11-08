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


## Using `string` Blob ouput bindings

In this exercise, we'll be creating a HTTP Function App with the default HTTPTrigger and extend it with a Blob output binding in order to write a `Player` json object to Blob Storage.

### Steps

1. In VSCode Create a new HTTP Trigger Function App with the following settings:
   1. Location: _AzureFunctions.Blob_
   2. Language: _C#_
   3. Template: _HttpTrigger_
   4. Function name: _StorePlayerWithStringBlobOutput_
   5. Namespace: _AzureFunctionsUniversity.Demo_  
   6. AccessRights: _Function_
2. Once the Function App is generated add a reference to the `Microsoft.Azure.WebJobs.Extensions.Storage` NuGet package to the project. This allows us to use bindings for Blobs, Tables and Queues. 

   > 📝 __Tip__ - One way to easily do this is to use the _NuGet Package Manager_ VSCode extension:
   >   1. Run `NuGet Pacakge Manager: Add new Package` in the Command Palette (CTRL+SHIFT+P).
   > 2. Type: `Microsoft.Azure.WebJobs.Extensions.Storage`
   > 3. Select the most recent (non-preview) version of the package.

3. We want to store an object with (game)player data. Create a new file in the project called _Player.cs_ and add the contents from this [Player.cs](../src/AzureFunctions.Blob/Models/Player.cs) file.
4. Now open the `StorePlayerWithStringBlobOutput.cs` function file and add the following output binding directly underneath the `HttpTrigger` method argument:
   ```csharp
   [Blob("players/out/string-{rand-guid}.json", FileAccess.Write)] out string playerBlob
   ``` 
    > 🔎 __Observation__ - The first part parameter of the Blob attibute is the full path where the blob will be stored. The __{rand-guid}__ section in path is a so-called __binding expression__. This specific expression creates a random guid. There are more expressions available as is described [in the documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns). The second parameter indicates we are writing to Blob Storage. Finally we specify that there is an output argument of type `string` named _playerBlob_.
5. We'll be doing a post to this function so the `"get"` can be removed from the `HttpTrigger` attribute.
6. Change the function input type and name from `HttpRequest req` to `Player player` so we have direct acccess to the Player object in the request.
7. Remove the existing content of the function method.
8. To return a meaningful response the the client, based on a valid Player object, add the following lines of code in the method:
   ```csharp
   IActionResult result;
   if (player == null)
   {
      result = new BadRequestObjectResult("No player data in request.");
   }
   else
   {
      result = new AcceptedResult();
   }
   ```
9. Since we're using `string` as the output type the Player object needs to be serialized. This can be done as follows:
   ```csharp
   playerBlob = JsonConvert.SerializeObject(player, Formatting.Indented);
   ```
10. Build & Run the `AzureFunctions.Blob` Function App.
11. Make a POST call to the `StorePlayerWithStringBlobOutput` endpoint and provide a valid json body with a Player object.
   ```http
   POST http://localhost:7071/api/StorePlayerWithStringBlobOutput
   Content-Type: application/json

   {
      "id": "{{$guid}}",
      "nickName" : "Ada",
      "email" : "ada@lovelace.org",
      "region" : "United Kingdom"
   }
   ```
12.  > ❔ __Question__ - Is there a blob created blob storage? What is the exact path of the blob?
13.  > ❔ __Question__ - What do you think would happen if you run the function again with the exact same input?

## Using `CloudBlobContainer` Blob ouput bindings

In this exercise, we'll be adding an HttpTrigger function and use the Blob output binding with the `CloudBlobContainer` type in order to write a `Player` json object to Blob Storage.

### Steps

## Using `dynamic` Blob ouput bindings

In this exercise, we'll be adding an HttpTrigger function and use a dynamic Blob output binding in order to write a `Player` json object to Blob Storage.

### Steps