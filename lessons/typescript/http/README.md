# HTTP Trigger (TypeScript)

Watch the recording of this lesson [on YouTube 🎥](https://youtu.be/zYb5sVQgUN4).

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

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/typescript/AzureFunctions.Http) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| An empty local folder / git repo | 1-5
| Azure Functions Core Tools | 1-5
| VS Code with Azure Functions extension| 1-5
| Rest Client for VS Code or Postman | 1-5

See [TypeScript prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_).

    > 📝 **Tip** - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you will be using to code the function. In this lesson we will be using `TypeScript`.
4. Select `HTTP trigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Select `Function` for the AccessRights.
    > 🔎 **Observation** - Now a new Azure Functions project is being generated. Once it's done, look at the files in the project. You will see the following:

    |File|Description
    |-|-
    |HelloWorldHttpTrigger\index.ts|The TypeScript file containing your function code exported as an Azure Function.
    |HelloWorldHttpTrigger\function.json|The [Azure Function configuration](https://docs.microsoft.com/de-de/azure/azure-functions/functions-reference#function-code) comprising the function's trigger, bindings, and other configuration settings.  
    |package.json|Contains the manifest file for your project.
    |tsconfig.json|Contains the specification of the compiler options required to compile the TypeScript project.
    |host.json|Contains [global configuration options](https://docs.microsoft.com/en-us/azure/azure-functions/functions-host-json) for all functions in a function app.
    |local.settings.json|Contains [app settings and connection strings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-host-json) for local development.

    > 📝 **Tip** - The `function.json` file also contains the location of the transpiled .js file. Be aware to change this in case you copy & paste functions

    > ❔ **Question** - Review the generated HTTPTrigger function. What is it doing?

7. Install the dependencies defined in the `package.json` via `npm install` in a shell of your choice.

8. Build the project by making use of the predefined script in the `package.json` file via `npm run build`  in a shell of your choice.
    > 🔎 **Observation** - A new folder `dist` is created in your Azure Functions project directory that contains the transpiled JavaScript files as well as your source map files needed for debugging your TypeScript files.

9. Start the Function App by pressing `F5` or via `npm run start`.
    > 🔎 **Observation** - You should see an HTTP endpoint in the output.

10. Now call the function by making a GET request to the above endpoint using a REST client:

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger?name=YourName
    ```

    > ❔ **Question** - What is the result of the function? Is it what you expected?

    > ❔ **Question** - What happens when you don't supply a value for the name?

## 2. Changing the template for GET requests

Let us change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed. Start with only allowing GET requests.

### Steps

1. Remove the `"post"` entry from the `"methods"`array in the `function.json` file. Now the function can only be triggered by a GET request.
2. Switch to the file `index.ts`. Here we leave the `req` parameter unchanged. However, we will ignore the `body` parameter defined on the interface `HttpRequest` and only take the `query` parameter into account.
3. Remove the content of the function (but keep the function definition). We'll be writing a new implementation.
4. To get the name from the query string you can do the following:

    ```typescript
    const name = req.query.name 
    ```

    > 🔎 **Observation** - In the generated template the response object always returns an HTTP status 200. Let's make the function a bit smarter and return a response object with HTTP status 400.

5. Add an `if` statement to the function that checks if the name value is `null`, an empty string or `undefined`. If this is this case we return an HTTP code 400 as response, otherwise we return an HTTP code 200.

    ```typescript
    let responseMessage: string
    let responseStatus: number
    
    if (name) {
        responseStatus = 200
        responseMessage = `Hello, ${name}. This HTTP triggered function executed successfully.`
    }
    else {
        responseStatus = 400
        responseMessage = `Pass a name in the query string or in the request body for a personalized response.`
    }

    context.res = {
        status: responseStatus,
        body: responseMessage
    }
    ```

    Now the function has proper return values for both correct and incorrect invocations.

    > 📝 **Tip** - This solution hard codes the HTTP status codes. To make the handling more consistent you can either define your own enumerations for the HTTP codes or use an npm package like [http-status-codes](https://www.npmjs.com/package/http-status-codes).

6. Run the function, once without name value in the query string, and once with a name value.

    > ❔ **Question** - Is the outcome of both runs as expected?

## 3. Changing the template for POST requests

Let's change the function to also allow POST requests and test it by posting a request with JSON content in the request body.

### Steps

1. Add the `"post"` entry in the `"methods"`array in the `function.json` file.
2. The function in the `index.ts` file currently only handles GET requests. We need to add some logic to use the query string for GET and the request body for POST requests. This can be done by checking the method property of the request parameter as follows:

    ```typescript
      if (req.method === "GET"){
        // Get name from query string
        // name = ...
      }
      else if (req.method === "POST"){
        // Get name from body
        // name = ...
      }
    ```

    > 📝 **Tip** - The code shows the HTTP verbs as strings. However, due to the declaration of the type in the request interface the validity is checked during design time as well as ode completion is available in the editor. This makes sure that you use valid values.

3. We will assign values to the variable `name` variable depending on the HTTP method. Therefore change the declaration of the `name` variable to `let name: string` and remove the assignment of an from the request.
4. Move the query string logic inside the `if` statement that handles the GET request. Leave the creation of the result object outside of the `if ... else if` statement as this can be used for both branches. The `if`-branch handling GET requests should look like this:

    ```typescript
    if (req.method === "GET") {
        name = req.query.name
    }
    ```

5. Now let's add the code to extract the name from the body for a POST request. The `else if`-branch handling the POST request should look like this:

    ```typescript
        else if (req.method === "POST") {
        name = (req.body && req.body.name)
    }
    ```

    > 📝 **Tip** - We need to check if the body exists at all before we assign the value.

6. We also adopt the message in case of an invalid request to handle the two different error situations. The construction of the response object has the following form after our changes:

    ```typescript
     if (name) {
        responseStatus = 200
        responseMessage = `Hello, ${name}. This HTTP triggered function executed successfully.`
    }
    else {
        responseStatus = 400
        responseMessage = `Pass a name in the query string (GET request) or a JSON body with the attribute "name" (POST request) for a personalized response.`
    }
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

In contrast to C# the typing of the request parameter of the function is restricted to `string`, `HttpRequest` and `buffer`, so we cannot have a type-safety on this level of the function. Nevertheless we can make use of implicitly assigning the JSON body of the request to an interface which gives us consistent typing at design time.

### Steps

1. Copy & paste the folder of the Azure Function from the exercise above and give it a new name e.g. `CustomGreetingHttpTrigger`.

    > 📝 **Tip** - Function names need to be unique within a Function App.

2. Adjust the `"scriptfile"` attribute in the `function.json` file to the new filename to get a consistent transpilation.
3. Remove the `"get"` entry from the `"methods"`array in the `function.json` file. Now the function can only be triggered by a POST request.
4. Switch to the the `index.ts` file and add a new interface named `Person` to the `index.ts` file.

   ```typescript
    interface Person {
        name: string
    }
   ```

5. Remove the logic inside the function which deals GET Http verb and with the query string.
6. Rewrite the function logic that the request body is assigned to the interface `Person`.

    ```typescript
     const person: Person = req.body
    ```

7. Update the logic which checks if the `name` variable is empty. You can now use `person.Name` instead. However, be aware that the request body can be empty which would result in an undefined assignment of the attribute `name` in the `if` statement, so we must still check that the person is not undefined. The updated code should look like this:

    ```typescript
    let responseMessage: string
    let responseStatus: number

    if (person && person.name) {
        responseMessage = `Hello, ${person.name}. This HTTP triggered function executed successfully.`
        responseStatus = 200
    }
    else {
        responseMessage = `Pass a name in the request's JSON body with the attribute "name" (POST) for a personalized response.`
        responseStatus = 400
    }

    context.res = {
        status: responseStatus,
        body: responseMessage
    }
    ```

8. Run the function.
    > 🔎 **Observation** You should see two HTTP endpoints in the output of the console.

9. Trigger the new endpoint by making a POST request.

    > ❔ **Question** Is the outcome as expected?

## 5. Homework

Ready to get hands-on? Checkout the [homework assignment for this lesson](http_homework-ts.md).

## 6. More info

For more info about the HTTP Trigger have a look at the official [Azure Functions HTTP Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=javascript) documentation.

---
[🔼 Lessons Index](../../README.md)
