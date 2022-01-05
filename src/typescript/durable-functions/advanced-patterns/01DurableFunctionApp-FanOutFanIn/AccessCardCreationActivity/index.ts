import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    context.log(`Access card created for  ${context.bindings.input.name} starting on ${context.bindings.input.startdate}`)

    return `Access card was created for ${context.bindings.input.name}.`

}

export default activityFunction
