import { AzureFunction, Context } from "@azure/functions"
import { Octokit } from "@octokit/core"

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    if (context.bindingData.raiseException && context.bindingData.raiseException === true) {
        context.log("Error was enforced by caller")
        throw {
            name: "ForcedException",
            message: "Caller enforced exception",
            toString: function () {
                return this.name + ": " + this.message;
            }
        }
    }

    const octokit = new Octokit()

    const query = `${context.bindingData.repositoryName.toString()} in:name`

    const searchResult = await octokit.request('GET /search/repositories', {
        q: query
    })

    const exactMatch = searchResult.data.items.find(item => item.name === context.bindingData.repositoryName.toString())

    return exactMatch.owner.login
}

export default activityFunction