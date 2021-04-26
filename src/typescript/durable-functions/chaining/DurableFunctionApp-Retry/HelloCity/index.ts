import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {
    return `Hello ${context.bindings.name}!`
}

export default activityFunction