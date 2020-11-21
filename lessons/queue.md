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
|7|Creating a default Queue triggered function
|8|Change the Queue triggered function
|9|Host.json settings

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

### Steps

1. Create a copy of the `NewPlayerWithTypedQueueOutput.cs` file and rename the file, the class and the function to `NewPlayerWithDynamicQueueOutput`.
2.
3.
4.
5.

6. Build & run the `AzureFunctions.Queue` Function App.
7. First make a POST call to the `NewPlayerWithCloudQueueMessageOutput` endpoint and provide a valid json body with a `Player` object:

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

8. > ❔ __Question__ - Inspect the `newplayer-items` queue. Does it contain a new message?
9. Now make a POST call to the `NewPlayerWithCloudQueueMessageOutput` endpoint and provide an __invalid__ player json body as follows:

      ```http
      POST http://localhost:7071/api/NewPlayerWithCloudQueueMessageOutput
      Content-Type: application/json

      {
         "nickName" : "Ada",
      }
      ```

10. > ❔ __Question__ - Inspect the `newplayer-error-items` queue. Does it contain a new message?

## 6. Using `ICollector<T>` Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use the Queue output binding with the `ICollector<Player>` output type in order to put multiple player messages on the `newplayer-items` queue when the HTTP request contains an array of `Player` objects.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 7. Creating a default Queue triggered function

In this exercise we'll create a new QueueTriggered function and trigger it with a message.
https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger?tabs=csharp

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 8. Change the Queue triggered function

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

> 📝 __Tip__ - Calling a Queue triggered function via HTTP

## 9. Host.json settings for queues

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

https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-output?tabs=csharp

---
[◀ Previous lesson](blob.md) | [🔼 Index](_index.md) | [Next lesson ▶](table.md)