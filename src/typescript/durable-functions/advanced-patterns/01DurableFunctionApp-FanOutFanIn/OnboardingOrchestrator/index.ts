import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const onboardingTasks = [] 

    onboardingTasks.push(context.df.callActivity("AccessCardCreationActivity", context.bindingData.input))
    onboardingTasks.push(context.df.callActivity("ItEquipmentOrderActivity", context.bindingData.input))
    onboardingTasks.push(context.df.callActivity("WelcomeEmailActivity", context.bindingData.input))


    const result = yield context.df.Task.all(onboardingTasks)

    return result

})

export default orchestrator
