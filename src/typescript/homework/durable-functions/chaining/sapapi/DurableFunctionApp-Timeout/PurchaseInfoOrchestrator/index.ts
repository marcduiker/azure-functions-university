import * as df from "durable-functions"
import moment from "moment"

import { MaterialGroupData, ProductGroupInfo } from "../utils/purchaseOrderTypes"

const orchestrator = df.orchestrator(function* (context) {

    const timeoutInMilliseconds: number = 3000
    const deadline = moment.utc(context.df.currentUtcDateTime).add(timeoutInMilliseconds, "ms")

    const timeoutTask = context.df.createTimer(deadline.toDate())

    const materialGroupTask = context.df.callActivity("GetPoDetails", context.bindingData.input)

    const winner = yield context.df.Task.any([materialGroupTask, timeoutTask])

    if (winner === materialGroupTask) {

        context.log("Dunning level fetched before timeout")

        timeoutTask.cancel();

        let materialGroup: MaterialGroupData = <MaterialGroupData>materialGroupTask.result;
        context.bindingData.input.materialGroupId = materialGroup.materialGroupId
    }
    else {
        context.log("OData Call for PO details timed out ...")
        throw new Error("OData Call for PO details timed out")
    }

    const productGroupInfo: ProductGroupInfo = yield context.df.callActivity("GetPgText", context.bindingData.input)

    return productGroupInfo

})

export default orchestrator
