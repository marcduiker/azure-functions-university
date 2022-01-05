
import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {
    
    context.log(`Welcome ${context.bindings.input.name}! Happy to have you on board and see you on ${context.bindings.input.startdate}!`)
    
    return `Welcome email was sent to ${context.bindings.input.email}.`

}

export default activityFunction
