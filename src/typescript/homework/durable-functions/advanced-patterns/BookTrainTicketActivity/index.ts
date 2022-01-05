import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const message = `Train ticket was booked for ${context.bindings.input.name} on travel date ${context.bindings.input.startdate}`

    context.log(message)

    return message

}

export default activityFunction
