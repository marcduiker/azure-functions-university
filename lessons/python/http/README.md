# HTTP Trigger (Python)

Watch the recording of this lesson [on YouTube 🎥](https://youtu.be/fDnPGeRTwHc).

## Goal 🎯

The goal of this lesson is to create your first function which can be triggered by doing an HTTP GET or POST to the function endpoint.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Creating a Function App](#1-creating-a-function-app)
|2|[Changing the template for GET requests](#2-changing-the-template-for-get-requests)
|3|[Changing the template for POST requests](#3-changing-the-template-for-post-requests)
|4|[Adding a new function for POST requests](#4-adding-a-new-function-for-post-requests)
|5|[Homework](#5-homework)
|6|[More info](#6-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/python/AzureFunctions.Http) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| An empty local folder / git repo | 1-5
| Azure Functions Core Tools | 1-5
| VS Code with Azure Functions extension| 1-5
| Rest Client for VS Code or Postman | 1-5

See [Python prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_).

    > 📝 **Tip** - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you will be using to code the function. In this lesson we will be using `python`.
4. Select `HTTP trigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Select `Function` for the AccessRights.
    > 🔎 **Observation** - Now a new Azure Functions project is being generated. Once it's done, look at the files in the project. You will see the following:

    |File|Description
    |-|-
    |`HelloWorldHttpTrigger/__init__.py`|The Python file containing your function code exported as an Azure Function.
    |`HelloWorldHttpTrigger/function.json`|The [Azure Function configuration](https://docs.microsoft.com/azure/azure-functions/functions-reference?tabs=blob) comprising the function's trigger, bindings, and other configuration settings.  
    |`requirements.txt`|Contains the required Python packages.
    |`host.json`|Contains [global configuration options](https://docs.microsoft.com/azure/azure-functions/functions-host-json) for all functions in a function app.
    |`local.settings.json`|Contains [app settings and connection strings](https://docs.microsoft.com/azure/azure-functions/functions-run-local?tabs=v4%2Cmacos%2Ccsharp%2Cportal%2Cbash%2Ckeda#local-settings) for local development.

    > 📝 **Tip** - When you create a Function App, VSCode automatically generates a [virtual environment](https://docs.python.org/3/library/venv.html) for you. If you navigate to the functions directory you can activate it using `source .venv/bin/activate`. If you install new Python packages you update your `requirements.txt` file using `pip freeze > requirements.txt`.

    > ❔ **Question** - Review the generated HTTPTrigger function. What is it doing?

7. Install the dependencies defined in the `requirements.txt` via `pip install -r requirements.txt` in a shell of your choice.

8. Start the Function App by pressing `F5` or via `func host start`.
    > 🔎 **Observation** - You should see an HTTP endpoint in the output.

0. Now call the function by making a GET request to the above endpoint using a REST client:

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger?name=YourName
    ```

    > 📝 **Tip** - You can use [Postman](https://www.postman.com/) as a REST client.

    > ❔ **Question** - What is the result of the function? Is it what you expected?

    > ❔ **Question** - What happens when you don't supply a value for the name?

## 2. Changing the template for GET requests

Let us change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed. Start with only allowing GET requests.

### Steps

1. Remove the `"post"` entry from the `"methods"`array in the `function.json` file. Now the function can only be triggered by a GET request.
2. Switch to the file `__init__.py`. Here we leave the `req.params` parameter unchanged. However, we will remove the `body` parameters associated with the POST request.
3. Remove the POST content of the function (but keep the function definition). We'll be writing a new implementation.
4. To get the name from the query string you can do the following:

    ```python
    name = req.params.get('name')
    ```

    > 🔎 **Observation** - In the generated template the response object always returns an HTTP status 200. Let's make the function a bit smarter and return a response object with HTTP status 400.

5. Add an `if` statement to the function that checks if the name value is `null`, an empty string or `undefined`. If this is this case we return an HTTP code 400 as response, otherwise we return an HTTP code 200.

    ```python   
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
    ```

    Now the function has proper return values for both correct and incorrect invocations.

    > 📝 **Tip** - This solution hard codes the HTTP status codes. To make the handling more consistent you can either define your own enumerations for the HTTP codes.

6. Run the function, once without name value in the query string, and once with a name value.

    > ❔ **Question** - Is the outcome of both runs as expected?

## 3. Changing the template for POST requests

Let's change the function to also allow POST requests and test it by posting a request with JSON content in the request body.

### Steps

1. Add the `"post"` entry in the `"methods"`array in the `function.json` file.
2. The function in the `__init__.p` file currently only handles GET requests. We need to add some logic to use the query string for GET and the request body for POST requests. This can be done by checking the method property of the request parameter as follows:

    ```python
    method_type = req.method
    if method_type == "GET":
        # Get name from query string
        # name = ...
    elif method_type == "POST":
        # Get name from body
        # name = ...
    ```

3. We will assign values to the variable `name` variable depending on the HTTP method.
4. Move the query string logic inside the `if` statement that handles the GET request. The `if`-branch handling GET requests should look like this:

    ```python
    method_type = req.method
    if method_type == "GET":
        name = req.params.get('name')
    ```

5. Now let's add the code to extract the name from the body for a POST request. The `else if`-branch handling the POST request should look like this:

    ```python
    elif method_type == "POST":
        try:
            req_body = req.get_json()
        except ValueError:
            name = None
        else:
            name = req_body.get('name')
    ```

    > 📝 **Tip** - We need to check if the body exists at all before we assign the value.

6. We also adopt the message in case of an invalid request to handle the two different error situations. The construction of the response object has the following form after our changes:

    ```python
    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
        status_code=200
    else:
        return func.HttpResponse(
             "Pass a name in the query string (GET request) or a JSON body with the attribute name (POST request) for a personalized response.",",
             status_code=400
        )
    ```

7. Now run the function and do a POST request and submit JSON content with a `name` attribute. If you're using the [VSCode REST client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) you can use this in a .http file:

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: application/json

    {
        "name": "Your name"
    }
    ```

    > ❔ **Question** - Is the outcome of the POST as expected?
    
    > ❔ **Question** - What is the response when you use an empty `name` property?

## 4. Adding a new function for POST requests
 
Let's change the function to map the the requst with JSON content to a Python class object. Since we only storing state we can use a [dataclass](https://docs.python.org/3/library/dataclasses.html).

### Steps

1. Copy & paste the folder of the Azure Function from the exercise above and give it a new name e.g. `CustomGreetingHttpTrigger`.

    > 📝 **Tip** - Function names need to be unique within a Function App.

2. Adjust the `"scriptfile"` attribute in the `function.json` file to the new filename to get a consistent transpilation.
3. Remove the `"get"` entry from the `"methods"`array in the `function.json` file. Now the function can only be triggered by a POST request.
4. Switch to the the `_init_.py` file and add a new interface named `Person` to the `_init_.py` file.

   ```python
    @dataclass
    class Person:
        name: str = None
   ```

5. Remove the logic inside the function which deals GET Http verb and with the query string.
6. Rewrite the function logic that the request body is assigned to the interface `Person`.

    ```python
    try:
        req_body = req.get_json()
    except ValueError:
        person = Person(name=None)
    else:
        person = Person(name=req_body.get('name'))
    ```

7. Update the logic which checks if the `name` variable is empty. You can now use `person.name` instead. However, be aware that the request body can be empty which would result in an undefined assignment of the attribute `name` in the `if` statement, so we must still check that the person is not undefined. The updated code should look like this:

    ```python
    if (person.name):
        return func.HttpResponse(f"Hello, {person.name}. This HTTP triggered function executed successfully.")
        status_code=200
    else:
        return func.HttpResponse(
             "Pass a name in the request's JSON body with the attribute name (POST) for a personalized response."
             responseStatus = 400
        )
    ```

8. Run the function.
    > 🔎 **Observation** You should see two HTTP endpoints in the output of the console.

9. Trigger the new endpoint by making a POST request.

    > ❔ **Question** Is the outcome as expected?

## 5. Homework

Ready to get hands-on? Checkout the [homework assignment for this lesson](http-homework-python.md).

## 6. More info

For more info about the HTTP Trigger have a look at the official [Azure Functions HTTP Trigger](https://docs.microsoft.com/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=python) documentation.

---
[🔼 Lessons Index](../../README.md) | [Homework ▶](http-homework-python.md)
