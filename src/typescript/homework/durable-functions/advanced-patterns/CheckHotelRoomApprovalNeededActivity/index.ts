import { AzureFunction, Context } from "@azure/functions"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    if (context.bindings.input.hotel === "Adlon Berlin") {

        return `approval needed`

    }
    else {

        return `approved`

    }

}

export default activityFunction
