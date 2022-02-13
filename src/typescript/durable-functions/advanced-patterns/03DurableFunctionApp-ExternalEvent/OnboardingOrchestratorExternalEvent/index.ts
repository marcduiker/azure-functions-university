import * as df from "durable-functions"
import { DateTime } from "luxon"

const orchestrator = df.orchestrator(function* (context) {

    const checkResult = yield context.df.callActivity("CheckItEquipmentValueByRoleActivity", context.bindingData.input)

    let orderApproved = false

    if (checkResult !== "approved") {

        const expiration = DateTime.fromJSDate(context.df.currentUtcDateTime, { zone: 'utc' }).plus({ seconds: 90 });
        const timeoutTask = context.df.createTimer(expiration.toJSDate())

        const approvalTask = context.df.waitForExternalEvent("ApprovalRequest")

        const winner = yield context.df.Task.any([approvalTask, timeoutTask])

        if (winner === approvalTask) {

            if (approvalTask.result === 'approved') {
                orderApproved = true
            }
            else {
                orderApproved = false
            }

        } else {

            context.log.warn("Timeout expired");
        }

        if (!timeoutTask.isCompleted) {

            timeoutTask.cancel()

        }
    }

    if (checkResult === "approved" || orderApproved === true) {
        const orderResult = yield context.df.callActivity("ItEquipmentOrderActivity", context.bindingData.input)

        return orderResult
    }
    else {
        const message = "Order was declined"

        context.log.error(message)

        return message

    }

})

export default orchestrator
