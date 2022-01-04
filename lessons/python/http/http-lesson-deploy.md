# Deploy your Azure function

In this step we will deploy the function that we created in your homework. This is not a mandatory step.
You can create a Azure account using this [link](https://azure.microsoft.com/en-gb/).

## Command line

1. To interact with Azure’s servers, we will use the Azure CLI command, `az`. First you need to log into your Azure account by running `az login`. It will load up the browser and ask you to log in to your Azure account. 

    ```bash
    # Connect `az` to your Azure account:
    az login
    ```

2. Now, you will need to run the three az commands below. You will need to change is to replace `MYAPPFUNCTIONSTORE` with something globally unique. 
The actual name you pick isn’t important – it’s just a place to store the data for your running functions, and won’t be seen by the users. 
You will also need to change `MYFIRSTAPP` to something globally unique.

    ```bash
    # Create a resource group:
    az group create --name myResourceGroup --location westeurope

    # Create a storage account for storing your function data:
    az storage account create --name "MYAPPFUNCTIONSTORE" \
        --location westeurope --resource-group myResourceGroup \
        --sku Standard_LRS

    # Create a function app for grouping your functions together:
    az functionapp create --resource-group myResourceGroup --os-type Linux \
        --consumption-plan-location westeurope  --runtime python \
        --name "MYFIRSTAPP" --storage-account  "MYAPPFUNCTIONSTORE"
    ```

    ```bash
    # Publish Your Function to Azure
    func azure functionapp publish MYFIRSTAPP --build remote
    ```
 
4. You can check that your function is deployed properly by creating a POST request in Postman using `https://MYFIRSTAPP.azurewebsites.net/api/response_text_processing?code=<USE_YOUR_CODE_HERE>` and confirming that the JSON output is being correctly produced.

## Cleaning up
Don't forget to delete the created resource after the workshop ends! You can delete the resource by using the Azure portal.
Go to Resource Groups, click on the resource you want to delete and click delete resource group.

Or by using Azure CLI, just run the following command in the terminal: 

    ```bash
    az group delete --name MyResourceGroup
    ```

## Where to go next?
Now that you’re here, you’ve successfully written your first Python Azure function and can understand what happens when the function works according 
to your expectations — and also what happens when it fails. Like most things, however, this barely scratches the surface of the power of Azure Functions. So what now?

For starters, here are just a few ideas:

* You can build a [serverless web application](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/serverless/web-app).
* Build, train and deploy a [Machine Learning model](https://azure.microsoft.com/en-us/free/machine-learning/search/?&ef_id=Cj0KCQiA1-3yBRCmARIsAN7B4H1RepL2p1tSCOK06GtxfTVeimTE4Ccc1PJIEiZeV9ku_2GMyAh8a6waAmsNEALw_wcB:G:s&OCID=AID2000098_SEM_L0hncz7b&MarinID=L0hncz7b_369039617729_azure%20machine%20learning_e_c__75540808959_kwd-298261819911&lnkd=Google_Azure_Brand&dclid=CMGAtqLA-ecCFZYK4AodYmUNOA).

---
[🔼 Lessons Index](../../README.md)
