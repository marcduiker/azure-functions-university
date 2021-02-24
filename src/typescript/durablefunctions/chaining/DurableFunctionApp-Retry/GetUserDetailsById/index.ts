import { AzureFunction, Context } from "@azure/functions"
import { Octokit } from "@octokit/core"

const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {
 
    const octokit = new Octokit()

    const apiPath = `/users/${context.bindingData.userId.toString()}`
    
    const searchResult = await octokit.request( apiPath )
    
    const userData = <JSON><any> searchResult.data
    
    return userData
    
}

export default activityFunction
