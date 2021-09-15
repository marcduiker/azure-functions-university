# Azure Durable Functions - Introduction & Function Chaining Pattern (.NETCore C#)

Watch the recording of this lesson [on YouTube]() //TODO add appropriate link

## Goal 🎯

The goal of this lesson is to give you an introduction into Azure Durable Functions.
In this lesson you will learn how to create your a serverless function app using Azure Durable Functions using chaining pattern.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1| Introduction to Serverless and Azure Durable Functions 
|2| How to Create a Durable Function App project in .NET Core using VS Code 
|3| How to Create a Durable Function App project in .NET Core using Visual Studio 
|4| How to Create a Durable Function App project in .NET Core using Azure Portal
|5| Function Chaining Example with Azure BLOB Trigger
|6|	Implementing a "Real-World" Scenario
|7|	Retries - Dealing with Temporal Errors
|8| Circuit Breaker - Dealing with Timeouts
|9|[Homework](#4-homework)
|10|[More info](#5-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../src/{language}/{topic}) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
|-|-
| Install latest [.NET Core SDK](https://dotnet.microsoft.com/download) if you don't have it. | 2,3,5,6,7,8
| Install [VS Code](https://code.visualstudio.com/download) if you don't have it. | 2,5,6,7,8
| Install [Azure Functions extension for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions). | 2,5,6,7,8
| Install [C# extension for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp). | 2,3,5,6,7,8
| Install latest version of [Azure Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cportal%2Cbash%2Ckeda) | 2,3,5,6,7,8
| Latest [Visual Studio](https://visualstudio.microsoft.com/vs/community/) | 2
| [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/) | 2,5,6,7,8
| A local folder for the Function App. | 2-5


See [{language} prerequisites](../prerequisites/prerequisites-{language}.md) for more details.

## 1. Introduction to Serverless and Azure Durable Functions 

//TODO Describe sub goal

### What is Serverless? 

//TODO Describe what is serverless and connect with Azure durable functions 

### Azure Durable Functions 

Within this section we want to take a look at the motivation for the usage of Azure Durable Functions and take a look at the underlying mechanics.

### 1.1 Functions and Chaining 
In general, functions are a great way to develop functionality in a serverless manner. However, this development should follow some guidelines to avoid drawbacks or even errors when using them. The three main points to consider are:

* Functions must be stateless
* Functions should not call other functions
*  Functions should only do one thing well

While the first guideline is due to the nature of functions, the other two guidelines could easily be ignored but would contradict the paradigms of serverless and loosely coupled systems. In real life scenarios we often have to model processes that resemble a workflow, so we want to implement a sequence of single steps. How can we do that sticking to the guidelines? One common solution for that is depicted below:

![Function Chaining Pattern](https://docs.microsoft.com/en-us/azure/azure-functions/durable/media/durable-functions-concepts/function-chaining.png)

Every function in the picture represents a single step of a workflow. In order to glue the functions together we use storage functionality, such as queues or databases. So Function 1 is executed and stores its results in a table. Function 2 is triggered by an entry in the table via the corresponding bindings and gets executed representing the second step in the workflow. This sequence is then repeated for Function 3. The good news is, that this pattern adheres to the guidelines. But this pattern comes with several downsides namely:

The single functions are only coupled via the event that they react to. From the outside it is not clear how the functions relate to each other although they represent a sequence of steps in a workflow.
The storage functionality between function executions are a necessary evil. One motivation for developing serverless is to care about servers less. Here we must care about the technical infrastructure in order to have our functions loosely coupled.
If you want to pass a context between the functions you must store it (and deal with the potential errors around it).
Handling errors and analyzing bugs in such a setup is very complicated.
Can we do better? Or is there even a solution provided by Azure Functions to handle such scenarios? There is good news - there are Azure Durable Function

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >

## 2.How to Create a Durable Function App project in .NET Core using VS Code 

//TODO Describe sub goal
### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >

## 3. How to Create a Durable Function App project in .NET Core using Visual Studio 

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >

## 4. How to Create a Durable Function App project in .NET Core using Azure Portal

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >


## 5. Function Chaining Example with Azure BLOB Trigger

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >


## 6 .Implementing a "Real-World" Scenario

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >



## 7. Retries - Dealing with Temporal Errors

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >


## 8. Circuit Breaker - Dealing with Timeouts

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >


## 9. Homework

## 10. More info

---
[🔼 Lessons Index](../README.md)
