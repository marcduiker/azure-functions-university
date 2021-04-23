import * as df from "durable-functions"
import { MaterialGroupData, ProductGroupInfo } from "../utils/purchaseOrderTypes"

const orchestrator = df.orchestrator(function* (context) {

    /*
    // For demo purposes
    if (context.df.isReplaying == true) {
        context.bindingData.input.purchaseOrderItemId = 30
    }
    */

    const firstRetryIntervalInMilliseconds: number = 1000
    const maxNumberOfAttempts: number = 3
    const maxRetryIntervalInMilliseconds: number = 1000
    const retryTimeoutInMilliseconds: number = 7000

    const retryConfig: df.RetryOptions = new df.RetryOptions(firstRetryIntervalInMilliseconds, maxNumberOfAttempts)
    retryConfig.maxRetryIntervalInMilliseconds = maxRetryIntervalInMilliseconds
    retryConfig.retryTimeoutInMilliseconds = retryTimeoutInMilliseconds

    const materialGroup: MaterialGroupData = yield context.df.callActivityWithRetry("GetPoDetails", retryConfig, context.bindingData.input)

    context.bindingData.input.materialGroupId = materialGroup.materialGroupId

    const productGroupInfo: ProductGroupInfo = yield context.df.callActivityWithRetry("GetPgText", retryConfig, context.bindingData.input)

    return productGroupInfo

});

export default orchestrator;