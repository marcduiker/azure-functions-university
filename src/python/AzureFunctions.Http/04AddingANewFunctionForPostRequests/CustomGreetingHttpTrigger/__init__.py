from dataclasses import dataclass
import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:

    @dataclass
    class Person:
        name: str
    
    try:
        req_body = req.get_json()
    except ValueError:
        pass
    else:
        person = Person(req_body.get('name'))

    if (person.name):
        return func.HttpResponse(f"Hello, {person.name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
