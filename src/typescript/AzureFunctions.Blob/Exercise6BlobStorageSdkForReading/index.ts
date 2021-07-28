/*
Azure Function for Exercise 6: GetPlayerFromBlob
*/

import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { BlobServiceClient } from "@azure/storage-blob"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    let responseStatusCode: number
    let responseMessage: string
    let responseHeaders: object


    if (req.params.id) {

        if (context.bindings.playerBlobIn) {

            responseStatusCode = 200
            responseHeaders = { "Content-Type": "application/json" }
            responseMessage = context.bindings.playerBlobIn

        }
        else {

            responseStatusCode = 404
            responseHeaders = { "Content-Type": "text/plain" }
            responseMessage = `No result found`

        }

    }
    else {

        const connectionString = "<YOUR CONNECTION STRING TO LOCAL STORAGE>"
        const containerName = "players"

        let blobData: string[] = new Array()

        const blobServiceClient = BlobServiceClient.fromConnectionString(connectionString)
        const containerClient = blobServiceClient.getContainerClient(containerName)

        let i = 0
        for await (const blob of containerClient.listBlobsFlat({ prefix: "in/" })) {

            let entry = `Blob ${i++}: ${blob.name}`
            blobData.push(entry)

        }

        responseStatusCode = 200
        responseHeaders = { "Content-Type": "application/json" }
        responseMessage = <any>{ "dataFromBlob": blobData }

    }

    context.res = {
        status: responseStatusCode,
        headers: responseHeaders,
        body: responseMessage
    }

}

export default httpTrigger