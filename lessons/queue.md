# Queue Trigger & Bindings

## Goal 🎯

The goal of this lesson is to learn how to trigger a function by putting a message on a queue, and how you can bind an output message to a queue.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|1|Using the Microsoft Azure Storage Explorer and Storage Emulator for Queues
|2|Using `string` Queue output bindings
|3|Using custom typed Queue output bindings
|4|Using `CloudQueueMessage` Queue output bindings
|5|Using `dynamic` Queue output bindings
|6|Using `ICollector<T>` Queue output bindings
|7.1|Creating a default Queue triggered function
|7.2|Examine & run the Queue triggered function
|7.3|Change the Queue triggered function
|8|Host.json settings

> 📝 __Tip__ - If you're stuck at any point you can have a look at the [source code](../src/AzureFunctions.Queue) in this repository.

---

## 1. Using the Microsoft Azure Storage Explorer and Storage Emulator for Queues

In this exercise we'll look into storage emulation and the Azure Storage Explorer to see how you can interact with queues and messages.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 2. Using `string` Queue output bindings

In this exercise, we'll be creating an HttpTrigger function and use the Queue output binding with a `string` type in order to put player messages on the `newplayer-items` queue.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 3. Using custom typed Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use the Queue output binding with the `Player` output type in order to put player messages on the `newplayer-items` queue.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 4. Using `CloudQueueMessage` Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use the Queue output binding with the `CloudQueueMessage` output type in order to put player messages on the `newplayer-items` queue.

### Steps

1. Create a copy of the `NewPlayerWithTypedQueueOutput.cs` file and rename the file, the class and the function to `NewPlayerWithCloudQueueMessageOutput`.
2. We'll be using a new output type, called `CloudQueueMessage`. To use the latest version of this type add a reference to the `Azure.Storage.Queues` NuGet package to the project.

   > 📝 __Tip__ - One way to easily do this is to use the _NuGet Package Manager_ VSCode extension:
   > 1. Run `NuGet Package Manager: Add new Package` in the Command Palette (CTRL+SHIFT+P).
   > 2. Type: `Azure.Storage.Queues`
   > 3. Select the most recent (non-preview) version of the package.

3. Change the output type of the Queue binding from:

    ```csharp
    out Player playerOutput
    ````

    to

    ```csharp
    out CloudQueueMessage message
    ```

    > 📝 __Tip__ Ensure that the `CloudQueueMessage` type is from the new `Microsoft.Azure.Storage.Queue` namespace and not the old `Microsoft.WindowsAzure.Storage.Queue` namespace.

4. Now that we have defined a new output parameter named `message` we still need to set it with player data. Replace:

    ```csharp
    playerOutput = null;
    ```

    with:

    ```csharp
    message = null;
    ```

5. A `CloudQueueMessage` is constructed with a `byte` array, or `string` as the message content. Let's use the `string` content option so serialize the `Player` object. Put the following code inside the `else` statement:

    ```csharp
    var serializedPlayer = JsonConvert.SerializeObject(player);
    message = new CloudQueueMessage(serializedPlayer);
    result = new AcceptedResult();
    ```

6. Build & run the `AzureFunctions.Queue` Function App.
7. Make a POST call to the `NewPlayerWithCloudQueueMessageOutput` endpoint and provide a valid json body with a `Player` object:

      ```http
      POST http://localhost:7071/api/NewPlayerWithCloudQueueMessageOutput
      Content-Type: application/json

      {
         "id": "{{$guid}}",
         "nickName" : "Ada",
         "email" : "ada@lovelace.org",
         "region" : "United Kingdom"
      }
      ```

8. > ❔ __Question__ - Inspect the `newplayer-items` queue. does it contain a new message?  

## 5. Using `dynamic` Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use dynamic output bindings in order to put valid player messages on the `newplayer-items` queue, and invalid messages on a `newplayer-error-items` queue.

> 📝 __Tip__ - Dynamic bindings are useful when output or input bindings can only be determined at runtime. In this case we'll use the dynamic binding to determine the queue name at runtime.

### Steps

1. Create a copy of the `NewPlayerWithTypedQueueOutput.cs` file and rename the file, the class and the function to `NewPlayerWithDynamicQueueOutput`.
2. Replace the line with the `Queue` binding attribute and parameter with:

   ```csharp
   IBinder binder
   ```

3. Remove the content of the method.
4. Now add the following sections to the method:

   1. First, initialize the result of the HTTP response and the name of the queue:

      ```csharp
         IActionResult result;
         string queueName;
      ```

   2. Second, add an `if/else` statement to determine the name of the queue and the HTTP response:

      ```csharp

         if (string.IsNullOrEmpty(player.Id))
         {
            queueName = QueueConfig.NewPlayerErrorItems;
            result = new BadRequestObjectResult("No player data in request.");
         }
         else
         {
            queueName = QueueConfig.NewPlayerItems;
            result = new AcceptedResult();
         }
      ```

   3. Then, serialize the `Player` object and create an instance of a `CloudQueueMessage`. The cloudQueueMessage will be used in the dynamic queue binding:

      ```csharp
      var serializedPlayer = JsonConvert.SerializeObject(player);
      var cloudQueueMessage = new CloudQueueMessage(serializedPlayer);
      ```

      > 📝 __Tip__ Make sure you use the CloudQueueMessage from the `Microsoft.Azure.Queue` namespace and **not**  the `Microsoft.WindowsAzure.Storage.Queue` namespace (the latter one is outdated). A dependency to the `Azure.Storage.Queues` NuGet package is required for this.

   4. Finally, create a new `QueueAttribute`, create an instance of a `CloudQueue` using the binder interface and add the cloudQueueMessage to the queue as follows:

      ```csharp
      var queueAttribute = new QueueAttribute(queueName);
      var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttribute);
      await cloudQueue.AddMessageAsync(cloudQueueMessage);

      return result;
      ```

      > ❔ __Question__ Look into the `CloudQueue` type. Which other operations does this type have?

5. Build & run the `AzureFunctions.Queue` Function App.
6. First make a POST call to the `NewPlayerWithDynamicQueueOutput` endpoint and provide a valid json body with a `Player` object:

      ```http
      POST http://localhost:7071/api/NewPlayerWithDynamicQueueOutput
      Content-Type: application/json

      {
         "id": "{{$guid}}",
         "nickName" : "Ada",
         "email" : "ada@lovelace.org",
         "region" : "United Kingdom"
      }
      ```

7. > ❔ __Question__ - Inspect the `newplayer-items` queue. Does it contain a new message?
8. Now make a POST call to the `NewPlayerWithCloudQueueMessageOutput` endpoint and provide an __invalid__ player json body as follows:

      ```http
      POST http://localhost:7071/api/NewPlayerWithDynamicQueueOutput
      Content-Type: application/json

      {
         "nickName" : "Ada",
      }
      ```

9. > ❔ __Question__ - Inspect the `newplayer-error-items` queue. Does it contain a new message?

## 6. Using `ICollector<T>` Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use the Queue output binding with the `ICollector<Player>` output type in order to put multiple player messages on the `newplayer-items` queue when the HTTP request contains an array of `Player` objects.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 7.1 Creating a default Queue triggered function

In this exercise we'll create a new QueueTriggered function and trigger it with a message.

### Steps

1. Create a new Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).

   > 📝 __Tip__ - Create a folder with a descriptive name since that will be used as the name for the project, e.g. `AzureFunctionsUniversity.Queue`.

2. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
3. Select `QueueTrigger` as the template.
4. Give the function a name (e.g. `HelloWorldQueueTrigger`).
5. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
6. Select `Create a new local app setting`.

   > 🔎 __Observation__ - The local app settings file (local.settings.json) is used to store environment variables and other useful configurations.

7. Select the Azure subscription you will be using.
8. Since we are using the QueueTrigger, we need to provide a storage account, select one or create a new storage account.
   1. If you select a new one, provide a name (we chose `azfuncstor`). The name you provide must be unique to all Azure.
9. Select a resource group or create a new one.
   1. If you create a new one, you must select a region. Use the one closest to you.
10. Enter the name of the storage queue, you can leave the default value `myqueue-items` if you'd like or change it. Make sure to keep this in mind as we will be referencing it later on.
11. When asked about storage required for debugging choose _Use local emulator_.

   ![AzureFunctionsRuntime storage](../img/lessons/queue/AzureFunctionsStorage.png)

Now the Function App with a Queue Trigger function will be created.

## 7.2 Examine & Run the Function App

Great, we've got our Function Project and Queue Trigger created, let's examine what has been generated for us.

```csharp
public static class HelloWorldQueueTrigger
{
    [FunctionName("HelloWorldQueueTrigger")]
    public static void Run(
        [QueueTrigger(
            "myqueue-items",
            Connection = "azfuncstor_STORAGE")]string myQueueItem,
            ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
    }
}
```

1. > 🔎 __Observation__ The `QueueTrigger` indicates this function will be triggered based on queue messages. The first parameter in this attribute is the name of the queue, `myqueue-items`. The `Connection` parameter contains the name of the application setting which contains the connection string. In this case a setting called `azfuncstor_STORAGE` should be present in the `local.settings.json`.

2. > 🔎 __Observation__ The queue message itself, named `myQueueItem`, is read as a string and outputted to the log inside the method.

3. Build and run the Function App.

4. The function will only be triggered when a message is put on the `myqueue-items` queue. Use the Azure Storage Explorer to add a message to this queue.

5. ❔ __Question__ - Is the function triggered once you've put a message on the queue? How can you determine this?

## 7.3. Change the Queue triggered function

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

> 📝 __Tip__ - Calling a Queue triggered function via HTTP

## 8. Host.json settings for queues

https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-output?tabs=csharp#hostjson-settings

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

> 📝 __Tip__ - Calling a Queue triggered function via HTTP

## More info

For more info about the Queue Trigger and binding have a look at the official [Azure Functions Queue Storage and Bindings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue) documentation.

---
[◀ Previous lesson](blob.md) | [🔼 Index](_index.md) | [Next lesson ▶](table.md)