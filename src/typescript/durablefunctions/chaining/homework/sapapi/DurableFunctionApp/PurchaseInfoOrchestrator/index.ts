import * as df from "durable-functions"
import { MaterialGroupData, ProductGroupInfo } from "../utils/purchaseOrderTypes"

const orchestrator = df.orchestrator(function* (context) {

    const materialGroup: MaterialGroupData = yield context.df.callActivity("GetPoDetails", context.bindingData.input)

    context.bindingData.input.materialGroupId = materialGroup.materialGroupId

    const productGroupInfo: ProductGroupInfo = yield context.df.callActivity("GetPgText", context.bindingData.input)

    return productGroupInfo

})

export default orchestrator
