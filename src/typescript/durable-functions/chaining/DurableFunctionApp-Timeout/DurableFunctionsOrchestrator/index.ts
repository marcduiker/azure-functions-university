import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {
    const outputs = []

    outputs.push(yield context.df.callActivity("HelloCity", "Tokyo"))
    outputs.push(yield context.df.callActivity("HelloCity", "Seattle"))
    outputs.push(yield context.df.callActivity("HelloCity", "London"))

    return outputs
})

export default orchestrator