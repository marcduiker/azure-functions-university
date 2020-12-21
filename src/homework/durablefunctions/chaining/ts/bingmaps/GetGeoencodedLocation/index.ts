import { AzureFunction, Context } from "@azure/functions"
import axios from "axios"


const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {

    const response = await getGeolocation(context.bindingData.location.toString())
    return response

}

async function getGeolocation(location: string) {
    try {

        const baseURL: string = "https://dev.virtualearth.net/REST/v1/Locations/"
        const APIkey: string = "?key=" + process.env["BingMapsAPIKey"]

        const finalURL = baseURL + location + APIkey
        const response = await axios.get(finalURL)

        return <JSON><any>response.data

    } catch (error) {
        console.error(error)
    }
}

export default activityFunction
