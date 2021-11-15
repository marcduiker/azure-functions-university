import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    if reg.method == "GET":
        name = req.params.get('name')
    
    elif reg.method == "POST":
        reg_body = req.get_json()
        reg_name = req_body.get('name')
        name = req.get_json(), reg_name



    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
