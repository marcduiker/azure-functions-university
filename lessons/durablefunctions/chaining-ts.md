# Azure Durable Functions - Introduction & Chaining

## Goal 🎯

The goal of this lesson is to give you an introduction into Azure Durable Functions including a first durable function that chains two functions calls. In addition we will take a look into some features of Durable Functions that help you when working with durable functions.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0| [Prerequisites](#0-prerequisites)
|1| [Introduction to Azure Durable Functions](#1-introduction-to-azure-durable-functions)
|1.1| [Functions and Chaining](#11-functions-and-chaining)
|1.2| [Solution via Durable Functions](#12-solution-via-durable-functions)
|1.3| [Mechanics of Durable Functions](#13-mechanics-of-durable-functions)
|2| [Creating a Function App project for a Durable Function](#2-creating-a-function-app-project-for-a-durable-function)
|2.1| [The Client Function](#21-the-client-function)
|2.2| [The Orchestrator Function](#22-the-orchestrator-function)
|2.3| [The Activity Function](#23-the-activity-function)
|2.4| [The First Execution](#24-the-first-execution)
|3| [Implementing a "Real-World" Scenario](#3-implementing-a-"real-world"-scenario)
|3.1| [Basic Setup of the Scenario](#31-basic-setup-of-the-scenario)
|3.2| [Implementation of the Activity Function `GetRepositoryDetailsByName`](#32-implementation-of-the-activity-function-`getrepositorydetailsbyname`)
|3.3| [Implementation of the Orchestrator Function](#34-implementation-of-the-orchestrator-function)
|3.4| [Implementation of the Activity Function `GetUserDetailsById`](#33-implementation-of-the-activity-function-`getuserdetailsbyid`)
|3.5| [Test the Implementation](#35-test-the-implementation)
|4| [Retries - Dealing with Temporal Errors](#4-retries---dealing-with-temporal-errors)
|5| [Circuit Breaker - Dealing with Timeouts](#5-circuit-breaker---dealing-with-timeouts)
|6| [Homework](#6-homework)
|7| [More Info](#7-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../src/durablefunctions/chaining/ts) in this repository.

---

## 0 Prerequisites

| Prerequisite | Exercise
| - | -
| A local folder with a Function App. | 2-5
| The [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) for VSCode. | 2, 3
| The [Microsoft Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) | 1-5
| The [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/) | 2-5

> 📝 **Tip** - Up to now the Durable Functions are not compatible with Azurite with respect to the emulation of storage. So if you are on a non-Windows machine you must use a hybrid approach and connect your Durable Functions to a storage in Azure. This means that you need an Azure subscription.

## 1. Introduction to Azure Durable Functions

Within this section we want to take a look at the motivation for the usage of Azure Durable functions and take a look at the basic mechanics.

### 1.1 Functions and Chaining

In general, Functions are a great way to develop functionality in a serverless manner. However, this development should follow some best practices to avoid drawbacks or even errors when using them. The three main points to follow are:

* Functions must be stateless
* Functions should not call other functions
* Functions should only do one thing well

While the first practice is due to the nature of functions, the other two are technically possible but will contradicting the paradigms of serverless. In real life scenarios we often have to model processes that resemble a workflow, so we want to implement a sequence of single steps. How can we do that sticking to the best practices? One common solution for that is depicted below:

![Function Chaining Pattern](../../img/lessons/durablefunctions/chaining/functionchaining.png)

Every function in the picture represents a single step of a workflow. In order to glue the functions together we use queues and storages or databases. So Function 1 is executed and stores its results in a table. Function 2 is triggered by an entry in the table via the corresponding bindings and gets executed representing the second step in the workflow. This story continued ten for Function 3. The good news is, that this pattern adheres to the best practices. But this pattern comes with several downsides namely:

* The single functions are just coupled via the event that they react to. From the outside it is not clear how the functions relate to each other although the represent a sequence of steps in a workflow.
* The storage or queues in between single function executions are a necessary evil. One motivation for developing serverless is to care about servers less. Here we must care about technical infrastructure in order to cover the requirement.
* If you want to pass a context between the functions you must store it (and deal with the potential errors around it).
* Handling errors and analyzing bugs in such a setup is very complicated.

Can we do better? Or is there even a solution provided by Azure Functions to handle such scenarios? There is good news - there are Azure Durable Functions.

### 1.2 Solution via Durable Functions

Azure Durable Functions are an extension to the Azure Functions Framework that support you with modeling workflows via Azure Functions. The extension supports you with dealing with all the tedious tasks mentioned above i. e. it does the heavy lifting for you, so that you can focus on the business requirement at hand. The local state is preserved by making use of [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html) and the chaining of functions is done in a programmatic way without you having to deal with state and its persistency. In addition the extension helps you with common tasks in WOrkflows like retries and race conditions as we will see later. Let us first take a look at how Durable Functions work and introduce some terminology.

### 1.3 Mechanics of Durable Functions

Durable Functions consist of three components:

* Orchestrator Functions: the central part of the Durable framework that orchestrates the actions that should take place by triggering Activity Functions.
* Activity Functions: the basic workers that execute the single tasks scheduled via the Orchestrator Function.
* Client Function: the gateway to the Orchestrator Function. The Client Function triggers the Orchestrator Function and serves as the single point of entry for requests from the caller like getting the status of the processing, terminating the processing etc.

Let us assume the following simple execution sequence with two tasks triggered by an HTTP request:

```typescript
let x = await ctx.CallActivityAsync("F1")
let y = await ctx.CallActivityAsync("F2", x)
```

The second task depends on the result of the first task.

The schematic setup with Azure Durable Functions looks like this:

![Durable Function Execution Schema](../../img/lessons/durablefunctions/chaining/SchemaDurableFunction0.png)

The Client Function is triggered by an HTTP request and consequently triggers the Orchestrator function. Internally this means that a message is enqueued in a task hub. We do not have to care about that as we will se later.

![Durable Function Execution Trigger](../../img/lessons/durablefunctions/chaining/SchemaDurableFunction1.png)

After that the Client Functions scales back down to zero and the Orchestrator Function takes over. It fetches the task from the task hub and schedules the Activity Function.

![Durable Function Execution Orchestrator](../../img/lessons/durablefunctions/chaining/SchemaDurableFunction2.png)

Again the orchestrator scales down to zero and the Activity Function is executed and returns its results.

![Durable Function Execution Activity](../../img/lessons/durablefunctions/chaining/SchemaDurableFunction3.png)

After the execution of the first Activity Function the Orchestrator function is invoked again and checks if there are tasks left to do. In our scenario the second Activity Functions is executed. This cycle continuos until all Activity Function calls implemented in the Orchestrator are executed.

After this more theoretical discussion Let us make our hands dirty with some code. But before let us sort our some prerequisites.

## 2. Creating a Function App project for a Durable Function

Ou scenario comprises a Durable Function App with two Activity Functions. The app will be triggered via an HTTP call.
From a business perspective we get an purchase order item number handed over via the HTTP call. Based on the purchase order number we determine ina first step the material group of the purchase order item and in the consequent call we fetch the corresponding description in English.

### 2.1 The Client Function

The first function that we create is the Client Function of our Durable Function app that represents the gateway towards the Orchestrator Function.

#### Steps

1. Create a directory for our function app and navigate into the directory.

   ```powershell
   mkdir DurableFunctionApp
   cd DurableFunctionApp
   ```

2. Start Visual Studio Code.

   ```powershell
   code .
   ```

3. Create a new project via the Azure Functions Extension.
   1. Name the project `DurableFunctionApp`.
   2. Choose `TypeScript` as language.
   3. Select `Durable Functions HTTP Starter` as a template as we want to trigger the execution of the Durable Function via an HTTP call.
   4. Name the function `DurableFunctionStarter`.
   5. Set the authorization level of the function to `Anonymous`.

   > 🔎 **Observation** - Take a look into the `package.json` file. You find already a lot of predefined scripts to build and run the Durable Function.

   ![package.json with predefined scripts](../../img/lessons/durablefunctions/chaining/PackageJsonScripts.png)

   > 🔎 **Observation** - Take a look into the `function.json` file that was created. Besides the common HTTP bindings we see a special binding called `starter`  of type `orchestrationClient`. This is a special binding that enables the function to kick off an Orchestrator Function.

   ![Binding for Orchestrator starter](../../img/lessons/durablefunctions/chaining/ClienFunctionConfig.png)

   > 🔎 **Observation** - In addition we find a routing configuration in the `function.json`. This means that the orchestration that is triggered is identified via the name of the Orchestrator Function.

   ![Binding for Orchestrator starter](../../img/lessons/durablefunctions/chaining/ClienFunctionConfig2.png)

   > ❔ **Question** - Why is there an error in the `index.ts` file of the function?

4. Install the `durable-functions` via npm `npm install durable-functions`.

> 📝 **Tip** - Install the the `@types/node` npm package as a dev-dependency because this is needed for the build process.

### 2.2 The Orchestrator Function

Now we create the Orchestrator Function responsible for the orchestration of the single Activity Functions.

#### Steps

1. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions orchestrator` as a template.
   2. Name the function `DurableFunctionsOrchestrator`.

   > 📝 **Tip** - Remove the comments from the index.ts file.

   > 🔎 **Observation** - Take a look into the `function.json` file of the Orchestrator Function. You find the binding type `orchestrationTrigger` which classifies the function as an Orchestrator Function.

   > ❔ **Question** - Can you derive how the Orchestrator Function triggers the Activity Functions? Do you see potential issues in the way the functions are called?

### 2.3 The Activity Function

To complete the Durable Function setup we create an Activity Function.
#### Steps

1. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the function `HelloCity`.

   > 📝 **Tip** - Remove the comments from the index.ts file.

   > 🔎 **Observation** - Take a look into the `function.json` file of the Activity Function. You find the binding type `activityTrigger` which classifies the function as an Activity Function.

   > ❔ **Question** - If you trigger the orchestration now, would you run into an error? What adoption do you have to make (hint: function name in orchestrator)?

### 2.4 The First Execution

Execute the Durable Function and experience its mechanics.

#### Steps

1. Start the Azure Storage Emulator.
2. Set the value of the `AzureWebJobsStorage` in your `local.settings.json` to `UseDevelopmentStorage=true`. This advices the function worker to use your local Azure Storage Emulator.
3. Start the function via `npm run start`.
   > 🔎 **Observation** - You can see that the runtime is serving three functions.

   ![Durable Function Runtime](../../img/lessons/durablefunctions/chaining/DurableFunctionSimple.png)
4. Trigger the orchestration via the tool of your choice e.g. Postman.
   > ❔ **Question** - What route do you have to use?

   > 🔎 **Observation** - The result of the orchestration is not directly returned to the caller. Instead the Client Function is returning the ID and several HTTP endpoints to interact with the orchestration.

   ![Durable Function Response](../../img/lessons/durablefunctions/chaining/DurableFunctionSimpleResponse.png)

5. Call the `statusQueryGetUri` endpoint to receive the results of the orchestration.

   ![Durable Function Result](../../img/lessons/durablefunctions/chaining/DurableFunctionSimpleResult.png)
   > 🔎 **Observation** - The status endpoint returns not only the result but also some metadata with respect to the overall execution.

6. Check the resulting entries in your Azure Storage Emulator
   > ❔ **Question** - How many tables have been created by the Azure Functions runtime? What do they contain?

## 3. Implementing a "Real-World" Scenario

In this section we develop some more realistic setup for our durable function. Assume the following situation:
Assume that we have the name of a GitHub repository. Now we want to find out who is the owner of this repo. In addition we want to find our some more things about the owner like the real name, the bio etc. To achieve this we must execute two calls in a sequence to the GitHub API:

1. Get the information about the repository itself containing the user ID.
2. Based on the user ID we fetch the additional information of the user.

The good thing about the GitHub API is that we do not need to care about authentication and API keys. This means that there are some restrictions with respect to the allowed number of calls per minute, but that is fine for our scenario.
### 3.1 Basic Setup of "Real-World" Scenario

In this section we add the skeleton for the implementation.
#### Steps

1. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions orchestrator` as a template.
   2. Name the function `GitHubInfoOrchestrator`.
2. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the function `GetRepositoryDetailsByName`.
3. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the function `GetUserDetailsByID`.
4. Install the following npm modules for a smooth interaction with the GitHub REST API
   1. `@octokit/core`

### 3.2 Implementation of the Activity Function `GetRepositoryDetailsByName`

In this section we implement the Activity Function `GetRepositoryDetailsByName`. We fetch the data of the repository by name making use of the `/search/repositories` endpoint.

#### Steps

1. Import the oktokit core package.

   ```typescript
   import { Octokit } from "@octokit/core"
   ```

2. Create an instance of the Oktokit class and build the query for the search based on the repository name.

   ```typescript
   const octokit = new Octokit()
    
   const query = `${context.bindingData.repositoryName.toString()} in:name`
   ```

3. Call the GitHub API with the query.

   ```typescript
   const searchResult = await octokit.request('GET /search/repositories', {
        q: query
    })
   ```

4. As the search via the API is not an exact match and more than one repo might be returned we must find the search result where the name matches exactly. We implement this via the find method on the returned array and return the ID of the owner

   ```typescript
   const exactMatch = searchResult.data.items.find(item => item.name === context.bindingData.repositoryName.toString())

   return exactMatch.owner.login
   ```

The resulting function finally looks like this:

```typescript
import { AzureFunction, Context } from "@azure/functions"
import { Octokit } from "@octokit/core"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const octokit = new Octokit()

    const query = `${context.bindingData.repositoryName.toString()} in:name`

    const searchResult = await octokit.request('GET /search/repositories', {
        q: query
    })

    const exactMatch = searchResult.data.items.find(item => item.name === context.bindingData.repositoryName.toString())

    return exactMatch.owner.login
}

export default activityFunction
```

### 3.3 Implementation of the Orchestrator Function

In this section we implement the Orchestrator Function that defines the call sequence of the Activity Functions and assures the transfer of the result of the first Activity Function to the second one.
#### Steps

1. Adopt the Orchestrator Function `GitHubInfoOrchestrator` to the Activity Functions. The first Activity Function is `GetRepositoryDetailsByName`. The returned information of the user ID must be transferred to the context of the Durable Function. The second Activity Function is `GetUserDetailsById`. The returned information of the second Activity Function is returned as response by the Orchestrator Function. The code should finally look like this:

   ```typescript
   import * as df from "durable-functions"

   const orchestrator = df.orchestrator(function* (context) {

   const userId:string = yield context.df.callActivity("GetRepositoryDetailsByName", context.bindingData.input)
    
   context.bindingData.input.userId = userId

   const userInfos = yield context.df.callActivity("GetUserDetailsById", context.bindingData.input)

   return userInfos
   })

   export default orchestrator

   ```

### 3.4 Implementation of the Activity Function `GetUserDetailsById`

In this section we implement the Activity Function `GetUserDetailsById` that fetches the details about the user from GitHub.

#### Steps

1. Import the oktokit core package.

   ```typescript
   import { Octokit } from "@octokit/core"
   ```

2. Create an instance of the Oktokit class and build the path for the request based on the user ID.

   ```typescript
   const octokit = new Octokit()
    
   const apiPath = `/users/${context.bindingData.userId.toString()}`
   ```

3. Call the GitHub API.

   ```typescript
   const searchResult = await octokit.request( apiPath )
   ```

4. As we only want the user data and no further information we restrict the result to this data and transform it into JSON. Be aware that you also nee to change the Promise type of the function to JSON.

   ```typescript
   const userData = <JSON><any> searchResult.data
    
   return userData
   ```

The resulting function finally looks like this:

```typescript
import { AzureFunction, Context } from "@azure/functions"
import { Octokit } from "@octokit/core"

const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {
 
    const octokit = new Octokit()

    const apiPath = `/users/${context.bindingData.userId.toString()}`
    
    const searchResult = await octokit.request( apiPath )
    
    const userData = <JSON><any> searchResult.data
    
    return userData
    
}

export default activityFunction
```

> ❔ **Question** - How many functions will be started by the runtime now?  

### 3.5 Test the Implementation

In this section we finally test our implementation.

#### Steps

1. Execute the Durable orchestration and fetch the user data.
   
   > 📝 **Tip** - You find some sample data in the `demoRequests.http` file in the directory /src/DurableFunctions/chaining/ts

   > ❔ **Question** - How can you address the new Orchestrator Function in the function app  

## 4. Retries - Dealing with Temporal Errors

As we are dealing with external systems the question is not if something will go wrong, but when this will be the case. So in this section we want to harden our setup to deal with temporal outages of the downstream system using retries when calling activities.
Azure Durable functions have the built-in capability to execute an [automatic retry](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-error-handling?tabs=javascript#automatic-retry-on-failure) in case an Activity Function fails. The retry mechanism can be configured using a so called retry policy. In this section we will adopt our `GitHubInfoOrchestrator` to call the Activity Functions with retries.

### Steps

1. Commit your changes and create a branch of the project and switch to the branch.

   ```powershell
   git add .
   git commit -m "Basic DF setup"
   git branch retry
   git checkout retry
   ```

2. Open the `index.ts` file of the `GitHubInfoOrchestrator` function.
3. Define the following parameters for the retry policy before the call of the Activity Functions.
  
   3.1 Set the amount of time to wait before the first retry attempt to 1000 ms.

   ```typescript
   const firstRetryIntervalInMilliseconds: number = 1000
   ```

   3.2. set the maximum number of attempts to 3.

   ```typescript
   const maxNumberOfAttempts: number = 3
   ```

   3.3 Set the maximum amount of time to wait in between retry attempts to 1000 ms.

   ```typescript
   const maxRetryIntervalInMilliseconds: number = 1000
   ```
  
   3.4 Set the maximum amount of time to spend doing retries to 7000 ms.

   ```typescript
   const retryTimeoutInMilliseconds:number = 7000
   ```

   > 📝 **Tip** - It is a best practice to inject the value via environment variables. When doing so make sure that the variable type is converted into a number.
  
   > ❔ **Question** - How long would the Orchestrator Function retry failed calls by default?

4. Create a instance of the retry options class and set the configuration values.

    ```typescript
    const retryConfig: df.RetryOptions = new df.RetryOptions(firstRetryIntervalInMilliseconds, maxNumberOfAttempts)
    retryConfig.maxRetryIntervalInMilliseconds = maxRetryIntervalInMilliseconds
    retryConfig.retryTimeoutInMilliseconds = retryTimeoutInMilliseconds
    ```

   > 🔎 **Observation** - The constructor of the retry options calls only expects a minimum configuration.

5. Adopt the calls of the Activity Functions to take into account retries and consider the retry configuration.

   ```typescript
   const userId: string = yield context.df.callActivityWithRetry("GetRepositoryDetailsByName", retryConfig, context.bindingData.input)
   ...
   const userInfos = yield context.df.callActivityWithRetry("GetUserDetailsById", retryConfig, context.bindingData.input)
   ```

6. In order to enforce an error we will cheat a bit. When we call the function we will hand over a parameter called `raiseException` that will be evaluated in the activity function `GetRepositoryDetailsByName`. In case that teh parameter is set to true we will throw an error. Change the logic of the activity function accordingly
   
   ```typescript
   ...
   const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    if (context.bindingData.raiseException && context.bindingData.raiseException === true) {
        context.log("Error was enforced by caller")
        throw {
            name: "ForcedException",
            message: "Caller enforced exception",
            toString: function () {
                return this.name + ": " + this.message;
            }
        }
    }
   ...
   ```

6. Start the durable function and make a call with correct input parameters to make sure we did not brake anything.

   > 🔎 **Observation** - The function is executed as before. There is also no difference in the data stored in the storage emulator with respect to the previous execution.

7. Start the durable function and make a call which sets the .

   > ❔ **Question** - What do you get as result from the status URL?

   > ❔ **Question** - Take a look into the storage explorer for the call. What mechanics is used behind the scenes to execute the retries?

8. Let us do another demo trick to simulate that the error vanishes. Stop your function and add the following code into the Orchestrator Function before the activities are called:

   ```typescript
   // For demo purposes
    if (context.df.isReplaying == true) {
        context.bindingData.input.raiseException = false
    }
   ```

   > 📝 **Tip** < TIP > - We make use of the replay mechanics of the durable function that is stored in the context. As soon as the execution is retried the input parameter gets corrected.

9. Start the function and make a call enforcing the error.

   > ❔ **Question** - What do you get as result from the status URL? Is there any hint that something went wrong?

   > ❔ **Question** - Take a look into the storage explorer for the call. What is the difference to erroneous call that we did before?

## 5. Circuit Breaker - Dealing with Timeouts

In this section we want to become even more resilient with respect to the called system. In this section we want to deal with the scenario that the system will not return any response in a meaningful time. As a consequence we want to abort the orchestration if a certain time threshold es exceeded. To achieve this we introduce a time out.

We have already seen in the retry scenario that a timer is used internally for dealing with retries, so we now make use of this functionality explicitly. To achieve this we use the [Function timeout](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-error-handling?tabs=javascript#function-timeouts).
### Steps

1. Create a new branch for this development task.

   ```powershell
   git add .
   git commit -m "sample retry scenario"
   git checkout main
   git branch timeout
   git checkout timeout
   ```

2. Install the `moment` package to handle the time settings.

   ```powershell
   npm install moment
   ```

3. Set the  value of `"esModuleInterop"` to `true` in the `tsconfig.json` file. Otherwise the build of the project will fail

4. Open the orchestrator function and import the moment package.

   ```typescript
   import moment from "moment"
   ```

5. Create a deadline for the timer in the Orchestrator Function. We will wait 3000 milliseconds until we assume that the call failed.

   ```typescript
   const timeoutInMilliseconds:number = 3000
   const deadline = moment.utc(context.df.currentUtcDateTime).add(timeoutInMilliseconds, "ms")
   ```

6. Create a timer task via the Durable Functions context but do not yield it.

   ```typescript
   const timeoutTask = context.df.createTimer(deadline.toDate())
   ```

   > 📝 **Tip** - Up to now we directly yielded all calls of the Activity Function. In this scenario we build up the activities and then let the durable function runtime execute them in parallel

7. Rewrite the call of the `GetRepositoryDetailsByName` Activity Function to become a task.

   ```typescript
   const repositoryDetailsTask = context.df.callActivity("GetRepositoryDetailsByName", context.bindingData.input)
   ```

7. Instruct the durable function runtime to let the two tasks (Activity Function and timer) race against each other. The durable function runtime will return the task that finishes first as the `winner`.

   ```typescript
   const winner = yield context.df.Task.any([repositoryDetailsTask, timeoutTask])
   ```

8. Implement the handling of the `winner` task.

   ```typescript
   if (winner === repositoryDetailsTask) {

        context.log("Repository Information fetched before timeout")

        timeoutTask.cancel();

        const userId = repositoryDetailsTask.result
        context.bindingData.input.userId = userId
    }
   else {
        context.log("Repository Information call timed out ...")
        throw new Error("Repository Information call timed out")
    }
   ```

   > 📝 **Tip** - The durable function runtime will not cancel any task but keep them running. Make sure that the loser task is canceled.

9. Start the durable function and make a call with correct input parameters to make sure we did not brake anything.

   > ❔ **Question** - Check the execution in the storage explorer. What is different with respect to the original execution without the timer?

10. Let us do a little demo trick to simulate a timeout. Add the following code pieces to the `GetRepositoryDetailsByName` function.

      10.1 Add an asynchronous function to create a timeout.

      ```typescript
      async function sleep(ms: number) {
      return new Promise(resolve => setTimeout(resolve, ms))
      }
      ```

     10.2 Call the `sleep` function before the call to the GitHub API is executed and trigger a delay of 10 seconds.

     ```typescript
     // For demo of timeout
     await sleep(10000)
     ```

11. Start the durable function and make a call with correct input parameters.

     > ❔ **Question** - What do you see in the execution history stored in the Azure Storage Emulator?

## 6. Homework

[Here](../../homework/durablefunctions/chaining/durable_locationsearch_api_ts.md) is the assignment for this lesson.

In addition we also have an additional homework that deals with a more advanced scenario i. e. making use of the SAP Cloud SDK to call a downstream SAP system. You find the instructions [here](../../homework/durablefunctions/chaining/durable_sapapi_ts.md).

## 7. More info

* Azure Durable Functions - [Official Documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/)
* JavaScript: [Generator functions](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/function*) and [Yield](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/yield)
* Azure Durable Functions - [Automatic retries](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-error-handling?tabs=javascript#automatic-retry-on-failure)
* Azure Durable Functions - [Function timeouts](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-error-handling?tabs=javascript#function-timeouts)
* More info on the [circuit breaker pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker)
* [GitHub REST API](https://docs.github.com/en/free-pro-team@latest/rest)
* Alternative to code-based workflows in Microsoft Azure: [Azure Logic Apps](https://azure.microsoft.com/en-us/services/logic-apps/)

---
[◀ Previous lesson](<previous>.md) | [🔼 Index](_index.md) | [Next lesson ▶](<next>.md)