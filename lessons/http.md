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

First, you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. Create the Function App by running `AzureFunctions: Create New Project` in the VSCode Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_). 

    > 📝 __Tip__ - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you'll be using to code the function, in this lesson we'll be using `C#`.
4. Select `HTTPTrigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Enter a namespace for the function (e.g. `AzureFunctionsUniversity.Demo`).
7. Select `Function` for the AccesssRights.

    > Now a new Azure Functions project is generated and once it's done you should see the HTTPTrigger function in the code editor.

    > ❔ __Question__ - Review the template HTTPTrigger function. What is it doing?
8. Build the project (CTRL+SHIFT+B).

9. Start the Function App by pressing `F5`.

    > Eventually you should see an HTTP endpoint in the output.
10. Now call the function by making a GET request to the above endpoint using a REST client.

    > ❔ __Question__ - What is the result of the function? Is it what you expected?

## 2. Changing the template for GET requests

Let's change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed and parameter types can be changed.
Start with only allowing GET requests.

### Steps

1. Remove both the `"get"` and `"post"` strings from the `HttpTrigger` attribute. To allow the function to be triggerd by a GET request add `nameof(HttpMethods.Get)` as the second argument in the attribute. Now you're using a strongly typed version of the HTTP GET verb instead of a string reference.
2. The `req` parameter type can also be changed. Try changing it from  `HttpRequest` to `HttpRequestMessage`. This requires a using of `System.Net.Http`.

    > You'll notice that this change breaks the code inside the function. This is because the `HttpRequestMessage` type has different properties and methods than the `HttpRequest` type.
3. Remove the content of the function method.
4. To get the name from the query string you can do the following:

```csharp
var collection = req.RequestUri.ParseQueryString();
string name = collection["name"];
```

TODO:
    - add if/else with BadRequestObject
    - run the function

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
[◀ Previous lesson](_index.md) | [🔼 Index](_index.md) | [Next lesson ▶](http.md)




