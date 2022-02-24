import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const employees2onboard = context.bindingData.input.employees2onboard

    if (employees2onboard) {

        const onboardingList = []

        for (const employeeEntry of employees2onboard) {

            const childId = context.df.instanceId + `:${employeeEntry.name}`
            const onboardingListEntry = context.df.callSubOrchestrator("OnboardingOrchestrator", employeeEntry, childId)

            onboardingList.push(onboardingListEntry)

        }

        if (context.df.isReplaying === false) {
            context.log.info(`Starting ${onboardingList.length} sub-orchestrations for update`)
        }

        yield context.df.Task.all(onboardingList)
    }
    else {
        context.log.warn("No employees to onboard")
    }

})

export default orchestrator
