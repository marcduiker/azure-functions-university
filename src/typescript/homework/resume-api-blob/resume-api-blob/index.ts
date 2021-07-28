import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    let responseStatusCode: number
    let responseMessage: string
    let responseHeaders: object

    if (context.bindings.ResumeBlobIn) {

        responseStatusCode = 200
        responseHeaders = { "Content-Type": "application/json" }
        responseMessage = context.bindings.ResumeBlobIn

    }
    else {

        responseStatusCode = 404
        responseHeaders = { "Content-Type": "text/plain" }
        responseMessage = `No result found`

    }

    context.res = {
        status: responseStatusCode,
        headers: responseHeaders,
        body: responseMessage
    }

}

export default httpTrigger;