import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    const person: Person = req.body

    let responseMessage: string
    let responseStatus: number

    if (person && person.name) {
        responseMessage = `Hello, ${person.name}. This HTTP triggered function executed successfully.`
        responseStatus = 200
    }
    else {
        responseMessage = `Pass a name in the request's JSON body with the attribute "name" (POST) for a personalized response.`
        responseStatus = 400
    }

    context.res = {
        status: responseStatus,
        body: responseMessage
    }

}

export default httpTrigger

interface Person {
    name: string
}