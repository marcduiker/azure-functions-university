# Calling third-party REST APIs (.NET 6)

Watch the recording of this lesson [on YouTube 🎥](https://youtu.be/11Qi8A_8cVY).


## Goal 🎯

The goal of this lesson is to learn how to call third-party REST APIs from your functions using dependency injection and Refit, a type-safe REST library.

Calling REST APIs usually involves the well-known [HttpClient](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient?view=net-6.0) .NET class. However, using this class effectively has always been a challenge as it is designed to be instantiated once and reused throughout the life of your application.

Starting from .NET Core 2.1, the base class library [introduced a set of changes](https://docs.microsoft.com/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) designed to make `HttpClient` easier to use in a correct and efficient manner.

This lesson consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Creating a default HTTP-triggered function app](#1-creating-a-default-http-triggered-function-app)
|2|[Defining a third-party REST API](#2-defining-a-third-party-rest-api)
|3|[Adding custom API parameters](#3-adding-custom-api-parameters)
|4|[Homework](#4-homework)
|5|[More info](#5-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/dotnet6/http-refit/AzFuncUni.Http) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| Azure Functions Core Tools | 1-3
| VS Code with Azure Functions extension| 1-3
| REST Client for VS Code or Postman | 1-3

See [.NET 6 prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a default HTTP-triggered function app

In this exercise, you'll be creating a Function App with the default HTTPTrigger to serve as a startup project for subsequent exercises.

This exercise is very similar to exercise [1. Creating a Function App](../http/README.md#1-creating-a-function-app) from the [HTTP Trigger (.NET 6)](../http/README.md).

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).

2. Browse to the location where you want to save the function app (e.g. *AzureFunctions.Http*).

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

9. Build the project (CTRL+SHIFT+B).

10. Run the Function App by pressing `F5`.

    > 🔎 **Observation** - Eventually make sure you see a local HTTP endpoint in the output. Ensure you can call the function by making a GET request to the above endpoint using a REST client:

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger
    ```

## 2. Defining a third-party REST API

In this exercise, you’ll declare a new interface that represents the *service contract* of a third-party API. You will also enable dependency injection to allow the Azure Functions runtime to automatically supply instances of this interface to your function.

### Overview

To make it easy for you to test third-party APIs, you will use [Httpbin.org](http://httpbin.org/) which hosts a basic public API specifically designed to test your HTTP clients. Specifically, its `POST /post` and `GET /status` operations will help you inspect HTTP queries and test various success or failure conditions.

For instance, using the `GET /status` operation allows to control the HTTP response code of a fictitious third-party API. Please, ensure the following call returns a `200` success code.

```http
GET http://httpbin.org/status/200
```

> 🔎 **Observation** - Note that `200` was specified as the requested response code. Try and change the requested response code and see the corresponding outcome. For instance, ensure that specifying a `404` status code does indeed produce a failure with a `404 Not Found` error.

Likewise, calling the `POST /post` route returns information about the request.  

```http
POST http://httpbin.org/post?hello=world!
Content-Type: text/plain

This is a plain-text content.
```

> 🔎 **Observation** - Note that the response is a JSON object. In particular, please note that its `args` property contains the parsed query string; its `data` property contains the contents of the HTTP request; and the `headers` property contains the HTTP request headers.

### Steps

1. Add the [Refit.HttpClientFactory](https://www.nuget.org/packages/Refit.HttpClientFactory/) package to the project.

    ```pwsh
    dotnet add package Refit.HttpClientFactory
    ```

    > 📝 **Tip** Refit is a class library that automatically generates HTTP proxies to call third-party REST APIs based upon interface specifications. The generated proxies internally use the [HttpClient](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient?view=net-6.0) .NET class to make its calls.

2. Create a new file named `Http/IHttpBinOrgApi.cs` and add the following code:

    ```csharp
    using Refit;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    /// <summary>
    /// This interface represents access to the HttpBin.org API.
    /// </summary>
    public interface IHttpBinOrgApi
    {
        [Get("/status/{code}")]
        Task<HttpContent> StatusCodes(int code);
    
        [Post("/post")]
        Task<HttpContent> GetRequest();
    }
    ```

    > 🔎 **Observation** - The [Httpbin.org](http://httpbin.org/) API defines a complete set of operations. In this exercise, we only surface a couple of operations. The resulting interface can be expanded further as you need more operations.

3. In `Program.cs`, let’s add a constant to hold the [Httpbin.org](http://httpbin.org/) API endpoint:

    ```csharp
    private const string HttpBinOrgApiHost = "http://httpbin.org";
    ```

4. In `Program.cs`, add the following code:

    ```csharp
    private static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
    {
        services
            .AddHttpClient("HttpBinOrgApi", (provider, client) =>
            {
                client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c));
    }
    ```

    This configures the Refit-proxy generation and enables dependency injection.

    > 📝 **Tip** The `AddHttpClient` call configures a *named* client with a base address and default HTTP headers. This allows your code to grab an instance of the `IHttpClientFactory` class and create a new instance of `HttpClient` based upon those specifications. Additionally, the `AddTypedClient` is a Refit-specific method that turns this configured `HttpClient` in a REST proxy using the strongly-typed `IHttpBinOrgApi` specification. This interface is then automatically registered into the dependency management system.

5. The `RestService` class lives in the `Refit` namespace, so add this to the using directives at the top of the file:

    ```csharp
    using Refit;
    ```

6. In `Program.cs`, call the new code in the `Main` method:

    *Replace*

    ```csharp
    var host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .Build();
    ```

    *With*

    ```csharp
    var builder = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices(ConfigureServices);

    var host = builder.Build();
    ```

7. In `HelloWorldHttpTrigger.cs`, add a new parameter to the constructor to inject the `IHttpBinOrgApi` interface. Assign the received interface to a new class field accordingly.

    ```csharp
    private readonly ILogger _logger;
    private readonly IHttpBinOrgApi _client;

    public HelloWorldHttpTrigger(
        ILoggerFactory loggerFactory,
        IHttpBinOrgApi client
    )
    {
        _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger>();
        _client = client;
    }
    ```

8. In `HelloWorldHttpTrigger.cs`, replace the contents of the function to relay the request to the [Httpbin.org](http://httpbin.org/) API and return the response to the caller.

    The final code for the function should look like:

    ```csharp
    [Function(nameof(HelloWorldHttpTrigger))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);

        try
        {
            var content = await _client.GetRequest();
            var text = await content.ReadAsStringAsync();

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(text);
        }
        catch (Refit.ApiException e)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(e.Message);
        }

        return response;
    }
    ```

    > 🔎 **Observation** - The current `IHttpBinOrgApi.GetRequest()` method returns an `HttpContent` that allows you to access the HTTP response headers as well as its body. Please, note that Refit throws an instance of a `Refit.ApiException` exception when an error occurs.

9. Run the Function App.

10. Trigger the endpoint by making a POST request and submitting a plain-text `name` content.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

    > ❔ Question - Is the outcome as expected?

    > ❔ Question - Experiment by specifying a query string `name` parameter instead. Is the outcome as you would expect?

## 3. Adding custom API parameters

In the previous exercise, you called a third-party REST API hosted by [Httpbin.org](http://httpbin.org/). However, neither the `name` query string parameter nor the contents of the HTTP request were relayed to the third-party API.

Additionally the return type of the `IHttpBinOrgApi.GetRequest()` method was `HttpContent`. This allows to retrieve some custom HTTP response headers but is not often useful in practice. As a better practice, it is recommended you return strongly-typed objects from API methods.

The [Httpbin.org](http://httpbin.org/) API’s `POST /post` operation also accepts an arbitrary body content as well as any number of arbitrary query string parameters.

In this exercise, you will change the `IHttpBinOrgApi` interface to enable custom API parameters and define a strongly-typed return value.

### Steps

1. Create a new file named `Http/GetRequestResponse.cs` and add the following code:

    ```csharp
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    
    public sealed class GetRequestResponse
    {
        public GetRequestResponse()
        {
            Args = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
    
        }
        [JsonPropertyName("args")]
        public Dictionary<string, string> Args { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }
    ```

    > 🔎 **Observation** - This object represents a subset of the expected HTTP response. You will recognize the `args`, `data` and `headers` properties discussed earlier. Please note that the names for these properties are specified using the `JsonPropertyName` attribute from the builtin [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/) package. This package is available to your function automatically and you do not need to install it separately.

2. The [Httpbin.org](http://httpbin.org/) API’s `POST /post` operation currently returns a generic `HttpContent` object. Change the return-type to the `GetRequestResponse` type that you have declared in the previous step.

    ```csharp
    [Post("/post")]
    Task<GetRequestResponse> GetRequest();
    ```

3. In `HelloWorldHttpTrigger.cs` change the code to handle the new strongly-typed object.

    *Replace*

    ```csharp
    var content = await _client.GetRequest();
    var text = await content.ReadAsStringAsync();

    response.Headers.Add("Content-Type", "application/json; charset=utf-8");
    await response.WriteStringAsync(text);
    ```

    *With*

    ```csharp
    var result = await _client.GetRequest();
    await response.WriteAsJsonAsync(result);
    ```

> 📝 **Tip** The `WriteAsJsonAsync` method will automatically set the value for the HTTP `Content-Type` header. Therefore, you must not set it explicitly.

4. The [Httpbin.org](http://httpbin.org/) API’s `POST /post` operation accepts an arbitrary body content and any number of arbitrary query string parameters. The query string parameters can be modelled as a `IDictionary<string, string>`.

    In `Http/IHttpBinOrgApi.cs` add a new parameter to the `GetRequest()` method. Its complete declaration should look like:

    ```csharp
    [Post("/post")]
    Task<GetRequestResponse> GetRequest([Query] IDictionary<string, string> query = default);
    ```

    The `IDictionary` class lives in the `System.Collections.Generic` namespace, so add this to the using directives at the top of the file:

    ```csharp
    using System.Collections.Generic;
    ```

> 📝 **Tip** The `[Query]` attribute instructs `Refit` to interpret the corresponding parameter – here a dictionary – as the query string parameters when making an HTTP request. Because query strings are optional, the method defines a default value for the dictionary for the case where it is not specified by the caller.

5. Create a new file `Extensions/NameValueCollections.cs` and add the following code:

    ```csharp
    using System.Collections.Generic;
    using System.Collections.Specialized;
    
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        ///     A NameValueCollection extension method that converts the collection to a dictionary.
        /// </summary>
        /// <param name="this">The collection to act on.</param>
        /// <returns>collection as an IDictionary&lt;string,string&gt;</returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var dict = new Dictionary<string, string>();
    
            foreach (string key in collection.AllKeys)
            {
                dict.Add(key, collection[key]);
            }
    
            return dict;
        }
    }
    ```

> 📝 **Tip** The `NameValueCollection.ToDictionary()` method is an *extension method*. An *extension method* exposes additional methods to an existing type without defining a derived class, recompiling or otherwise modifying the original class. Notice that both the method and its enclosing class are static. Notice also that the first parameter has a special `this` specifier that allows you to call this method as if it originally belonged to the `NameValueCollection` type itself as demonstrated by the following change.

6. In `HelloWorldHttpTrigger.cs`, add code to retrieve the query strings from the incoming HTTP request and convert those to a dictionary. Recall from the previous lesson that you can retrieve the query strings on the `HttpRequestData` object using the `HttpUtility.ParseQueryString()` method from the `System.Web` namespace.

    The `System.Web` namespace declaration should have already been included at the top of the file from the previous lesson. Please, make sure it is indeed specified:

    ```csharp
    using System.Web;
    ```

    Add code to the top of the function code in the `Run` method:

    ```csharp
    var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
    var queryStrings = queryStringCollection.ToDictionary();
    ```

    Finally, add this parameter to the invocation of the `GetRequest()` method:

    ```csharp
    var result = await _client.GetRequest(query: queryStrings);
    ```

    > 🔎 **Observation** - Notice that the statement `queryStringCollection.ToDictionary()` is taking advantage of the `ToDictionary()` *extension method* defined earlier in the `NameValueCollectionExtensions` class.

7. Run the Function App.

8. Trigger the endpoint by making a POST request and specifying a `name` query string parameter.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger?name=AzureFunctionsUniversity
    ```

    > 🔎 **Observation** - You should now see that the contents of the expected HTTP response has its `args` property set to a JSON object.

9. Likewise, the [Httpbin.org](http://httpbin.org/) API’s `POST /post` operation also accepts any arbitrary HTTP body. Add a `Stream` parameter to the `IHttpBinOrgApi.GetRequest()` method to support this scenario.

    ```csharp
    [Post("/post")]
    Task<GetRequestResponse> GetRequest(Stream content = null, [Query] IDictionary<string, string> query = default);
    ```

10. The `Stream` type is available in the `System.IO` namespace. Make sure to add a using directive to the top of the file.

    ```csharp
    using System.IO;
    ```

12. In `HelloWorldHttpTrigger.cs` relay the contents of the incoming HTTP request to the [Httpbin.org](http://httpbin.org/) API.

    ```csharp
    var result = _client.GetRequest(req.Body, query: queryStrings);
    ```

13. Run the Function App.

14. Trigger the endpoint by making a POST request and either submit a plain-text `name` in the body content or use the `name` query string parameter.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

    or

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger?name=World!
    ```

## 4. Homework

Deploy the function to Azure and test that it behaves as you would expect.

> 📝 **Tip** - Once deployed to Azure, the function endpoint is now `https` and a mandatory *function key* must be specified as a query string parameter. Please, make sure to update your HTTP requests accordingly.

## 5. More info

- [Use IHttpClientFactory to implement resilient HTTP requests](https://docs.microsoft.com/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

- [Refit - the automatic type-safe REST library for .NET Core, Xamarin and .NET](https://github.com/reactiveui/refit)

---
[🔼 Lessons Index](../../README.md)
