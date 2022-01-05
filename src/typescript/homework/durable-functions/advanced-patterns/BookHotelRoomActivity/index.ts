import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    const message = `Hotel room for ${context.bindings.input.name} for arrival date ${context.bindings.input.startdate} was booked in hotel ${context.bindings.input.hotel}`
    
    context.log(message)

    return message
}

export default activityFunction;
