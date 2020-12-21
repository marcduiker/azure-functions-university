import { AzureFunction, Context } from "@azure/functions"
import { PurchaseOrderItem } from "@sap/cloud-sdk-vdm-purchase-order-service"

const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {

    await sleep(10000)

    try {
        let purchaseOrderItemDetails = await getPurchaseOrderItemDetails({ purchaseOrderId: context.bindingData.purchaseOrderId.toString(), purchaseOrderItemId: context.bindingData.purchaseOrderItemId.toString() })

        const result: JSON = <JSON><any>{ "materialGroupId": purchaseOrderItemDetails.materialGroup }

        return result

    }
    catch (error) {
        context.log("Error in OData call happened: ", error)
        throw error
    }

}


async function sleep(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms))
}

async function getPurchaseOrderItemDetails(
    { purchaseOrderId,
        purchaseOrderItemId
    }:
        {
            purchaseOrderId: string,
            purchaseOrderItemId: string
        }
): Promise<PurchaseOrderItem> {
    return PurchaseOrderItem.requestBuilder()
        .getByKey(purchaseOrderId, purchaseOrderItemId)
        .withCustomHeaders({ APIKey: process.env["APIHubKey"] })
        .execute({
            url: process.env["APIHubDestination"]
        })
}

export default activityFunction
