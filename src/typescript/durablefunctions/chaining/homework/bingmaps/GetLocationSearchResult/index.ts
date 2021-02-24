import { AzureFunction, Context } from "@azure/functions"
import axios from "axios"


const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {

    const response = await getSearchResult(context.bindingData.type.toString(), context.bindingData.geolocationPoint0.toString(), context.bindingData.geolocationPoint1.toString())
    return response

}

async function getSearchResult(type: string, point0: string, point1: string) {
    try {

        const baseURL: string = "https://dev.virtualearth.net/REST/v1/LocalSearch/"
        const APIkey: string = "&key=" + process.env["BingMapsAPIKey"]

        const finalURL = baseURL + "?query=" + type + "&userLocation=" + point0 + "," + point1 + APIkey
        const response = await axios.get(finalURL)

        return <JSON><any>response.data

    } catch (error) {
        console.error(error)
    }
}

export default activityFunction