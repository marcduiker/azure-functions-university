import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const userId:string = yield context.df.callActivity("GetRepositoryDetailsByName", context.bindingData.input)
    
    context.bindingData.input.userId = userId

    const userInfos = yield context.df.callActivity("GetUserDetailsById", context.bindingData.input)

    return userInfos
})

export default orchestrator
