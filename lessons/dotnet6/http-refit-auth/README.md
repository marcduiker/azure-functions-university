# Advanced Scenarios using Refit (.NET 6)

## Goal 🎯

The goal of this lesson is to build upon the knowledge derived from the
[Calling third-party REST APIs (.NET 6)](../http-refit/README.md)
lesson and learn how to use the Refit library for more advanced scenarios.

In the lesson referred to above, you learned how to design a C# interface
that represents the service contract of a third-party API. Using Refit –
a type-safe library that automatically generates HTTP proxies based upon
interface specifications – you learned how to use dependency injection
to consume this interface from your Azure functions and make HTTP calls
to third-party APIs.

In this lesson, you will learn how to mock APIs as well as how to
add cross-cutting concerns – such as authentication – to your HTTP calls.

This lesson consists of the following exercises:

|Nr|Exercise
|-|-
|0|[Prerequisites](#0-prerequisites)
|1|[Creating a Refit-based HTTP-triggered function app](#1-creating-a-refit-based-http-triggered-function-app)
|2|[Mocking third-party APIs](#2-mocking-third-party-apis)
|3|[Mocking a hypothetical authentication server](#3-mocking-a-hypothetical-authentication-server)
|4|[Authenticating HTTP calls](#4-authenticating-http-calls)
|5|[Homework](#5-homework)
|6|[More info](#6-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/dotnet6/http-refit-auth/AzFuncUni.Http) in this repository.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| Azure Functions Core Tools | 1-4
| VS Code with Azure Functions extension| 1-4
| REST Client for VS Code or Postman | 1-4

See [.NET 6 prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a Refit-based HTTP-triggered function app

In this exercise, you'll be creating a Function App with the default HTTPTrigger to serve as a startup project for subsequent exercises.

This exercise is a condensed version of the 
[Calling third-party REST APIs (.NET 6)](../http-refit/README.md) lesson.

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

8. Select `Function` for the AccessRights.

9. Once the project is generated, add some boilerplate files to bootstrap the project.

     For illustration purposes will use [Httpbin.org](http://httpbin.org/) which hosts a basic public API specifically designed to test your HTTP clients. Its `POST /post` operations specifically will help you return information about the request.

    Create a file `Http/GetRequestResponse.cs` and paste the following content:

    ```c#
    using System.Text.Json.Serialization;
    
    public sealed class GetRequestResponse
    {
        public GetRequestResponse()
        {
            Args = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
    
        }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }
    ```

    The `GetRequestResponse` class represents a response from the
    [HttpBinOrg](http://httpbin.org/) API' `POST /post` route.
    The response includes the HTTP headers and the content of the
    original request in the `headers` and `data` properties respectively.

    Create a file `Http/IHttpBinOrgApi.cs` and paste the following content:
   
    ```c#
    using Refit;
    
    /// <summary>
    /// This interface represents access to the HttpBin.org API.
    /// </summary>
    public interface IHttpBinOrgApi
    {
        [Post("/post")]
        Task<GetRequestResponse> GetRequest(Stream content = null);
    }
    ```

    Make sure to add the required `Refit.HttpClientLibrary` NuGet package to the project
    using the following command from a terminal:

     ```pwsh
     dotnet add package Refit.HttpClientFactory
     ```

    This file defines a new `IHttpBinOrgApi` interface that represents a subset
    of the service contract to the HttpBinOrg API.

    At runtime, Refit will generate a proxy class that wraps an instance of the
    `HttpClient` class to make HTTP calls according to the interface specification.
    In particular, the `GetRequest` method's `Refit.PostAttribute` decoration
    instructs Refit to convert method invocations to HTTP POST requests against
    the `/post` route.

    Open the `Program.cs` file and replace its contents with the following code:

    ```c#
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    
    var builder = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices(ConfigureServices)
        ;
    
    var host = builder.Build();
    
    host.Run();
    
    const string HttpBinOrgApiHost = "http://httpbin.org";
    static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
    {
        services
            .AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
            .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c));
    }
    
    static void ConfigureHttpClient(IServiceProvider provider, HttpClient client)
    {
        client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    ```

    This code registers the `IHttpBinOrgApi` to the dependency management system.
    Any class whose constructor accepts an `IHttpBinOrgApi` parameter will receive
    an instance of a Refit-generated proxy class that is configured to make
    HTTP calls to the `http:/httpbin.org` endpoint and accepting JSON responses.
 
    Finally, open the `HelloWorldHttpTrigger.cs` file and replace its contents
    with the following code:

    ```c#
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    
    public class HelloWorldHttpTrigger
    {
        private readonly IHttpBinOrgApi _client;
        private readonly ILogger _logger;
    
        public HelloWorldHttpTrigger(
            IHttpBinOrgApi client,
            ILoggerFactory loggerFactory
        )
        {
            _client = client;
            _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger>();
        }
    
        [Function(nameof(HelloWorldHttpTrigger))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
        )
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
    
            try
            {
                var result = await _client.GetRequest(req.Body);
                await response.WriteAsJsonAsync(result);
            }
            catch (Refit.ApiException e)
            {
                response.StatusCode = e.StatusCode;
                response.Headers.Add("Content-Type", "text/plain");
                await response.WriteStringAsync(e.Message);
            }
    
            return response;
        }
    }
    ```

    This code defines an HTTP-triggered function that accepts and relays
    an incoming HTTP request to the third-party HttpBinOrg API. It then
    retrieves the response from the API and relays the response to the caller.

    If an error occurs, Refit will raise a `Refit.ApiException` so this must
    be caught and dealt with accordingly.

10. Build the project (CTRL+SHIFT+B).

11. Run the Function App by pressing `F5`.

    > 🔎 **Observation** - Eventually make sure you see a local HTTP endpoint in the output. Ensure you can call the function by making a POST request to the above endpoint using a REST client:

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

## 2. Mocking third-party APIs

In this exercise, you’ll see how to prevent making actual HTTP calls over the network
by mocking a third-party API.

### Overview

Mocking an API can sometimes be useful when the target API is not available.
For instance, the API may be in the process of being implemented by a separate team
and you do not want to slow down your developers responsible for consuming the API.

In the `Program.cs` code from the previous exercise, the `AddHttpClient` method
registers an `IHttpClientFactory` that configures and creates an instance of the
well-known `HttpClient` class.

`IHttpClientFactory` also surfaces the concept of _outgoing middleware_ by organizing a pipeline of
delegating handlers that you can chain to define arbitrary processing logic invoked as part of the HTTP request.

Those handlers allow your code to inspect, route or otherwise modify the request
and the response messages as required by your application.
This is also a good way to implement cross-cutting concerns, such as logging, or
– as we shall see a bit later – authentication.

Each step in the outgoing middleware pipeline is a `DelegatingHandler` that
receives the request and produces a response. Typically, a delegating handler will
_delegate_ part of its processing to the next delegating handler in the chain.

Ultimately, the _primary handler_ – as the last handler from the chain – is
responsible for sending the HTTP request to the target API and receiving the
response.

By default, the outgoing middleware pipeline consists of the primary handler only.

#### Cross-cutting concerns

Using delegating handlers is also a good way to implement cross-cutting
concerns, such as logging, for instance. A typical implementation of
a `DelegatingHandler` looks like so:

```c#
public sealed class MockingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        // perform custom processing logic
        // on the request message
        …

        // invoke the next delegating handler in the chain
        // and receives the response

        var response = base.SendAsync(request, cancellationToken);

        // perform custom processing logic
        // on the response message
        …

        return response;
    }
}
```

A delegating handler can inspect or modify both the incoming request
and outgoing response messages.

For instance, logging consists in reading the request or the response
and writing log messages accordingly, such as the HTTP verb used to
make the call, the request URI, the request headers, the response
status code, and, if necessary, the request and response body.

Authentication can be implemented by adding an appropriate
`Authorization` header to the request before calling the next
delegating handler in the chain as we will see in a next exercise.

Using dedicated libraries such as [Polly](https://github.com/App-vNext/Polly),
you can implement resilient patterns using delegating handlers,
such as transient fault handling, retry mechanisms, throttling, caching
and many more.

#### Mocking API calls

In order to mock a third-party API one has to register a delegating handler earlier
in the chain – before a request has a chance to hit the primary handler – that
**short circuits** the chain by **not** delegating its processing to the next link
in the chain.

Consider the following implementation of a `DelegatingHandler`:

```c#
public sealed class MockingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        // perform custom processing logic

        …

        // by *NOT* proceeding any further
        // we will short-circuit the pipeline
        // HTTP request will *NOT* be sent over the wire

        // return base.SendAsync(request, cancellationToken);
    }
}
```

The call to `base.SendAsync(request, cancellationToken)` is commented-out
to prevent the next link in the chain to be invoked. That effectively
short-circuits the pipeline. As a result the HTTP request will **not** be
automatically sent to the target API.

### Steps

1. Create a new file `Http/MockedUnauthorizedHandler.cs` and paste the following content:

    ```c#
    using System.Net;
    using System.Net.Http.Headers;
    
    public sealed class MockedUnauthorizedHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorization = request.Headers?.Authorization ?? new AuthenticationHeaderValue("Bearer");
            if (String.IsNullOrWhiteSpace(authorization.Parameter))
            {
                var unauthorized = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = new(),
                };
    
                return Task.FromResult(unauthorized);
            }
    
            return base.SendAsync(request, cancellationToken);
        }
    }
    ```
    
    This is a `DelegatingHandler` that ensures that the incoming HTTP call is
    properly authenticated. For the purpose of this exercise, a property 
    authenticated HTTP call is a request whose `Authorization` header contains
    a bearer token.
    
    The delegating handler receives a request message and inspects its
    `Authorization` header. If that header does not exist or does not contain a token,
    the delegating handler returns an `401` HTTP error response.
    
    Notice that if the HTTP request is not authenticated, the handler
    effectively **short circuits** the remainder of the pipeline and returns
    a response without calling the next handler in the chain.
    Thus, the request is never sent to the target in this case.
    
    This is consistent with the pattern that was shown
    about mocking API calls in the previous section.

2. Register the handler in the corresponding outgoing middleware.

    Open the `Program.cs` file and update the `ConfigureServices()` method.

    *Replace*

    ```c#
    services
        .AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c));
    ```

    *With*

    ```c#
    services
        .AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c))
        .AddHttpMessageHandler<MockedUnauthorizedHandler>();

    services.AddTransient<MockedUnauthorizedHandler>();
    ```

    This registers the `MockedUnauthorizedHandler` delegating handler
    as a new stage in the pipeline to ensure HTTP calls are property
    authenticated.

    > 🔎 **Observation** - Delegating handlers need to be registered
    to the outgoing middleware pipeline **and** registered to the
    dependency management system.
    
    > 📝 **Tip** - This exercise simulates receiving an
    unauthorized response message if HTTP calls are not properly
    authenticated. In a real-world scenario, the target API would
    return an unauthorized response itself. However, since HttpBinOrg
    does not require authentication, you need to handle this yourselves.

3. Run the project and invoke the HTTP endpoint.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

    > 🔎 **Observation** - You should receive a `401 Unauthorized` HTTP error.
    
## 3. Mocking a hypothetical authentication server

In this exercise you will mock a hypothetical authentication server by applying
the pattern you have just seen again.

### Steps

1. Design an interface that represents a hypothetical authentication server.

    Create a couple of plain-old C# object (POCO) classes that represent
    a request and a response to an authentication server respectively.

    Create a file `Http/GetAccessToken.cs` and paste the following code:

    ```c#
    using System.Text.Json.Serialization;
    
    public sealed class GetAccessTokenRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = default!;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = default!;
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "client_credentials";
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }
    }
    
    public sealed class GetAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;
    }
    ```

    Those classes use the builtin `System.Text.Json` NuGet package that is already
    available to your code. It drives the serialization of the strongly-typed C#
    object to the JSON representation used when issueing HTTP calls to the
    authentication server.

    Create a file `Http/IAuthentication.cs` and paste the following code:

    ```c#
    using Refit;
    public interface IAuthentication
    {
    
        [Post("/oauth/token")]
        Task<GetAccessTokenResponse> GetAccessToken([Body(BodySerializationMethod.UrlEncoded)] GetAccessTokenRequest request);
    }
    ```

    This interface defines an API that exposes a `/oauth/token` route and expects
    parameters commonly found when requesting an access token. Those parameters are
    defined in the `GetAccessTokenRequest` class and are converted to a
    `x-www-form-urlencoded` form when sent as an HTTP call by Refit.

    In response the `/oauth/token` route returns a JSON object that contains an
    access token property, as defined in the `GetAccessTokenResponse` class.
    

2. Registers the authentication interface to the dependency management system.

    Open the `Program.cs` file and add a new set of instructions to the
    `ConfigureServices` method to register the `IAuthentication` interface
    as a Refit-generated proxy.

    ```c#
    services
        .AddHttpClient(nameof(IAuthentication), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IAuthentication>(c));
    ```

3. Mock the authentication server.

    Since there is no actual `oauth/token` route defined on the HttpBinOrg API,
    you need to mock this endpoint using a delegating handler.

    Create a file `Http/MockedAuthenticationServerHandler.cs` file and paste the following code:

    ```c#
    using System.Text;
    using System.Text.Json;
    
    public sealed class MockedAuthenticationServerHandler : DelegatingHandler
    {
        private const string token_ = "eyJhbGciOiJoczI1NiIsInR5cCI6ICJKV1QifQ.eyJzdWIiOiJtZSJ9.signature";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenResponse = new GetAccessTokenResponse
            {
                AccessToken = token_,
            };
    
            var tokenResponseJson = JsonSerializer.Serialize(tokenResponse);
    
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(tokenResponseJson, Encoding.UTF8, "application/json");
    
            return Task.FromResult(response);
    
            // by *NOT* proceeding any further
            // we will short-circuit the pipeline
            // HTTP request will *NOT* be sent over the wire
    
            // return base.SendAsync(request, cancellationToken);
        }
    }
    ```

    This delegating handler accepts an incoming HTTP request and 
    produces a hard-coded response containing a fixed access token
    for test purposes.

    > 🔎 **Observation** - Notice that this delegating handler **does not**
    call the next handler in the chain, thus **short circuiting** the pipeline.
    This is consistent with the pattern for mocking API calls described earlier
    in this document.

4. Register the mocked authentication handler to the pipeline.

    Open `Program.cs` and add the necessary instructions to the
    `ConfigureServices` method.

    First register the `MockedAuthenticationServerHandler` class in the
    outgoing middleware pipeline of the `IAuthentication` client:

    ```c#
    services
        .AddHttpClient(nameof(IAuthentication), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IAuthentication>(c))
        .AddHttpMessageHandler<MockedAuthenticationServerHandler>();
    ```

    Then, register the `MockedAuthenticationServerHandler` class 
    to the dependency management system:

    ```c#
    services.AddTransient<MockedAuthenticationServerHandler>();
    ```

5. Run and test the function.

    > 🔎 **Observation** - No real change was made to the externally
    facing HTTP endpoint in this exercise. Please, make sure that
    you did not introduce any regressions. Calling the HTTP endpoint
    must still return a `401 Unauthorized` response.

## 4. Authenticating HTTP calls

In this exercise will learn how to implement custom
authentication when sending HTTP requests.

### Overview

Authenticating HTTP call takes many forms but the most common way usually
involves adding a specific value in the `Authorization` header.

Therefore the most basic way to authenticate an HTTP call is to use a
delegating handler in the pipeline to inject an appropriate value for the
authorization header.

Consider the following code, assuming that you must use a bearer token
for authentication:

```c#
public sealed class AuthenticationHandler : DelegatingHandler
{
    private readonly IRequestToken _requestToken;

    public AuthenticationHandler(IRequestToken requestToken)
    {
        _requestToken = requestToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var tokenResponse = await _requestToken.GetAccessToken();
        if (tokenResponse != null)
        {
            var accessToken = tokenResponse.AccessToken;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
```

Given an hypothetical `IRequestToken` that supports retrieving an access token,
this code will be called for each request. It will inject the access token
value in the incoming request’s `Authorization` HTTP header.

Obviously, `IRequestToken` could implement some caching mechanism to avoid
performing an HTTP call to retrieve an access token on each request.
In more elaborate cases, it could also handle OAuth2 refresh tokens automatically.

Because `IRequestToken` is an interface that gets injected in the delegating handler’s
constructor at runtime, there’s nothing that prevents you from using a Refit-generated
proxy in the actual implementation as we have now seen in the first part of this lesson
and in the
[Calling Third-Party REST APIs (.NET 6)](../http-refit/README.md) lesson.

That is what you will learn to doing in this exercise.

### Steps

1. Create an abstraction over retrieving access token.

    Although you may want to use `IAuthentication` directly in the implementation
    of the authentication handler you still need to specify parameters that will
    actually be supplied to its `GetAccessToken()` method.

    Besides, as alluded to above, you might want to introduce some mechanism to
    refresh the token automatically upon expiration or introduce custom caching
    to prevent requesting an access token for every call.

    Create a new file `Http/RequestToken.cs` and paste the following code:

    ```c#
    public interface IRequestToken
    {
        Task<GetAccessTokenResponse> GetAccessToken();
    }
    public sealed class RequestToken : IRequestToken
    {
        private readonly IAuthentication _client;
        private readonly GetAccessTokenRequest _credentials;
        public RequestToken(IAuthentication client, GetAccessTokenRequest credentials)
        {
            _client = client;
            _credentials = credentials;
        }
        public Task<GetAccessTokenResponse> GetAccessToken()
        {
            // this requests a token on every call
            // implement caching for more performance
    
            return _client.GetAccessToken(_credentials);
        }
    }
    ```

    The `RequestToken` class is currently a thin layer over `IAuthentication`.
    It will retrieve a new access token on every request. That is where
    you can implement custom logic to improve performance.

2. Implement a `DelegatingHandler` for authentication purposes.

    Create a file `Http/AuthenticationHandler.cs` and paste the following code:

    ```c#
    using System.Net.Http.Headers;
    
    public sealed class AuthenticationHandler : DelegatingHandler
    {
        private readonly IRequestToken _requestToken;
    
        public AuthenticationHandler(IRequestToken requestToken)
        {
            _requestToken = requestToken;
        }
    
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            var tokenResponse = await _requestToken.GetAccessToken();
            if (tokenResponse != null)
            {
                var accessToken = tokenResponse.AccessToken;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
    
            return await base.SendAsync(request, cancellationToken);
        }
    }
    ```

    > 🔎 **Observation** - Notice that this is the exact same code
    as the one used to illustrate the authentication mechanism.

3. Register the authentication handler to the HttpBinOrg middleware.

    Open `Program.cs` and add an instruction to register the
    `AuthenticationHandler` class to the outgoing middleware
    associated with the `IHttpBinOrgApi` HTTP client.

    *Replace*

    ```c#
    services
        .AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c))
        .AddHttpMessageHandler<MockedUnauthorizedHandler>();
    ```

    *With*

    ```c#
    services
        .AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
        .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c))
        .AddHttpMessageHandler<AuthenticationHandler>()
        .AddHttpMessageHandler<MockedUnauthorizedHandler>();
    ```

    > 🔎 **Observation** - The order in which the delegating
    handlers are specified in a pipeline is important. Notice
    how `AuthenticationHandler` is registered **before** the
    `MockedUnauthorizedHandler`. This gives the former a chance
    to inject the `Authorization` HTTP header, before the latter
    gets to check that the call is properly authenticated.

    Do not forget to also register the `AuthenticationHandler`
    class to the dependency management system as well:

    ```c#
    services.AddTransient<AuthenticationHandler>();
    ```

4. Register support classes to the dependency management system.

    The `AuthenticationHandler` class depends on an implementation
    of the `IRequestToken` interface. Any implementation will do.
    You have already declared the `RequestToken` class a its
    first implementation.

    ```c#
    services.AddTransient<IRequestToken, RequestToken>();
    ```
    > 📝 **Tip** - This registers the `IRequestToken` interface
    in the dependency management system and maps its implementation
    to the `RequestToken` class. When instantiating a class that
    expects an `IRequestToken` constructor parameter, the dependency
    management system will automatically supply a new instance of the
    `RequestToken` class.

    > 🔎 **Observation** - Only the `IRequestToken` interface
    is actually registered in the dependency management system.
    If a class expects a `RequestToken` constructor parameter,
    the dependency management system will be unable to find an
    corresponding registration in its configuration.

    The `RequestToken` class accepts an `IAuthentication`
    constructor parameter whose Refit-generated proxy implementation
    is already registered to the dependency management system.

    It also expects a `GetAccessTokenRequest` constructor parameter.
    Since the parameters need not change, they could be retrieved
    from application settings or from a secret vault service.

    Add the following code to the `ConfigureServices` method:

    ```c#
    services.AddSingleton(
        new GetAccessTokenRequest()
        {
            ClientId = "please-include-client-id-here",
            ClientSecret = "please-retrieve-client-secret-from-application-settings",
            Resource = HttpBinOrgApiHost
        }
    );
    ```
    > 🔎 **Observation** - The `GetRequestToken` is registered
    to the dependency management system as a _singleton_ instance.
    This means that when a new instance of the `AuthenticationHandler`
    class is created, its will always receive the same instance
    of the `GetRequestToken` class.

6. Test your changes.

    You made extensive changes to the code since in the last two exercises.
    Run the function app and invoke the HTTP endpoint to test its behaviour.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: text/plain

    AzureFunctionsUniversity
    ```

    > 🔎 **Observation** - Make sure you receive a `200 OK` success response.

## 5. Homework

Deploy the function to Azure and test that it behaves as you would expect.

> 📝 **Tip** - Once deployed to Azure, the function endpoint is now `https` and a mandatory *function key* must be specified as a query string parameter. Please, make sure to update your HTTP requests accordingly.

## 6. More info

- [Use IHttpClientFactory to implement resilient HTTP requests](https://docs.microsoft.com/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

- [Refit - the automatic type-safe REST library for .NET Core, Xamarin and .NET](https://github.com/reactiveui/refit)

- [Polly - a .NET resilience and transient-fault-handling library](https://github.com/App-vNext/Polly)

---
[🔼 Lessons Index](../../README.md)
