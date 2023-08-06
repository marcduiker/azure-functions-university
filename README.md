# Azure Functions University

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
|Prerequisites|[✔](lessons/dotnetcore31/prerequisites/README.md)|[✔](lessons/dotnet6/prerequisites/README.md)|[✔](lessons/typescript/prerequisites/README.md)|[✔](lessons/PowerShell/prerequisites/README.md)|[✔](lessons/python/prerequisites/README.md)|Marc, Gwyneth, Barbara, Christian, Dana
|HTTP Trigger|[✔](lessons/dotnetcore31/http/README.md)|[✔](lessons/dotnet6/http/README.md)|[✔](lessons/typescript/http/README.md)|[✔ (VS Code)](lessons/PowerShell/http/README.md), <br />[✔ (Portal)](lessons/PowerShell/http/http-lesson-powershell-portal.md)|[✔](lessons/python/http/README.md)|Marc, Gwyneth, Barbara, Caroline, Christian, Dana
|Calling 3rd party REST APIs with Refit|-|[✔](lessons/dotnet6/http-refit/README.md)|-|-|-|Maxime, Marc
|Advanced scenarios with Refit|-|[✔](lessons/dotnet6/http-refit-auth/README.md)|-|-|-|Maxime
|Blob Trigger & Bindings|[✔](lessons/dotnetcore31/blob/README.md)|-|[✔](lessons/typescript/blob/README.md)|-|-|Marc, Gwyneth, Christian
|Queue Trigger & Bindings|[✔](lessons/dotnetcore31/queue/README.md)|-|[✔](lessons/typescript/queue/README.md)|-|-|Marc, Christian
|Table Bindings|[✔](lessons/dotnetcore31/table/README.md)|-|-|-|-|Marc
|Deployment to Azure|[✔](lessons/dotnetcore31/deployment/README.md)|[✔](lessons/dotnet6/deployment/README.md)|-|-|[✔](lessons/python/http/http-lesson-deploy.md)|Marc, Dana
|Cosmos DB Trigger & Bindings|[✔](lessons/dotnetcore31/cosmosdb/README.md)|-|-|-|-|Gabriela, Marc
|Durable Functions I |-|-|[✔](lessons/typescript/durable-functions/chaining/README.md)|-|-|Christian, Marc
|Durable Functions II |-|-|[✔](lessons/typescript/durable-functions/advanced/README.md)|-|-|Christian, Marc
|Configuration|[✔](lessons/dotnetcore31/configuration/README.md)|-|-|-|-|Stacy, Marc

## Contribute

Want to contribute? We have a [guide](./CONTRIBUTING.md)!

## Discussions

Do you have questions or ideas how to improve this project? Join the [discussions](https://github.com/marcduiker/azure-functions-university/discussions) here on GitHub!

## License

Please check our [LICENSE.md](./LICENSE.md).
