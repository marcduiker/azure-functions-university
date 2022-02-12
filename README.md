# Azure Functions University

[![Open in Visual Studio Code](https://open.vscode.dev/badges/open-in-vscode.svg)](https://open.vscode.dev/marcduiker/azure-functions-university)

![Zappy student](./img/zappy-university-192.gif)

Welcome to Azure Functions University! 🎓

This repository contains everything you need to learn about Azure Functions &lt;⚡&gt; and complements the learning videos made by an amazing group of [contributors](https://github.com/marcduiker/azure-functions-university/graphs/contributors)!

You can:

* Watch the lesson videos on the [*Serverless on Azure* YouTube channel](https://bit.ly/az-func-uni-playlist) (and please subscribe!).
* Follow the [**lessons and complete the exercises**](lessons/README.md), in your own pace, here on GitHub.

## Introduction to Serverless

> "Worry about servers, less."

That's the promise of serverless. There are still servers involved, but you don't have to worry about them as much. You can focus on writing code that delivers value for you and your business.

Common aspects of serverless are:

* Pay as you go
* Automatic scaling
* Event-driven (for serverless compute)

These aspects make it very attractive for both small and large organizations to move to serverless.

The Azure cloud contains many services which are considered serverless. There's serverless storage, such as CosmosDB and Blob Storage, and there's serverless compute services such as Azure Functions and Logic Apps.

These are just a few of the [many serverless solutions](https://azure.microsoft.com/en-us/solutions/serverless/#overview) available in Azure.

### Azure Functions

Azure Functions is an event-driven serverless compute platform in the Azure cloud. It integrates seamlessly with many other Azure services through triggers and bindings which we'll cover in great depth in the [**lessons**](./lessons/README.md).

## Lessons

Lesson|.NET Core|.NET 6|Typescript|PowerShell|Python|Contributions by
|-|-|-|-|-|-|-
|Prerequisites|[.NET Core](lessons/dotnetcore31/prerequisites/prerequisites-dotnet.md)|[.NET 6](lessons/dotnet6/prerequisites/README.md)|[TypeScript](lessons/typescript/prerequisites/prerequisites-ts.md)|[PowerShell](lessons/PowerShell/prerequisites/prerequisites-powershell.md)|[Python](lessons/python/prerequisites/prerequisites-python.md)|Marc, Gwyneth, Barbara, Christian, Dana
|HTTP Trigger|[.NET Core](lessons/dotnetcore31/http/http-lesson-dotnet.md)|[.NET 6](lessons/dotnet6/http/README.md)|[TypeScript](lessons/typescript/http/http-lesson-ts.md)|[PowerShell (VS Code)](lessons/PowerShell/http/http-lesson-powershell.md), [PowerShell (portal)](lessons/PowerShell/http/http-lesson-powershell-portal.md)|[Python](lessons/python/http/http-lesson-python.md)|Marc, Gwyneth, Barbara, Caroline, Christian, Dana
|Calling 3rd party REST APIs|-|[.NET 6](lessons/dotnet6/http-refit/README.md)|-|-|-|Maxime, Marc
|Blob Trigger & Bindings|[.NET Core](lessons/dotnetcore31/blob/blob-lesson-dotnet.md)|-|[TypeScript](lessons/typescript/blob/blob-lesson-ts.md)|-|-|Marc, Gwyneth, Christian
|Queue Trigger & Bindings|[.NET Core](lessons/dotnetcore31/queue/queue-lesson-dotnet.md)|-|-|-|-|Marc
|Table Bindings|[.NET Core](lessons/dotnetcore31/table/table-lesson-dotnet.md)|-|-|-|-|Marc
|Deployment to Azure|[.NET Core](lessons/deployment/deployment-lesson.md)|-|-|-|[Python](lessons/python/http/http-lesson-deploy.md)|Marc, Dana
|Cosmos DB Trigger & Bindings|[.NET Core](lessons/dotnetcore31/cosmosdb/cosmosdb-lesson-dotnet.md)|-|-|-|-|Gabriela, Marc
|Durable Functions I |-|-|[TypeScript](lessons/typescript/durable-functions/chaining/chaining-lesson-ts.md)|-|-|Christian, Marc
|Durable Functions II |-|-|[TypeScript](lessons/typescript/durable-functions/advanced/advanced-patterns-lesson.md)|-|-|Christian, Marc
|Configuration|[.NET Core](lessons/dotnetcore31/configuration/configuration-lesson-dotnet.md)|-|-|-|-|Stacy, Marc

## Contribute

Want to contribute? We have a [guide](./CONTRIBUTING.md)!

## License

Please check our [LICENSE.md](./LICENSE.md).
