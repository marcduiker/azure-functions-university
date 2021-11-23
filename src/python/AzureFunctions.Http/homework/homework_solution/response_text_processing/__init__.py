import logging
import json

import azure.functions as func

from ..shared_code import nlp_text_processing as tp


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    file_sent = None
    text = ""

    try:
        file_sent = req.get_body()
    except ValueError as e:
        logging.error("Value Error: ", e)
        raise ValueError
    else:
        text = str(file_sent)

    processed_text = tp.remove_stop_words(text)
    tokens = tp.tokenize_text(processed_text)
    entities = tp.get_entities(tokens)

    if file_sent:
        return func.HttpResponse(
            json.dumps([ {
                "processedText": processed_text,
                "tokens": tokens,
                "entities": entities
            } ]),
            status_code=200
        )
    else:
        return func.HttpResponse(
            "Please pass a file in the request body",
            status_code=400
        )
