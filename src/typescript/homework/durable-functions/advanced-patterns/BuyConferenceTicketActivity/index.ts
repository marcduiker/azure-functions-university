import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const message = `Conference ticket for ${context.bindings.input.name} was bought`
    
    context.log(message)

    return message



}

export default activityFunction
