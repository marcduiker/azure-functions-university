# HTTP Trigger (PowerShell)

## Goal 🎯

The goal of this lesson is to create your first function which can be triggered by doing an HTTP GET or POST to the function endpoint.

This lessons consists of the following exercises:

|Nr|Exercise
|-|-
|1|[Creating a Function App](#1-creating-a-function-app)
|2|[Changing the template for GET requests](#2-changing-the-template-for-get-requests)
|3|[Changing the template for POST requests](#3-changing-the-template-for-post-requests)
|4|[Adding a new function for POST requests](#4-adding-a-new-function-for-post-requests)
|5|[Change the route for a custom greeting](#5-change-the-route-for-a-custom-greeting)
|6|[Homework](#6-homework)
|7|[More info](#7-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/dotnet/AzureFunctions.Http) in this repository.

---

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger and review the generated code.

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. _AzureFunctions.Http_).

    > 📝 **Tip** - Create a folder with a descriptive name since that will be used as the name for the project.

3. Select the language you'll be using to code the function, in this lesson we'll be using `PowerShell`.
4. Select `HTTPTrigger` as the template.
5. Give the function a name (e.g. `HelloWorldHttpTrigger`).
6. Select `Function` for the AccessRights.
    > 🔎 **Observation** - Now a new Azure Functions project is being generated. Once it's done, look at the files in the project. You will see the following:

    |File|Description
    |-|-
    |host.json|Contains [global configuration options](https://docs.microsoft.com/en-us/azure/azure-functions/functions-host-json) for all the functions in a function app.
    |profile.ps1| A profile file, like you would have in a local PowerShell prompt. Here you can store all cmdlets that need to be executed when the function [cold starts](https://azure.microsoft.com/en-us/blog/understanding-serverless-cold-start/).
    |proxies.json| Can be used if the Function app is part of a larger API, is not used for standalone Functions. [More information](https://docs.microsoft.com/en-us/azure/azure-functions/functions-proxies)
    |requirements.psd1| Define modules in the PowerShell gallery that need to be loaded when the function starts.
    |local.settings.json|Contains [app settings and connectionstrings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#local-settings-file) for local development.
    |**HelloWorldHttpTrigger**|
    |function.json| The trigger and binding settings of the Function
    |run.ps1| The actual PowerShell script that will run when the function is triggered
    |sample.dat| and example body input for the function http trigger.

    > ❔ **Question** - Review the generated HTTPTrigger function. What is it doing?
7. Start the Function App by pressing `F5`.
    > 🔎 **Observation** - Eventually you should see an HTTP endpoint in the output.
8. Now call the function by opening a PowerShell prompt outside of Visual Studio Code and using Invoke-RestMethod:

    ```PowerShell
     Invoke-RestMethod http://localhost:7071/api/HelloWorldHttpTrigger?name=YourName
    ```

    >📝 **Tip** - In Invoke-RestMethod, you can choose not to define a `Method`. If you don't, it will always use `GET`.

    > 🔎 **Observation** - We use Invoke-RestMethod, but you could also use Curl or just your web browser!

    > ❔ **Question** - What is the result of the function? Is it what you expected?

    > ❔ **Question** - What happens when you don't supply a value for the name?

## 2. Changing the template for GET requests

Let's change the template to find out what parameters can be changed.
Depending on the trigger, arguments can be added/removed and parameter types can be changed.
Start with only allowing GET requests.

### Steps

1. Open function.json. In the httpTrigger binding, the `methods` are listed. Remove `"post"`. Don't forget to remove the comma after `"get"`. Now the function can only be triggered by a GET request.

    > 🔎 **Observation** - You'll notice that this change breaks the code inside the function. This is because the `HttpRequestMessage` type has different properties and methods than the `HttpRequest` type.

2. To get the name from the query string you can do the following:

    ```PowerShell
    $name = $Request.Query.Name
    ```

    > 🔎 **Observation** - In the generated template the response was always an `[HttpStatusCode]::OK`. This means that when a clients calls the function, an HTTP status 200, is always returned. Let's make the function a bit smarter and return a `[HttpStatusCode]::BadRequest` (HTTP status 400).
3. Add an `if` statement to the function that checks if the name value is `$null`. If the name is `$null` return a `[HttpStatusCode]::BadRequest`, otherwise return a `[HttpStatusCode]::OK`. The complete code should now look like this.

    ```PowerShell
    using namespace System.Net
    
    # Input bindings are passed in via param block.
    param($Request, $TriggerMetadata)
    
    # Write to the Azure Functions log stream.
    Write-Host "PowerShell HTTP trigger function processed a request."
    
    # Interact with query parameters or the body of the request.
    $Name = $Request.Query.Name
    
    if ([string]::IsNullOrEmpty($name)) {
        $Body = "Pass a name in the query string or in the request body for a personalized response."
        $Result = [HttpStatusCode]::BadRequest
    }
    else{
        $Body = "Hello, $Name. This HTTP triggered function executed successfully."
        $Result = [HttpStatusCode]::OK
    }
    
    # Associate values to output bindings by calling 'Push-OutputBinding'.
    Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
        StatusCode = $Result
        Body = $Body
    })

    ```

    Now the function has proper return values for both correct and incorrect invocations.

4. Run the function, once without name value in the querystring, and once with a name value.

    > ❔ **Question** - Is the outcome of both runs as expected?

## 3. Changing the template for POST requests

Let's change the function to also allow POST requests and test it by posting a request with JSON content in the body.

### Steps

1. Open function.json. In the httpTrigger binding, the `methods` are listed. Add `"post"`. Don't forget to add a comma after `"get"`. Now the function can be triggered by a POST request.
2. We need to add some logic to use the querystring for GET requests and use the request body for POST requests. This can be done by checking the Method property of the request (`$Request.Method`) as follows:

    ```PowerShell
    if ($Request.Method -eq "GET") {
        $Name = $Request.Query.Name
    }
    elseif ($Request.Method -eq "POST"){
        $Name = $Request.Body.Name
    }
    ```

    >📝 **Tip** - You could also use a [Switch](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_switch?view=powershell-7.1) to reach the same goal.

3. Replace the line that creates `$Name` with the `if` loop.

4. Now run the function and do a POST request and submit a body with a `Name` property. Through PowerShell, you can do that like this:

    ```PowerShell
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    Content-Type: application/json

    $Body = @{
        Name = "Your name"
    }
    $Parameters = @{
       Method = "POST"
       Body = ($Body | ConvertTo-Json)
       URI = "http://localhost:7071/api/HelloWorldHttpTrigger"
       ContentType = "application/json"
       }
    Invoke-RestMethod @Parameters
    ```

    >📝 **Tip** - This script uses [Parameter splatting](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_splatting?view=powershell-7.1) to make the code more readable.

    > ❔ **Question** - Is the outcome of the POST as expected?

    > ❔ **Question** - What is the response when you use an empty `name` property?

## 4. Adding a new function for POST requests

Instead of using the `Request` as the name of the parameters in your script, you can change this for something that is more descriptive for your script. This is also very useful if you have more than one input binding. It will not have any impact on the user input.

### Steps

1. Open `function.json`. For the httpTrigger binding, change the `name` to `Person`.
  
   ```Json
       {
      "authLevel": "function",
      "type": "httpTrigger",
      "direction": "in",
      "name": "Person",
      "methods": [
        "get",
        "post"
      ]
    },
   ```

2. Now open `run.ps1` and change every instance of `$Request` to `$Person`
3. Run the Function App.
4. Trigger the new endpoint by making a request.

    > ❔ **Question** - Is the outcome as expected?

## 5. Change the route for a custom greeting

Instead returning *"Hello {name}"* all the time, it would be nice if we can supply our own greeting. So we could return *"Hi {name}"* or  *"Good evening {name}"*. We can do this by changing the route of the function so it contains the greeting. The function will only triggered for GET requests.

### Steps

1. Create a copy of the `HelloWorldHttpTrigger` folder and rename it to `CustomGreetingHttpTrigger`.

    > 📝 **Tip** - Function folder names need to be unique within a Function App.

2. Open `function.json` and change the `httpTrigger` object. Remove the `post` method. Add a route for the custom greeting. The result should look like this

   ```json
    {
      "authLevel": "function",
      "type": "httpTrigger",
      "direction": "in",
      "name": "Request",
      "methods": [
        "get"
      ],
      "route": "CustomGreetingHttpTrigger/{greeting:alpha?}"
    }
   ```

    > 🔎 **Observation** - The `Route` uses a route argument named `greeting` and it has an `alpha` constraint. This means that `greeting` may only contain characters from the alphabet (a-z). The question mark indicates the `greeting` parameter is optional. More info on route parameter constraints in the [official docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1#route-constraint-reference).

3. Remove the `if` loop that checked if the Method was `Post` or `Get` and replace it with the following:

   ```PowerShell
   $Name = $Request.Query.Name
   ```

4. Add an `if` loop to check if the greeting input was used. If not, the default greeting is used.

    ```PowerShell
    $Greeting = $Request.Params.greeting
    if (-not $Greeting) {
       $Greeting = "Hello"
    }
    ```

5. Now change the body that is returned to the user to use the `$Greeting` variable:

    ```PowerShell
    $Body = "$Greeting $Name. This HTTP triggered function executed successfully."

    ```

6. Run the Function App.
    > 🔎 **Observation** - You should see the new HTTP endpoint in the output of the console.

7. Trigger the new endpoint by making a GET request to the following endpoint.

    ```PowerShell
    Invoke-RestMethod http://localhost:7071/api/CustomGreetingHttpTrigger/hi?name=YourName
    ```

    > ❔ **Question** - Is the outcome as expected?

## 6. Homework

Ready to get hands-on? Checkout the [homework assignment for this lesson](http-homework-dotnet.md).

## 7. More info

- For more info about the HTTP Trigger have a look at the official [Azure Functions HTTP Trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=powershell) documentation.

---
[◀ Previous (Prerequisites)](../../prerequisites-powershell.md) | [🔼 Index](../../_index.md) | [Next (Blob Lesson) ▶](../../blob-dotnet.md)
