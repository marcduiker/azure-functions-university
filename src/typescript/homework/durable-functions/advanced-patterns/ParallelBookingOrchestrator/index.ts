import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const trips2book = context.bindingData.input.trips2book

    if (trips2book) {

        const bookingList = []

        for (const tripEntry of trips2book) {

            const childId = context.df.instanceId + `:${tripEntry.name}`
            const bookingListEntry = context.df.callSubOrchestrator("BookingOrchestrator", tripEntry, childId)

            bookingList.push(bookingListEntry)

        }

        if (context.df.isReplaying === false) {
            context.log.info(`Starting ${bookingList.length} sub-orchestrations for update`)
        }

        yield context.df.Task.all(bookingList)
    }
    else {
        context.log.warn("No trips to book")
    }


})

export default orchestrator
