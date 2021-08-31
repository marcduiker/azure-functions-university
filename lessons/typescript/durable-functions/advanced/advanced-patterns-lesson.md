# Azure Durable Functions - Advanced Patterns (TypeScript)

Watch the recording of this lesson [on YouTube 🎥]().

## Goal 🎯

The goal of this lesson is to to dive deeper into the area of Azure Durable Functions. In this lesson we discuss some more advanced patterns for the modeling of workflows with Durable Functions.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Business Scenario]()
|2|[Fan-Out/Fan-In Scenario]()
|3|[Sub-Orchestration]()
|4|[External Events - Human Interaction]()
|5|[Homework](#4-homework)
|6|[More info](#5-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../src/typescript/durable-functions/advanced-patterns) in this repository.

---

## 0. Prerequisites

| Prerequisite                          | Exercise
| -                                     | -
| Azurite (Storage Emulator)            | 2-4
| Azure Storage Explorer                | 2-4
| An empty local folder / git repo      | 2-4
| Azure Functions Core Tools            | 2-4
| VS Code with Azure Functions extension| 2-4
| Rest Client for VS Code or Postman    | 2-4

See [TypeScript prerequisites](../prerequisites/prerequisites-ts.md) for more details.

We assume that you have already made your way through the [first lesson](../chaining/chaining-lesson-ts.md) on Azure Durable Functions to have an understanding of the basics. If not we highly recommend to do so before starting with this lesson. 

## 1. Business Scenario

We will focus on one _business scenario_ throughout this lesson namely employee onboarding. So the storyline is to automate the workflow that lies behind the onboarding of a new employee to a company. Let us assume that whenever a new employee is onboarded the following steps need to be executed in different backend systems of the company:

- trigger the creation of an access card to the company building
- start the purchasing process for the IT equipment of the employee  
- send out an email with a welcoming text to the new employee and some guidance how the start day will look like.

As we already know we can model a sequence of these steps making use of Azure Durable Functions, so it would be possible to execute one step after the other. But these steps to not have any dependency on each other, so there is no need to wait for the outcome of the access card creation to start the purchasing process. At least from a business perspective we could kick off all activities in parallel and then wait until all activities have finished to successfully close this part of the onboarding process. Let's find out how to do this with Azure Durable Functions.

> 🔎 **Observation** - The goal of this lesson is to learn about patterns that can be applied using Azure Durable Functions. We will therefore "simulate" the onboarding steps and not call any other external systems as we would have to in reality. We have shown this including patterns for resilience of the calls to external systems in the  [first lesson](../chaining/chaining-lesson-ts.md) on Azure Durable Functions.

## 2. Fan-Out/Fan-In Scenario

In this section we want to explore how to execute the onboarding activities in parallel using Azure Durable Functions. This pattern is called _Fan-Out/Fan-In_ as we first fan-out the execution of several activities and then wait until all activities have finished to fan-in again and continue with the process.

We design the solution as follows:

- as an entry point for the process we make use a `Durable Functions HTTP Starter` function that receives a `POST` request containing the data of the new employee (name, email, start date, role).
- the orchestration of the onboarding activities is done via a  `Durable Functions orchestrator`.
- the single steps of the onboarding are defined in `Durable Functions activities`.

We achieve the parallel execution of the tasks via the `Tasks.all` method available on the Durable Functions context object. This method expects an array of activity calls as input.

### Basic Setup

#### Steps

1. Create a directory for our function app and navigate into the directory.

   ```powershell
   mkdir OnbordingByDurableFunction
   cd OnbordingByDurableFunction
   ```

2. Start Visual Studio Code.

   ```powershell
   code .
   ```

3. Create a new project via the Azure Functions Extension.
   1. Name the project `OnbordingByDurableFunction`.
   2. Choose `TypeScript` as language.
   3. Select `Durable Functions HTTP Starter` as a template as we want to trigger the execution of the Durable Function via an HTTP call.
   4. Name the function `OnboardingStarter`.
   5. Set the authorization level of the function to `Anonymous`.

4. Install the `durable-functions` via npm `npm install durable-functions`.

   > 📝 **Tip** - Install the the `@types/node` npm package as a dev-dependency because this is needed for the build process.

5. Adjust the `function.json` file of the HTTP starter to expose only an `POST` endpoint.

   ```json
    "methods": [
      "post"
    ]
   ```

6. Create a new Orchestrator Function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions orchestrator` as a template.
   2. Name the function `OnboardingOrchestrator`.

   > 📝 **Tip** - We will adjust the code of the orchestrator after we have created the Activity Functions that we want to execute.

7. Create three new Activity Functions via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the functions `AccessCardCreationActivity`, `ItEquipmentOrderActivity`, `WelcomeEmailActivity`.

8. Set the `AzureWebJobsStorage` parameter in the `local.settings.json` to `UseDevelopmentStorage=true`

   ```json
   {
    "IsEncrypted": false,
    "Values": {
       "AzureWebJobsStorage": "UseDevelopmentStorage=true",
       "FUNCTIONS_WORKER_RUNTIME": "node"
     }
   }
   ```

### Implementation of the Orchestrator Function

As the basic setup is in place we will now implement the fan-out/fan-in logic in the Orchestrator Function.  

#### Steps

1. Open the `index.ts` file of the Orchestrator Function and remove the comments.
2. Remove the content of the function, the function should now look like this:

   ```typescript
   import * as df from "durable-functions"

   const orchestrator = df.orchestrator(function* (context) {

   })

   export default orchestrator
   ```

3. Define an empty array called `onboardingTasks` that will be filled with the activity functions that should be executed in parallel
  
   ```typescript
   const onboardingTasks = [] 
   ```

4. Push the onboarding Activity Functions that we defined before into the array. Hand over the input available via the `context` object to the Activity Function.

   ```typescript
   onboardingTasks.push(context.df.callActivity("AccessCardCreationActivity", context.bindingData.input))
   onboardingTasks.push(context.df.callActivity("ItEquipmentOrderActivity", context.bindingData.input))
   onboardingTasks.push(context.df.callActivity("WelcomeEmailActivity", context.bindingData.input))  
   ```

   > 🔎 **Observation** - Comparing this call of activities with the one of the chaining we see that the call itself remains the same, but we omit the key word `yield` and therefore do not trigger the generator functionality used in the chaining scenario. This enables the parallel execution. 

   > 📝 **Tip** - You can also make use of the retry functionality available on the `context.df` object to call the Activity Function.

5. Start the parallel execution via `Task.all` of the activities, fetch the result data and return it.

   ```typescript

    const result = yield context.df.Task.all(onboardingTasks)

    return result

   ```

   > 🔎 **Observation** - In contrast to the single activity calls that we did not `yield`, we yielded the task execution. We get back a task that won't complete until all activities have completed. This is the same concept as `Promise.all` in JavaScript. The difference is that the tasks could be running on multiple virtual machines concurrently, and the Durable Functions extension ensures that the end-to-end execution is resilient to process recycling.

### Implementation of the Activity Functions



#### Steps

1. function.json - change the name of the parameter

## 3. Sub-Orchestration

//TODO Describe sub goal
### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >

## 4. External Event - Human Interaction

//TODO Describe sub goal

### Steps

1.
2.
3.

> 📝 **Tip** - < TIP >

> 🔎 **Observation** - < OBSERVATION >

> ❔ **Question** - < QUESTION >

## 5. Homework

## 6. More info

---
[🔼 Lessons Index](../../README.md)
