import * as df from "durable-functions"
import moment from "moment"

const orchestrator = df.orchestrator(function* (context) {

    const timeoutInMilliseconds: number = 3000
    const deadline = moment.utc(context.df.currentUtcDateTime).add(timeoutInMilliseconds, "ms")

    const timeoutTask = context.df.createTimer(deadline.toDate())

    const repositoryDetailsTask = context.df.callActivity("GetRepositoryDetailsByName", context.bindingData.input)

    const winner = yield context.df.Task.any([repositoryDetailsTask, timeoutTask])

    if (winner === repositoryDetailsTask) {

        context.log("Repository Information fetched before timeout")

        timeoutTask.cancel();

        const userId = repositoryDetailsTask.result
        context.bindingData.input.userId = userId
    }
    else {
        context.log("Repository Information call timed out ...")
        throw new Error("Repository Information call timed out")
    }

    const userInfos = yield context.df.callActivity("GetUserDetailsById", context.bindingData.input)

    return userInfos
})

export default orchestrator
