from dataclasses import dataclass
import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    @dataclass
    class Person:
        name: str = None

    try:
        req_body = req.get_json()
    except ValueError:
        person = Person(name=None)
    else:
        person = Person(name=req_body.get("name"))

    if person.name:
        return func.HttpResponse(
            f"Hello, {person.name}. This HTTP triggered function executed"
            " successfully.",
            status_code=200,
        )
    else:
        return func.HttpResponse(
            "Pass a name in the request's JSON body with the attribute name"
            " (POST) for a personalized response.",
            status_code=400,
        )
