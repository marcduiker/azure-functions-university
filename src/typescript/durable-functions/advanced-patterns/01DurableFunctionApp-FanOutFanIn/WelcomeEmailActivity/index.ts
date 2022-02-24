
import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const message = `Welcome ${context.bindings.input.name}! Happy to have you on board and see you on ${context.bindings.input.startdate}!`

    context.log(message)

    return message

}

export default activityFunction
