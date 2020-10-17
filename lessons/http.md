# HTTP Trigger

## Goal 🎯

The goal of this lesson is to create your first function which can be triggered by doing an HTTP GET or POST to the function endpoint.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|1|Creating a Function App
|2|Changing the template for GET requests
|3|Changing the template for POST requests
---

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. Create the Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_). 

    > 📝 __Tip__ - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
4. Select `HTTPTrigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
    > 📝 __Tip__ - Namespaces are used to organize pieces of code into a hierarchy. Make sure you don't use the exact same name as the function/class. Namespaces and classes should be named uniquely to prevent compiler and readability issues.
7. Select `Function` for the AccesssRights.
    > 🔎 __Observation__ - Now a new Azure Functions project is generated and once it's done you should see the HTTPTrigger function in the code editor.

    > ❔ __Question__ - Review the template HTTPTrigger function. What is it doing?
8. Build the project (CTRL+SHIFT+B).

9. Start the Function App by pressing `F5`.
    > 🔎 __Observation__ - Eventually you should see an HTTP endpoint in the output.
10. Now call the function by making a GET request to the above endpoint using a REST client.

    > ❔ __Question__ - What is the result of the function? Is it what you expected?

## 2. Changing the template for GET requests

Let's change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed and parameter types can be changed.
Start with only allowing GET requests.

### Steps

1. Remove the `"post"` string from the `HttpTrigger` attribute. Now the function can only be triggered by a GET request.
    > 📝 __Tip__ - Some people don't like to use strings and prefer something that is known as _strong typing_. Strong typing can prevent you from making certain mistakes such as typos in strings since specific .NET types are used instead. To allow the function to be triggered by a GET request replace the `"get"` string with `nameof(HttpMethods.Get)`. Now you're using a strongly typed version of the HTTP GET verb instead of a string reference.
2. The `req` parameter type can also be changed. Try changing it from  `HttpRequest` to `HttpRequestMessage`. This requires a using of `System.Net.Http`.

    > 🔎 __Observation__ - You'll notice that this change breaks the code inside the function. This is because the `HttpRequestMessage` type has different properties and methods than the `HttpRequest` type.
3. Remove the content of the function method (but keep the method definition). We'll be writing a new implementation.
4. To get the name from the query string you can do the following:

    ```csharp
    var collection = req.RequestUri.ParseQueryString();
    string name = collection["name"];
    ```

    > 🔎 __Observation__ - In the generated template the response was always an `OkResultObject`. This means that when a clients calls the function, an HTTP status 200, is always returned. Let's make the function a bit smarter and return a `BadRequestObjectResult` (HTTP status 400).
5. Add an `if` statement to the function that checks if the name value is `null`. If the name is `null` return a `BadRequestObjectResult`, otherwise return a `OkResultObject`.

    ```csharp
    if(string.IsNullOrEmpty(name))
    {
        var responseMessage = "Pass a name in the query string or in the request body for a personalized response.";
        return new BadRequestObjectResult(responseMessage);
    }
    else
    {
        var responseMessage = $"Hello, {name}. This HTTP triggered function executed successfully.";
        return new OkObjectResult(responseMessage);
    }
    ```

    Now the function has proper return values for both correct and incorrect invocations.

6. Run the function, once without name value in the querystring, and once with a name value.

    > 🔎 __Observation__ - Is the outcome of the both runs as expected?

## 3. Changing the template for POST requests

Let's change the function to also allow POST requests and test it by passing .

### Steps

1. Add a new class file named `Person.cs` to the project.
2. Make sure the content of the file looks like this (adjust the namespace so it matches yours):

```csharp
namespace AzureFunctionsUniversity.Demo
{
    public class Person
    {
        public string Name { get; set; }
    }
}
```

TODO:

 - Read typed content
 - Use Person type as request type as alternative (as seperate function since the querystring is then no longer available).

## More info

See the [Azure Functons HTTP Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=csharp) documentation.
`

---
[◀ Previous lesson](prerequisites.md) | [🔼 Index](_index.md) | [Next lesson ▶](blob.md)
