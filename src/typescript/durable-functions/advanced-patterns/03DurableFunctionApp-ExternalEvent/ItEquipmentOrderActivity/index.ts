import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const message = `Role specific IT equipment (role: ${context.bindings.input.role}) was ordered for ${context.bindings.input.name} starting on ${context.bindings.input.startdate}`

    context.log(message)

    return message

}

export default activityFunction
