# HTTP Trigger (.NET 6)

## Goal 🎯

The goal of this lesson is to create your first function which can be triggered by doing an HTTP GET or POST to the function endpoint.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Creating a Function App](#1-creating-a-function-app)
|2|[Changing the template for GET requests](#2-changing-the-template-for-get-requests)
|3|[Adding a BadRequest response](#3-adding-a-badrequest-response)
|4|[Handling POST requests with string data](#4-handling-post-requests-with-string-data)
|5|[Handling POST requests with JSON data](#5-handling-post-requests-with-json-data)
|6|[Changing the route for a custom greeting](#6-changing-the-route-for-a-custom-greeting)
|7|[Homework](#7-homework)
|8|[More info](#8-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/dotnet6/http/AzFuncUni.Http) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| Azure Functions Core Tools | 1-6
| VS Code with Azure Functions extension| 1-6
| REST Client for VS Code or Postman | 1-6

See [.NET 6 prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger and review the generated files & code.

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. *AzFuncUni.Http*).

    > 📝 **Tip** - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you'll be using to code the function, in this lesson that is using `C#`.
4. Select the `.NET 6` (isolated) as the runtime.

    If you don't see .NET 6, choose:
    - `Change Azure Functions version`
    - Select `Azure Functions v4`
    - Select `.NET 6 (isolated)`

    > 📝 **Tip** - More information about the isolated process can be found in the [official Azure documentation](https://docs.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide).

5. Select `HTTPTrigger` as the template.
6. Give the function a name (e.g. `HelloWorldHttpTrigger`).
7. Enter a namespace for the function (e.g. `AzFuncUni.Http`).
    > 📝 **Tip** - Namespaces are used to organize pieces of code into a hierarchy. Make sure you don't use the exact same name as the function/class. Namespaces and classes should be named uniquely to prevent compiler and readability issues.
8. Select `Function` for the AccessRights.
    > 🔎 **Observation** - Now a new Azure Functions project is being generated. Once it's done, look at the files in the project. You will see the following:

    |File|Description
    |-|-
    |AzFuncUni.Http.csproj|The C# project file which specifies the .NET version, Azure Functions version and package references.
    |Program.cs|The C# class containing the [startup code](https://docs.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide#start-up-and-configuration) for the host instance.
    |HelloWorldHttpTrigger.cs|The C# class containing the HTTPTrigger function method.
    |host.json|Contains [global configuration options](https://docs.microsoft.com/azure/azure-functions/functions-host-json) for all the functions in a function app.
    |local.settings.json|Contains [app settings and connectionstrings](https://docs.microsoft.com/azure/azure-functions/functions-run-local?tabs=v4%2Cwindows%2Ccsharp%2Cportal%2Cbash%2Ckeda#local-settings) for local development.

    > ❔ **Question** - Review the generated HTTPTrigger function. What is it doing?
9. Build the project (CTRL+SHIFT+B).

10. Run the Function App by pressing `F5`.
    > 🔎 **Observation** - Eventually you should see a local HTTP endpoint in the output.
11. Now call the function by making a GET request to the above endpoint using a REST client:

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger
    ```

    > ❔ **Question** - What is the result of the function? Is it what you expected?

12. Now call the function by making a POST request to the above endpoint using a REST client:

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    ```

    > ❔ **Question** - Do you know why the function works with both GET and POST HTTP methods? What would the response be if you try a PUT or DELETE HTTP method?

## 2. Changing the template for GET requests

Let's change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed and parameter types can be changed.

Start with only allowing GET requests and reading a value from the query string called `name`.

### Steps

1. Remove the `"post"` string from the `HttpTrigger` attribute. Now the function can only be triggered by a GET request.
2. The `req` parameter is of type `HttpRequestData`. Although this type has [various properties and methods](https://docs.microsoft.com/dotnet/api/microsoft.azure.functions.worker.http.httprequestdata?view=azure-dotnet) it does not contain anything to read query string values from the url. To read query string values, we can use the `HttpUtility.ParseQueryString()` method. The `HttpUtility` class lives in the `System.Web` namespace, so add this to the using directives at the top of the file:

    ```csharp
    using System.Web;
    ```

3. To read the `name` query string value, use the `ParseQueryString()` method directly underneath the `logger.LogInformation(...)` line:

    ```csharp
    var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
    var name = queryStringCollection["name"];
    ```

4. Now that a name value is extracted from the query string it can be used in the response.

    *Change*

    ```csharp
    response.WriteString("Welcome to Azure Functions!");
    ```

    *To*

    ```csharp
    response.WriteStringAsync($"Hello, {name}");
    ```

    The function should look like this now:

    ```csharp
    [Function("HelloWorldHttpTrigger")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
        var name = queryStringCollection["name"];
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteStringAsync($"Hello, {name}");
        return response;
    }
    ```

5. Run the function, once with a name value in the query string, and once without a name value.

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger?name=AzureFunctionsUniversity
    ```

    > ❔ **Question** - Is the outcome of both runs as expected?

## 3. Adding a BadRequest response

In the previous exercise, when the `name` query string value is missing, the function still returned an `OK` response (HTTP status code 200). When something is missing from a request it is better to return a `BadRequest` response (HTTP status code 400). This informs the client that the request was not valid and a corrective action needs to be taken.

Let's change the function to return a `BadRequest` response when the `name` query string is empty.

### Steps

1. Before changing the response, let's update the name of the function in the `[Function]` attribute. Currently it is a string literal, but some people don't like to use string literals and prefer strings based on .NET types. Instead of the string literal `"HelloWorldHttpTrigger"`, the .NET class name can be used like this: `nameof(HelloWorldHttpTrigger)`. Since the name in the `[Function()]` attribute is the same as the class name you can use this approach.

    *Change*

    ```csharp
    [Function("HelloWorldHttpTrigger")]
    ```

    *To*

    ```csharp
    [Function(nameof(HelloWorldHttpTrigger))]
    ```

    > 📝 **Tip**  .NET uses *strong typing*, this means that types and their values are checked at compile time (while you write your code). Languages that use *weakly typing* do type checking at runtime (when your program is running). Strong typing prevents you from making certain mistakes during programming, such as making typos, or using completely incorrect types or operations on types.

2. Now let's apply the proper HTTP status code in the response. Inside the function, after extracting the `name` value from the query string, add an `if` statement that checks the `name` field. If `name` is null, then return a `HttpStatusCode.BadRequest`. If the name is not null return a `HttpStatusCode.OK`.

    The function should look like this now:

    ```csharp
    [Function(nameof(HelloWorldHttpTrigger))]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
        var name = queryStringCollection["name"];

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (string.IsNullOrEmpty(name))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.WriteStringAsync($"Please provide a value for the name query string parameter.");
        }
        else
        {
            response.StatusCode = HttpStatusCode.OK;
            response.WriteStringAsync($"Hello, {name}");
        }

        return response;
    }
    ```

3. Run the function, once with a value in the name query string parameter, once without a value, and once without the name query parameter.

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger?name=AzureFunctionsUniversity
    ```

    > ❔ **Question** - Are all situations handled properly now?

## 4. Handling POST requests with string data

Let's change the function to also allow POST requests and test it by posting a request with a name in the body (as plain text).

### Steps

1. Update the `HttpTrigger` attribute of the function to include the POST HTTP verb;

    ```csharp
    [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
    ```

2. Now let's make the function a bit smarter and have it check the query string parameter only for GET requests, and check the request body only for POST requests. This can be done by adding and `if` statement that checks the `Method` property of the request:

    *Replace*

    ```csharp
        var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
        var name = queryStringCollection["name"];
    ```

    *With*

    ```csharp
    string name = default;
    if (req.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
    {
        var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
        name = queryStringCollection["name"];
    }
    else
    {
        name = await req.ReadAsStringAsync();
    }
    ```

3. Notice that the `ReadAsStringAsync()` method is an asynchronous method. The function needs to wait until that method is completed so the result is stored in the `name` field. That's why the `await` keyword is there before the method call. The `await` keyword can only be used in asynchronous methods and the function isn't. Let's make it asynchronous by adding the `async` keyword to the function:

    *Change*

    ```csharp
    public HttpResponseData Run(...)
    ```

    *To*

    ```csharp
    public async Task<HttpResponseData> Run(...)
    ```

    > 🔎 **Observation** - `async` methods always return a `Task` or Task<T>. In this case our response type is HttpResponseData, so the output type is `Task<HttpResponseData>`. The `Task` type is available in the `System.Threading.Tasks` namespace so add that to the using directives.

4. There was already an `async` method used in the function: `response.WriteStringAsync()`. Let's add an await to those two method calls as well:

    *Change*

    ```csharp
    response.WriteStringAsync(...)
    ```

    *To*

    ```csharp
    await response.WriteStringAsync(...)
    ```

5. Now run the function and do a POST request by submitting a string value in the body. If you're using the VSCode REST client you can use this in a .http file and execute it:

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

    > ❔ **Question** - Is the outcome of the POST as expected?

    > ❔ **Question** - What is the response when you use an empty body?

## 5. Handling POST requests with JSON data

Let's add a new function that only handles POST requests with a specific JSON structure in the request body.

### Steps

1. Create a copy of the `HelloWorldHttpTrigger.cs` file and rename the file, the class name and the `[Function()` attribute value to `HelloWorldHttpTrigger5`.

    > 📝 **Tip** - Function names need to be unique within a Function App.

2. Add a new file named `Person.cs` and add the following code:

    ```csharp
    namespace AzFuncUni.Http
    {
        record Person
        {
            public string Name { get; init; }
        }
    }
    ```

    This record will be used in the function to read a JSON structure from the request body.

    > 📝 **Tip** - This is a .NET `record` type, used for data classes which are supposed to be immutable. For more information see the [official Azure docs](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/record).

3. Remove the `"get"` verb from the `HttpTrigger` attribute since this function will only be triggered by POST requests. Make sure the `"post"` verb is still included in the attribute:
4. Since this function will only handle POST requests with JSON body the `if` statement can be replaced with retrieving a `Person` object from the request body:

    *Replace*

    ```csharp
    string name = default;
    if (req.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
    {
        var queryStringCollection = HttpUtility.ParseQueryString(req.UrlQuery);
        name = queryStringCollection["name"];
    }
    else
    {
        name = await req.ReadAsStringAsync();
    }
    ```

    *With*

    ```csharp
    var person = await req.ReadFromJsonAsync<Person>();
    ```

5. A `Person` object is now available in the `person` variable. Let's use the `person.Name` property in the two places where the `name` variable was used.

    The final function should look like this:

    ```csharp
    [Function(nameof(HelloWorldHttpTrigger5))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var person = await req.ReadFromJsonAsync<Person>();
        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (string.IsNullOrEmpty(person.Name))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            await response.WriteStringAsync($"Please provide a value for the name in JSON format in the body.");
        }
        else
        {
            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync($"Hello, {person.Name}");
        }

        return response;
    }
    ```

6. Run the Function App.

    > 🔎 **Observation** - You should see the new HTTP endpoint in the output of the console.

7. Trigger the new endpoint by making a POST request and submitting a JSON body with a `name` parameter. If you're using the VSCode REST client you can use this in a .http file and execute it:

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger5
    Content-Type: application/json

    {
        "name": "AzureFunctionsUniversity"
    }
    ```

    > ❔ **Question** - Is the outcome as expected?

## 6. Changing the route for a custom greeting

Instead returning *"Hello {name}"* all the time, it would be nice if we can supply our own greeting. So we could return *"Hi {name}"* or  *"Ola {name}"*. We can do this by changing the route of the function so it contains the greeting.

### Steps

1. Create a copy of the `HelloWorldHttpTrigger.cs` class that can handle a GET request and rename the file, the class and the `[Function()]` attribute to `HelloWorldHttpTrigger6.cs`.
2. Now add the `Route` parameter in the `HttpTrigger` binding as follows:

    ```csharp
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "HelloWorldHttpTrigger6/{greeting:alpha?}")
    ```

    > 🔎 **Observation** - The `Route` uses a route argument named `greeting` and it has an `alpha` constraint. This means that `greeting` may only contain characters from the alphabet (a-zA-Z). The question mark indicates the `greeting` parameter is optional. More info on route constraints in the [official Azure docs](https://docs.microsoft.com/aspnet/core/fundamentals/routing?view=aspnetcore-6.0#route-constraints).

    > 📝 **Tip** - If you want a less restrictive route that allows multiple words you can use `{greeting:regex(^[\\w\\s]*$)?}` instead.

3. Although a `greeting` parameter is now part of the route the function is still unaware of such parameter. It needs to be added to the function `Run()` method as a separate parameter:

    ```csharp
    string greeting,
    ```

4. Locate the `response.WriteStringAsync()` for the OK result and add the `greeting` parameter:

    *Change*

    ```csharp
    await response.WriteStringAsync($"Hello", {name}");
    ```

    *To*

    ```csharp
    await response.WriteStringAsync($"{greeting ?? "Hello"}, {name}");
    ```

    > 🔎 **Observation** - Here a null-coalescing operator is used to set the default value of `greeting` to `"Hello"` if it is null.

    The entire function should look like this:

    ```csharp
    [Function(nameof(HelloWorldHttpTrigger6))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "HelloWorldHttpTrigger6/{greeting:alpha?}")] 
        HttpRequestData req,
        string greeting)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string name = default;
        if (req.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
        {
            var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
            name = queryStringCollection["name"];
        }
        else
        {
            name = await req.ReadAsStringAsync();
        }

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (string.IsNullOrEmpty(name))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            await response.WriteStringAsync($"Please provide a value for the name query string parameter or in the body as plain text.");
        }
        else
        {
            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync($"{greeting ?? "Hello"}, {name}");
        }

        return response;
    }
    ```

5. Run the Function App.
    > 🔎 **Observation** - You should see the new HTTP endpoint (ending with `HelloWorldHttpTrigger6/{greeting:alpha?}`) in the output of the console.

6. Trigger the new endpoint by making a GET request to the following endpoint.

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger6/Hi?name=AzureFunctionsUniversity
    ```

    > ❔ **Question** - Is the outcome as expected?

    > ❔ **Question** - What happens when you remove the `greeting` parameter?

## 7. Homework

Ready to get hands-on? Checkout the [homework assignment for this lesson](http-homework-dotnet6.md).

## 8. More info

- For more info about the HTTP Trigger have a look at the official [Azure Functions HTTP Trigger](https://docs.microsoft.com/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=csharp) documentation.

- A brief overview [video](https://youtu.be/Wbw6MS5VoDo) by Gwyneth Pena

---
[🔼 Lessons Index](../../README.md)
 