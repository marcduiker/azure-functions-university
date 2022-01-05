import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const employees2onboard = context.bindingData.input.employees2onboard

    if (employees2onboard) {

        const onboardingList = []

        let id = 0

        for (const employeeEntry of employees2onboard) {

            const child_id = context.df.instanceId + `:${employeeEntry.name}`
            const onboardingListEntry = context.df.callSubOrchestrator("OnboardingOrchestrator", employeeEntry, child_id)

            onboardingList.push(onboardingListEntry)

            id++
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
