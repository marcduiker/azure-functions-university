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
2. 
3. 

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

## 5. Using `dynamic` Queue output bindings

In this exercise, we'll be adding an HttpTrigger function and use dynamic output bindings in order to put valid player messages on the `newplayer-items` queue, and invalid messages on a `newplayer-error-items` queue.

### Steps

1.
2.
3.

> 📝 __Tip__ < TIP >

> 🔎 __Observation__ < OBSERVATION >

> ❔ __Question__ - < QUESTION >

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