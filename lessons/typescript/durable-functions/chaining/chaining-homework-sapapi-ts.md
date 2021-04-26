# Durable Functions - Chaining Lesson Advanced Homework: Call SAP APIs in a Sequence (TypeScript)

## Goal 🎯

The goal of this homework assignment is the development of a Durable Function setup to interact with an SAP backend system.

Assume the following scenario:
Our function gets triggered via a purchase order create event that gives us an identifier for the order item. We want to identify the material group texts used in the order item. To do so we must call a down stream system. We cannot retrieve the text in one call but we first make a call to get the material group ID and with this result issue a second call in order to retrieve the desired texts. This represents a perfect scenario for a sequence of function calls.

For this example we make use of a sandbox SAP system and the OData APIs available from there. To make the calls to the system a bit more convenient we make use of the [SAP Cloud SDK](https://community.sap.com/topics/cloud-sdk) that wraps the business objects in a virtual data model (VDM) and provides a fluent API to interact with the business object.

The exercise i.e. the Activity Functions interact with an external API. This API is free to use and mimics an [SAP system](https://api.sap.com/package/SAPS4HANACloud?section=Artifacts). The only prerequisite is to register in order to get an API key. To get the free API keys to the sandbox system you must register yourself at the [API Business Hub](https://api.sap.com/) using the Log On button.

## Assignment

Create an HTTP-triggered Durable Function consisting of two activities that need to be executed in sequence. The input parameters are:

* the purchase order ID
* the purchase order item ID
* the language that we want our texts

The first Activity Function calls the S/4HANA Cloud system OData Service [Purchase Order](https://api.sap.com/api/API_PURCHASEORDER_PROCESS_SRV/resource) in order to get the material group ID from the purchase order item.

The second Activity Function uses the results of the first activity to call the S/4HANA Cloud system OData Service [Product Group - Read](https://api.sap.com/api/API_PRODUCTGROUP_SRV/resource) in order to read the product group texts.

In addition we want to make our setup as resilient as possible. So it is a good idea to implement a retry policy as well as a mechanism to deal with timeouts of the calls to the SAP system.

## Resources

* Solution can be found in the folder [`/src/typescript/homework/durable-functions/chaining/sapapi`](../../../../src/typescript/homework/durable-functions/chaining/sapapi), try to accomplish it on your own first.
* Make sure to update your local.settings.json to use development storage and to have storage emulator running.
* Store your API key in the local.settings.json.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Marc](https://twitter.com/marcduiker) and [Christian](https://twitter.com/lechnerc77)

---

## Addendum

As this scenario is a more advanced one, we also want to give you a step-by-step guide to get along with the scenario in the following sections.

### A1.1 Basic Setup

In this section we add the skeleton for the implementation.
#### Steps

1. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions orchestrator` as a template.
   2. Name the function `PurchaseInfoOrchestrator`.
2. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the function `GetPoDetails`.
3. Create a new function via the Azure Functions Extension in VSCode.
   1. Select `Durable Functions activity` as a template.
   2. Name the function `GetPgText`.
4. Install the following npm modules for interacting with the SAP sandbox system:
   1. `@sap/cloud-sdk-core` - this package provides you the basic infrastructure to interact with the SAP system.
   2. `@sap/cloud-sdk-vdm-purchase-order-service` - this package provides you the VDM for the purchase orders.
   3. `@sap/cloud-sdk-vdm-product-group-service` - this package provides you the VDM for the product group.
5. Provide values for the URL and the API key of the sandbox system via environment variables. To do so add the following values to your `local.settings.json`. We will inject those values as environment variables in our Activity Functions.

   ```json
   ...
   "APIHubDestination": "https://sandbox.api.sap.com/s4hanacloud",
   "APIHubKey": "<Your Key Value>"
   ...
   ```

### A1.2 Implementation of the Activity Function `GetPoDetails`

In this section we implement the Activity Function `GetPoDetails` that determines the detailed information based on the purchase order ID and the purchase order item ID via an OData call to the SAP system.

#### Steps

1. Import the package for the virtual data model of the purchase order service.

   ```typescript
   import { PurchaseOrderItem } from "@sap/cloud-sdk-vdm-purchase-order-service"
   ```

2. Create a asynchronous function that retrieves the purchase order item information using the fluent API of the SAP Cloud SDK.

   ```typescript
   async function getPurchaseOrderItemDetails(
    { purchaseOrderId,
        purchaseOrderItemId
    }:
        {
            purchaseOrderId: string,
            purchaseOrderItemId: string
        }
    ): Promise<PurchaseOrderItem> {
    return PurchaseOrderItem.requestBuilder()
        .getByKey(purchaseOrderId, purchaseOrderItemId)
        .withCustomHeaders({ APIKey: process.env["APIHubKey"] })
        .execute({
            url: process.env["APIHubDestination"]
        })
    }
   export default activityFunction
   ```

3. Call the function in the body of the Activity Function. Transfer the field `materialGroup` of the function call to a JSON object and return it to the Orchestrator Function.

   ```typescript
   const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {
    try {
        let purchaseOrderItemDetails = await getPurchaseOrderItemDetails({ purchaseOrderId: context.bindingData.purchaseOrderId.toString(), purchaseOrderItemId: context.bindingData.purchaseOrderItemId.toString() })

        const result: JSON = <JSON><any>{ "materialGroupId": purchaseOrderItemDetails.materialGroup }

        return result

    }
    catch (error) {
        context.log("Error in OData call happened: ", error)
        throw error
    }

   }
   ```

### A1.3 Implementation of the Activity Function `GetPgText`

In this section we implement the Activity Function `GetPgText` that fetches the material group texts based on the material group ID and the input language via an OData call to the SAP system.

#### Steps

1. Import the package for the virtual data model of the purchase order service.

   ```typescript
   import { ProductGroupText } from "@sap/cloud-sdk-vdm-product-group-service"
   ```

2. Create a asynchronous function that retrieves the material group texts using the fluent API of the SAP Cloud SDK.

   ```typescript
   async function getProductGroupText(
    { materialGroupId,
        languageCode
    }:
        {
            materialGroupId: string,
            languageCode: string
        }
   ): Promise<ProductGroupText> {
    return ProductGroupText.requestBuilder()
        .getByKey(materialGroupId, languageCode)
        .withCustomHeaders({ APIKey: process.env["APIHubKey"] })
        .execute({
            url: process.env["APIHubDestination"]
        })
   }

   export default activityFunction
   ```

3. Call the function in the body of the Activity Function. Transfer the fields `materialGroup`, `language`, `materialGroupName` and `materialGroupText` of the function call to a JSON object and return it to the Orchestrator Function.

   ```typescript
   const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {

    try {
        let productGroupText = await getProductGroupText({ materialGroupId: context.bindingData.materialGroupId.toString(), languageCode: context.bindingData.languageCode.toString() })

        const result: JSON = <JSON><any>{
            "materialGroup": productGroupText.materialGroup,
            "language": productGroupText.language,
            "materialGroupName": productGroupText.materialGroupName,
            "materialGroupText": productGroupText.materialGroupText
        }

        return result

    }
    catch (error) {
        context.log("Error in OData call happened: ", error)
        throw error
    }

   }
   ```

### A1.4 Implementation of the Orchestrator Function

In this section we implement the Orchestrator Function that defines the call sequence of the two Activity Functions and assures the transfer of the result of the first Activity Function to the second one.
#### Steps

1. Create a new directory `util` on root level and create the file `purchaseOrderTypes.ts` in this directory. We will use this file to define interfaces that allow us a consistent handling of the JSON based results.
2. Create the following two interfaces:

   ```typescript
   export interface ProductGroupInfo {
    materialGroup: string,
    language: string,
    materialGroupName: string,
    materialGroupText: string
    }

   export interface MaterialGroupData {
    materialGroupId: string;
   }
   ```

3. Adopt the Orchestrator Function `PurchaseInfoOrchestrator` to the Activity Functions. The first Activity Function is `GetPoDetails`. The returned information of the material group ID must be transferred to the context of the Durable Function. The second Activity Function is `GetPgTexts`. The returned information is returned as response. The code should finally look like this:

   ```typescript
   import * as df from "durable-functions"
   import { MaterialGroupData, ProductGroupInfo } from "../utils/purchaseOrderTypes"

   const orchestrator = df.orchestrator(function* (context) {

    const materialGroup: MaterialGroupData = yield context.df.callActivity("GetPoDetails", context.bindingData.input)

    context.bindingData.input.materialGroupId = materialGroup.materialGroupId
    
    const productGroupInfo: ProductGroupInfo = yield context.df.callActivity("GetPgText", context.bindingData.input )

    return productGroupInfo

   })

   export default orchestrator
   ```

### A1.5 Test the Implementation

In this section we finally test our implementation.

#### Steps

1. Set the  value of `"esModuleInterop"` to `true` in the `tsconfig.json` file. Otherwise the build of the project will fail as some parts of the SAP Cloud SDK are not fully compliant to the default project settings i. e. the importing of modules within the files.
2. Execute the Durable orchestration and fetch the data.

   > 📝 **Tip** - You find some sample data in the `demoRequests.http` file


## A2 Retries - Dealing with Temporal Errors

Next we implement the retry functionality to harden our setup with respect to temporal errors occurring when the APIs are called by the Activity Functions.

### Steps

1. Commit your changes and create a branch of the project and switch to the branch.

   ```powershell
   git add .
   git commit -m "Basic setup - SAP homework"
   git branch retry
   git checkout retry
   ```

2. Open the `index.ts` file of the `PurchaseInfoOrchestrator` function.
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

4. Create a new instance of the retry options class and set the configuration values.

    ```typescript
    const retryConfig: df.RetryOptions = new df.RetryOptions(firstRetryIntervalInMilliseconds, maxNumberOfAttempts)
    retryConfig.maxRetryIntervalInMilliseconds = maxRetryIntervalInMilliseconds
    retryConfig.retryTimeoutInMilliseconds = retryTimeoutInMilliseconds
    ```

5. Adopt the calls of the Activity Functions to take into account retries and consider the retry configuration.

   ```typescript
   const materialGroup: MaterialGroupData = yield context.df.callActivityWithRetry("GetPoDetails", retryConfig, context.bindingData.input)
   ...
   const productGroupInfo: ProductGroupInfo = yield context.df.callActivityWithRetry("GetPgText", retryConfig, context.bindingData.input)
   ```

6. Start the durable function and make a call with correct input parameters to make sure we did not brake anything.

7. Start the durable function and make a call with wrong input parameters like setting the same value for both input parameters and check the result in the storage explorer.

8. Let us apply the demo trick from the lesson to simulate that the error vanishes. Stop your function and add the following code into the Orchestrator Function before the activities are called:

   ```typescript
   // For demo purposes
    if (context.df.isReplaying == true) {
        context.bindingData.input.purchaseOrderItemId = 30
    }
   ```

9. Start the function and make a call with the wrong value for the `purchaseOrderItemId`.

## A3 Circuit Breaker - Dealing with Timeouts

In this section we want to become even more resilient with respect to the called system. In this section we want to deal with the scenario that the system will not return any response in a meaningful time. As a consequence we want to abort the orchestration if a certain time threshold es exceeded. To achieve this we introduce a racing condition between an Activity Function and a timer.

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

3. Open the orchestrator function and import the moment package.

   ```typescript
   import moment from "moment"
   ```

4. Create a deadline for the timer. We will wait 3000 milliseconds until we assume that the call failed.

   ```typescript
   const timeoutInMilliseconds:number = 3000
   const deadline = moment.utc(context.df.currentUtcDateTime).add(timeoutInMilliseconds, "ms")
   ```

5. Create a timer task via the Durable Functions context but do not yield it.

   ```typescript
   const timeoutTask = context.df.createTimer(deadline.toDate())
   ```

6. Rewrite the call of the `GetPoDetails` Activity Function to become a task.

   ```typescript
   const materialGroupTask = context.df.callActivity("GetPoDetails", context.bindingData.input)
   ```

7. Instruct the durable function runtime to let the two tasks (Activity Function and timer) race against each other. The durable function runtime will return the task that finishes first as the `winner`.

   ```typescript
   const winner = yield context.df.Task.any([materialGroupTask, timeoutTask])
   ```

8. Implement the handling of the `winner` task.

   ```typescript
   if (winner === materialGroupTask) {

        context.log("Dunning level fetched before timeout")

        timeoutTask.cancel();

        let materialGroup: MaterialGroupData = <MaterialGroupData>materialGroupTask.result;
        context.bindingData.input.materialGroupId = materialGroup.materialGroupId
    }
   else {
        context.log("OData Call for PO details timed out ...")
        throw new Error("OData Call for PO details timed out")
    }
   ```

9. Start the durable function and make a call with correct input parameters to make sure we did not brake anything.

10. Let us do a little demo trick to simulate a timeout. Add the following code pieces to the `GetPoDetails` function.

      10.1 Add an asynchronous function to create a timeout.

      ```typescript
      async function sleep(ms: number) {
      return new Promise(resolve => setTimeout(resolve, ms))
      }
      ```

     10.2 Call the `sleep` function before the call to the SAP system is executed and trigger a delay of 10 seconds.

     ```typescript
     // For demo of timeout
     await sleep(10000)
     ```

11. Start the durable function and make a call with correct input parameters.
