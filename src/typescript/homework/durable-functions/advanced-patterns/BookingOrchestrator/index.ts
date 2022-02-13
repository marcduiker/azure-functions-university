import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const bookingTasks = []

    bookingTasks.push(context.df.callActivity("BuyConferenceTicketActivity", context.bindingData.input))
    bookingTasks.push(context.df.callActivity("BookHotelRoomActivity", context.bindingData.input))
    bookingTasks.push(context.df.callActivity("BookTrainTicketActivity", context.bindingData.input))


    const result = yield context.df.Task.all(bookingTasks)

    return result


})

export default orchestrator
