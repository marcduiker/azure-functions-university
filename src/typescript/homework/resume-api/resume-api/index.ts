import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processing resume request')

    const resumeData = new ResumeData("Gwyneth", "Pena-Siguenza", "CT, USA", "Cloud Consultant and YouTuber")

    context.res = {
        body: JSON.stringify(resumeData)
    }

}

export default httpTrigger

class ResumeData {

    firstName: string
    lastName: string
    location: string
    currentPosition: string

    constructor(firstName: string, lastName: string, location: string, currentPosition: string) {
        this.firstName = firstName
        this.lastName = lastName
        this.location = location
        this.currentPosition = currentPosition

    }
}
