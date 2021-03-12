import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    const name = req.query.name

    let responseMessage: string
    let responseStatus: number

    if (name) {
        responseStatus = 200
        responseMessage = `Hello, ${name}. This HTTP triggered function executed successfully.`
    }
    else {
        responseStatus = 400
        responseMessage = `Pass a name in the query string or in the request body for a personalized response.`
    }

    context.res = {
        status: responseStatus,
        body: responseMessage
    }

}

export default httpTrigger