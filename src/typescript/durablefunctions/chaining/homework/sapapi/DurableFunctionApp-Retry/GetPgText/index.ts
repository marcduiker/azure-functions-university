import { AzureFunction, Context } from "@azure/functions"
import { ProductGroupText } from "@sap/cloud-sdk-vdm-product-group-service"

const activityFunction: AzureFunction = async function (context: Context): Promise<JSON> {

    try {
        let productGroupText = await getProductGroupText({ materialGroupId: context.bindingData.materialGroupId.toString(), languageCode: context.bindingData.languageCode.toString() })

        const result: JSON = <JSON><any>{
            "materialGroup": productGroupText.materialGroup,
            "language": productGroupText.language,
            "materialGroupName": productGroupText.materialGroupName,
            "materialGroupText": productGroupText.materialGroupText
        }

        return result

    }
    catch (error) {
        context.log("Error in OData call happened: ", error)
        throw error
    }

}

async function getProductGroupText(
    { materialGroupId,
        languageCode
    }:
        {
            materialGroupId: string,
            languageCode: string
        }
): Promise<ProductGroupText> {
    return ProductGroupText.requestBuilder()
        .getByKey(materialGroupId, languageCode)
        .withCustomHeaders({ APIKey: process.env["APIHubKey"] })
        .execute({
            url: process.env["APIHubDestination"]
        })
}

export default activityFunction;
