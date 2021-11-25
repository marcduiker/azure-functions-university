import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:

    method_type = req.method

    if method_type == "GET":
        name = req.params.get("name")
    elif method_type == "POST":
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get("name")

    if name:
        return func.HttpResponse(
            f"Hello, {name}. This HTTP triggered function executed"
            " successfully.",
            status_code=200,
        )
    else:
        return func.HttpResponse(
            "Pass a name in the query string (GET request) or a JSON body with"
            " the attribute name (POST request) for a personalized response.",
            status_code=400,
        )
