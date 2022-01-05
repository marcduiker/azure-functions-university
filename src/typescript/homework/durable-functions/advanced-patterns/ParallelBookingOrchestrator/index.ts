import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const travels2book = context.bindingData.input.travels2book

    if (travels2book) {

        const bookingList = []

        let id = 0

        for (const travelEntry of travels2book) {

            const child_id = context.df.instanceId + `:${travelEntry.name}`
            const bookingListEntry = context.df.callSubOrchestrator("BookingOrchestrator", travelEntry, child_id)

            bookingList.push(bookingListEntry)

            id++
        }

        if (context.df.isReplaying === false) {
            context.log.info(`Starting ${bookingList.length} sub-orchestrations for update`)
        }

        yield context.df.Task.all(bookingList)
    }
    else {
        context.log.warn("No travels to book")
    }


})

export default orchestrator
