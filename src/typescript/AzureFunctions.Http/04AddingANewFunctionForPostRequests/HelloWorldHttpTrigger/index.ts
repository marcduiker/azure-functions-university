import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    let name: string

    let responseMessage: string
    let responseStatus: number

    if (req.method === "GET") {
        name = req.query.name
    }
    else if (req.method === "POST") {
        name = (req.body && req.body.name)
    }

    if (name) {
        responseStatus = 200
        responseMessage = `Hello, ${name}. This HTTP triggered function executed successfully.`
    }
    else {
        responseStatus = 400
        responseMessage = `Pass a name in the query string (GET request) or a JSON body with the attribute "name" (POST request) for a personalized response.`
    }

    context.res = {
        status: responseStatus,
        body: responseMessage
    }

}

export default httpTrigger