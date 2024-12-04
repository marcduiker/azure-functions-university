# Logging

## Goal 🎯

Logging is a critical part of any application and helps monitor and troubleshoot its behaviour in production. In this lesson you will learn how to log from your Function App.

This lesson consists of the following exercises:

|Nr|Exercise
|---|---
|0|[Prerequisites](#0-prerequisites)
|1|[Creating a Function App](#1-creating-a-function-app)
|2|[Logging to Application Insights](#2-logging-to-application-insights)
|3|[Log levels and categories](#3-log-levels-and-categories)
|4|[Cleanup Azure resources](#4-cleanup-azure-resources)
|5|[More Info](#5-more-info)

> 📝 **Tip** - If you're stuck at any point you can have a look at the [source code](../../../src/dotnet8/logging/AzFuncUni.Logging) in this repository.

> 📝 **Tip** - If you have questions or suggestions about this lesson, feel free to [create a Lesson Q&A discussion](https://github.com/marcduiker/azure-functions-university/discussions/categories/lesson-q-a) here on GitHub.

---

## 0. Prerequisites

| Prerequisite | Exercise
| - | -
| Azure Functions Core Tools | 1-5
| VS Code with Azure Functions extension| 1-5
| REST Client for VS Code or Postman | 1-5
| Azure Subscription | 2-5

See [.NET 8 prerequisites](../prerequisites/README.md) for more details.

## 1. Creating a Function App

In this exercise, you'll be creating a Function App with the default HTTPTrigger to serve as a startup project for subsequent exercises.

This exercise is a condensed version of the 
[HTTP Trigger (.NET 6)](../../dotnet6/http/README.md) lesson, targetting .NET 8 instead. Please, refer to that lesson for more in-depth review of the generated files and code

### Steps

1. In VSCode, create the Function App by running `AzureFunctions: Create New Project` in the Command Palette (CTRL+SHIFT+P).
2. Browse to the location where you want to save the function app (e.g. *AzFuncUni.Logging*).
3. Select the language you'll be using to code the function, in this lesson that is using `C#`.
4. Select `.NET 8.0 Isolated LTS` as the runtime.

    If you don't see .NET 8.0, choose:

    - `Change Azure Functions version`
    - Select `Azure Functions v4`
    - Select `.NET 8.0 Isolated LTS`
>  
5. Select `HTTPTrigger` as the template.
6. Give the function a name (e.g. `HelloWorldHttpTrigger`).
7. Enter a namespace for the function (e.g. `AzFuncUni.Logging`).
8. Select `Function` for the AccessRights.

    > 🔎 **Observation** - Notice that a new project has been fully generated.

9. Build the project (CTRL+SHIFT+B).
10. Run the Function App by pressing `F5`.

    > 🔎 **Observation** - Eventually you should see a local HTTP endpoint in the output.

11. Now call the function by making a GET request to the above endpoint using a REST client:

    ```http
    GET http://localhost:7071/api/HelloWorldHttpTrigger
    ```

    > 🔎 **Observation** - You should receive a `200 OK` success response.


## 2. Logging to Application Insights

You may have noticed that some lines have been printed to the Console each time the HTTP function is triggered. Unfortunately, the Console log output is very limited and does not offer much details on the structure of logs.

Most Azure Functions end up logging to Azure Monitor. In particular, _Application Insights_ is a comprehensive Application Performance Monitoring component of Azure Monitor that, amongst other things, collects telemetry from a running application. Logs − or traces − are one of many signals of telemetry that _Application Insights_ collects.

### Overview

Before diving into the code for this exercise, it is critical to understand how your Function App works as it directly drives the configuration for logging to _Application Insights_ successfully.

Azure Functions is a compute infrastructure that runs your _functions-as-a-service_ workload
through the _Functions Runtime Host_ process.

The Function App itself is a .NET Console application that runs as a separate process referred to as the _Worker_ process. 
By linking to the `Microsoft.Azure.Functions.Worker` NuGet package, the program starts a remote-process server and waits for the
communication from the host using the [gRPC](https://grpc.io/) protocol.

![](./isolated-worker-process.png)

You may already be familiar with the `host.json` file that is used to configure the host.

By default, a Function App running as an isolated worker process does not use a separate configuration
file and relies entirely on environment variables and application settings in Azure. As you will see later
in this lesson, however, it is often desirable to use a separate file for the log configuration.

As your isolated worker process runs – as its name suggests – in a separate process from the host,
it needs its own file to store its own settings and configurations for logging. By default, if
a file named `appsettings.json` is present in the output folder, it can automatically registered
as a source of configuration for the worker. Additionally, the log levels and categories
can be automatically configured from the `Logging/LogLevel` configuration section.

In the following section, you will learn the basics of _Application Insights_ and its _Live Metrics_ dashboard.

### Steps

1. Navigate to the Azure Portal and create [a new resource group](https://portal.azure.com/#view/Microsoft_Azure_Marketplace/GalleryItemDetailsBladeNopdl/id/Microsoft.ResourceGroup) named `AzFuncUni`, for instance.

2. Navigate to the newly created resource group and create [a new _Application Insights_](https://portal.azure.com/#view/Microsoft_Azure_Marketplace/GalleryItemDetailsBladeNopdl/id/Microsoft.AppInsights) resource. This may also create a _Log Analytics Workspace_ resource.

3. Go to the newly created resource and notice the `Essentials` section, at the top of the center pane. Please take note of the and `Connection String` property.

4. Back to your local working folder, open the `local.settings.json` project file and add two corresponding properties in the `Values` section:

```json
{
    "Values": {
        …
        "APPLICATIONINSIGHTS_CONNECTION_STRING": "<paste-the-connection-string>"
    }
}
```

5. Open the `host.json` file and replace its content with:

```json
{
    "version": "2.0",
    "logging": {
        "applicationInsights": {
            "samplingSettings": {
                "isEnabled": false,
                "excludedTypes": "Request"
            },
            "enableLiveMetrics": true
        },
        "logLevel": {
            "default": "Warning",
            "Function": "Information",
            "Host.Results": "Information"
        }
    }
}
```

Our changes enable integration with the _Live Metrics_ dashboard associated with the _Application Insights_ resource that we will refer to from now on as “App Insights”, for short.
It also enables [monitoring the function execution](https://learn.microsoft.com/en-us/azure/azure-functions/configure-monitoring?tabs=v2#configure-log-levels) when hosted on Azure through the **Application Insights** and **Funcion Monitors** tabs.

6. At the root of your project, create a new file named `appsettings.json` with the following content:

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Trace",
        }
    }
}
```

> 📝 **Tip** - Please, make sure to name the file `appsettings.json` exactly.

7. Open the `AzFuncUni.Logging.csproj` project file, locate the `<None Update="host.json">` XML start element somewhat towards the end of the file, and add the following – mostly identical section for `appsettings.json` like so:

```xml
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
```

> 📝 **Tip** - Please, note that this section is very similar to the one that copies the `host.json` file to the output folder.

The `appsettings.json` configuration file defines the default level for logging to `Trace` which allows virtually all logs emitted from your worker process to be sent to App Insights.

However, logging to App Insights is not enabled by default.

Furthermore, in an effort to help manage your infrastructure costs efficiently, the App Insights SDK adds a default logging filter for capturing warnings and errors only.

Logging to Application Insights using lower severity requires an explicit override.

8. To fix this, install the [required packages](https://learn.microsoft.com/fr-fr/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Cwindows#install-packages) as follows:

    ```pwsh
    dotnet add package Microsoft.ApplicationInsights.WorkerService
    dotnet add package Microsoft.Azure.Functions.Worker.ApplicationInsights
    ```

    > 📝 **Tip** - The `Microsoft.ApplicationInsights.WorkerService` adjusts logging behavior of the worker (_i.e_ your Function App) to no longer emit logs through the host application (_i.e_ the Functions Runtime host controlling your Function App). Once installed, logs are sent directly to application insights from the worker process instead.

9. Open the `Program.cs` and add some using directives at the top of the file:

    ```c#
    using System.Reflection;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    ```

10. In `Program.cs` load the configuration file and configure log levels and categories.

    Add the following code immediately after the call to
    `builder.ConfigureFunctionsWebApplication()`.

    ```c#
    // Register log levels and categories

    var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    var appSettingsLoggingSection = new ConfigurationBuilder()
        .SetBasePath(basePath!) // required on Linux
        .AddJsonFile("appsettings.json") // first read configuration file
        .AddEnvironmentVariables() // then override using app settings
        .Build()
        .GetSection("Logging")
        ;

    ```

11. Further down in `Program.cs`, enabled logging to App Insights by replacing the commented code with the relevant portion of the code.

    *Replace*

    ```c#
    // Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
    // builder.Services
    //     .AddApplicationInsightsTelemetryWorkerService()
    //     .ConfigureFunctionsApplicationInsights();
    ```

    *With*

    ```c#
    // Logging to Application Insights
    
    builder.Services
        .AddApplicationInsightsTelemetryWorkerService()
        .ConfigureFunctionsApplicationInsights()
        .Configure<LoggerFilterOptions>(options =>
        {
            // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
            // Log levels can also be configured using appsettings.json. For more information, see https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#ilogger-logs
            LoggerFilterRule? toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
    
            if (toRemove is not null)
            {
                options.Rules.Remove(toRemove);
            }
        });
    ```

12. Open the `HelloWorldHttpTrigger.cs` file and update its contents.

    Replace the full contents of the file with the following code:

    ```c#

    ```

13. Compile and run the application again.

14. From the Azure Portal, navigate to App Insights and display the _Live Metrics_ dashboard. You can find `Live Metrics` as one of the options available on the left pane, under the `Investigate` topic.

Wait for ten to twenty seconds for the web page to refresh and display the dashboard.

> 📝 **Tip** - Some ad blockers are known to prevent the dashboard from displaying. If you have μBlock₀, you may see a `Data is temporarily inaccessible` red banner, for instance. Make sure to disable your ad blocker for the _Live Metrics_ page to display properly.

Once the dashboard displays, notice that your machine is listed as one of the servers currently connected to App Insights.

15. On your local machine, call the HTTP-triggered function a couple of times.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    ```

    > 🔎 **Observation** - You should notice some spikes of activity in the _Live Metrics_ dashboard and see a host of logs being recorded on the right `Sample telemetry` pane.

From the right pane, locate and click on one of the recorded logs, with the following message text: `"C# HTTP trigger function processed a request"`.

Details from the selected log are displayed on the lower right pane. In particular, notice the following property:

- `CategoryName`: `HelloWorldHttpTrigger`

Along with their messages, the _Severity Level_ and log _Category_ are amongst the most important properties from the collected logs. In the next section, you will dive into those properties in a bit more details.

16. Hit <kbd>Ctrl</kbd>+<kbd>C</kbd> from the Console of the running application to stop its execution.

## 3. Log levels and categories

In this section, you will learn the basics of _Application Insights_ and its _traces_ log store.
You will also learn about _Log Categories_ and how to filter log output based upon _Log Levels_ and categories.

### Overview

> 📝 **Tip** - App Insights is a comprehensive Application Performance Monitoring (APM) solution. As such, it does a lot more than collecting traces from a running application. In this lesson, you will mostly focus on interacting with App Insights using .NET's [Microsoft.Extensions.Logging.ILogger](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) abstraction.

As you have seen on the previous section, each log is associated with a set of properties, two of which are its _CategoryName_ and _Severity Level_ properties.

#### Log levels

The log level is a measure of how critical or urgent an event reported by the application is. Using an appropriate log level, you can separate logs that simply convey useful information on how your application behaves from errors that are raised as part of its execution.

More importantly, log levels offer a way to regulate the amount of data that is transmitted to your monitoring system. For App Insights, this is a good way to limit the charges incurred by the service.

When troubleshooting is needed, additional insights can be gained by increasing the level of logs momentarily. This can be done dynamically without impacting the running application.

When to use what level for logging falls outside the scope of this lesson.
However, you may like [this simple chart](https://stackoverflow.com/a/64806781) that I found online.

![](log_levels.png)

Credit: [Taco Jan Osinga](https://stackoverflow.com/users/3476764/taco-jan-osinga) (2020) - [When to use the different log levels](https://stackoverflow.com/questions/2031163/when-to-use-the-different-log-levels) - [Stack Overflow](https://stackoverflow.com/).

The [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel) abstraction used within Azure Functions defines the following log levels:

- `LogLevel.Trace`
- `LogLevel.Debug`
- `LogLevel.Information`
- `LogLevel.Warning`
- `LogLevel.Error`
- `LogLevel.Critical`
- `LogLevel.None`

Using `LogLevel.None` effectively disables log output.

> 📝 **Tip** - The .NET `ILogger` abstraction defines the various log levels listed above. However, in App Insights, this translates to the concept of _Severity Level_. There is a one-to-one correspondance between the .NET `LogLevel` and App Insights’ _Severity Levels_.

In the previous exercise, you have seen how setting the default log level for the entire application to `"Trace"` in the `host.json` file dramatically increased the amount of traces emitted when running the application. Changing the default level is a crude way to limit the quantity of logs. Later in this exercise, you will learn to configure log levels for particular _categories_ of logs.

### Categories

Logs are divided into multiple _categories_, which form a set of hierachical namespaces.
Splitting logs into multiple categories allows you to associate appropriate log levels to each category.

By default, each `ILogger` instance is associated with a category hierarchy based upon the full type name of its corresponding dotnet class.

As a [rule of thumb](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging#log-category), each dotnet class has a full type name that represents a category hierarchy for the purpose of logging. However, functions in your Function App behave slightly differently.

Before running the code from your functions, the Functions Runtime first runs the code associated with any trigger and input bindings associated with parameters to your functions. Likewise, after having executed your code, the Functions Runtime runs the code associated with return and output bindings that you may have specified.

For this reason each function decorated using the [`FunctionAttribute`](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Cwindows#methods-recognized-as-functions) class emits logs associated with the following category hierarchy:

- `Functions.<function-name>`: category associated with logs from triggers and bindings used by your function.
- `<custom-function-category>` category associated with logs from your code using the `ILogger` instance obtained from your function method’s `FunctionContext` parameter.

In this exercise, you will discover log categories and learn how to filter log output based upon categories using appropriate log levels.

### Steps

1. Open the Azure Portal and navigate to App Insights.
2. On the left pane, click on `Logs` option under the `Monitoring` topic.
3. Close the `Welcome to Application Insights` screen.
4. On the `Queries` screen, uncheck the `Always show Queries` option and close the screen.

    > 🔎 **Observation** - This is the main interface in App Insights. You will use this interface to write and execute queries on collected telemetry. This screen has a left panel that displays the App Insights tables, amongst which you should see one named `traces`.

    > 📝 **Tip** - Double-click on the `traces` table to start a new query and click the `▶️ Run` button. You should see a list of logs in the results pane at the bottom of your screen.

5. Use a query to discover log categories emitted by your application:

    Replace the query with the following text:

    ```sql
    traces 
    | summarize count(message) by tostring(customDimensions.CategoryName)
    | order by customDimensions_CategoryName
    ```

    > 📝 **Tip** - App Insights uses a SQL-like query language named [Kusto Query Language](https://learn.microsoft.com/en-us/azure/data-explorer/kusto/query/) (KQL).

    > 🔎 **Observation** - The query breaks down the number of logs associated with each category. This is an easy way to get hold of the category associated with each log. You may want to use this method if you see spurious logs that you would like to filter in the future.
    
    Please, note that most log categories share only a handful of common "top-level" prefixes. In practice, the most common category hierarchies are:

	- `Azure`
        - `Azure.Core`
    - `Microsoft`
        - `Microsoft.Azure`
        - `Microsoft.Extensions`
        - `Microsoft.AspNetCore`
    - `Host`
        - `Host.General`
        - `Host.Startup`
    - `Function`
        - `Function.<function-name>`
            - `Function.<function-name>.User`
    - `System`


6. Armed with this knowledge, filter the log output from your application.

    In the `host.json` file, update the `loglevel` section to look like:

    ```json
    "logLevel": {
        "default": "Warning",
        "Functions.HelloWorldHttpTrigger": "Warning",
        "HelloWorldHttpTrigger": "Information"
    }
    ```

7. Compile and run the application.
8. Call the HTTP-triggered function multiple times.

    ```http
    POST http://localhost:7071/api/HelloWorldHttpTrigger
    ```

9. In App Insights, change the `Time range` to `Last 30 minutes` and run the log category break down query again.

    ```sql
    traces 
    | summarize count(message) by tostring(customDimensions.CategoryName)
    | order by customDimensions_CategoryName
    ```

    > 🔎 **Observation** - After a few minutes, you should see a dramatic reduction in the amount of categories under which your application logs. In fact, given enough time, only logs associated with the `HelloWorldHttpTrigger` category should be emitted.

10. Hit <kbd>Ctrl</kbd>+<kbd>C</kbd> from the Console of the running application to stop its execution.

11. Update log levels while the application is running

    Once deployed to Azure, you Function App `host.json` file cannot easily be modified. To change the level associated with a given log category while the application is running, you can update the Function App’s _App Setttings_.

    Notice that in `host.json`, the full "path" to a particular log level directive looks like:

    `` logging / loglevel / <category> ``

    When coming directly from the host, logs are anchored into an implicit `AzureFunctionsJobHost` top-level section.

    To represent this hiearchy in _App Settings_, use a `__` double-underscore separator and replace the `.` separator with a single `_` underscore character.

    On Azure, when _App Settings_ are changed, the Function App restarts.

    When run locally, _App Settings_ are specified in `local.settings.json`.
    Update this file and add the following two properties:

    ```json
    {
        "Values": {
            …
            "AzureFunctionsJobHost__Logging__LogLevel__default": "Debug",
            "AzureFunctionsJobHost__Logging__LogLevel__Function__HelloWorldHttpTrigger": "Trace",
            "Logging__LogLevel__Default": "Warning",
            "Logging__LogLevel__HelloWorldHttpTrigger": "Trace"
        }
    }
    ```

12. Compile and run the application
13. Call the HTTP-triggered function a couple of times.

    > 🔎 **Observation** - Head over to App Insights in Azure Portal and run a query from the _Logs_ authoring page. You can also display the _Live Metrics_ dashboard for more lively feedback.


In this lesson, you learned how to add logs to your application, and how to efficiently use categories and log levels to limit the quantity of logs emitted by the application at any one time – this helps reduce costs and helps support staff by preventing unwanted noise.

You also learned how to update log levels for particular categories to help troubleshoot issues that may happen after the application is deployed to Azure.


## 4. Cleanup Azure resources

In this exercise, you will cleanup Azure resources to prevent unwanted recurring charges.

### Steps

1. Navigate to the Azure Portal locate and delete the `AzFuncUni` resource group. This will automatically remove all resources that have been created as part of this lesson. For the record, you may need to remove:

- The default _Log Analytics Workspace_ resource.
- The test _Application Insights_ resource.
- The resource group.

2. Remove the _Function App_ resource you may have deployed when doing your homework.

## 5. More info

- [Log Levels Explained](https://betterstack.com/community/guides/logging/log-levels-explained/)
