import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    name = req.params.get('name')

    if name:
        return func.HttpResponse(
            f"Hello, {name}. This HTTP triggered function executed successfully.", 
            status_code=200
            )
    else:
        return func.HttpResponse(
             "Pass a name in the query string or in the request body for a personalized response",
             status_code=400
        )
