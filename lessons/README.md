# Lessons Index

Lesson|.NET Core|.NET 6|Typescript|PowerShell|Python|Contributions by
|-|-|-|-|-|-|-
|Prerequisites|[.NET Core](dotnetcore31/prerequisites/README.md)|[.NET 6](dotnet6/prerequisites/README.md)|[TypeScript](typescript/prerequisites/README.md)|[PowerShell](PowerShell/prerequisites/README.md)|[Python](python/prerequisites/README.md)|Marc, Gwyneth, Barbara, Christian, Dana
|HTTP Trigger|[.NET Core](dotnetcore31/http/README.md)|[.NET 6](dotnet6/http/README.md)|[TypeScript](typescript/http/README.md)|[PowerShell (VS Code)](PowerShell/http/README.md), [PowerShell (portal)](PowerShell/http/http-lesson-powershell-portal.md)|[Python](python/http/README.md)|Marc,Gwyneth, Barbara, Caroline, Christian, Dana
|Calling 3rd party REST APIs|-|[.NET 6](dotnet6/http-refit/README.md)|-|-|-|Maxime, Marc
|Blob Trigger & Bindings|[.NET Core](dotnetcore31/blob/README.md)|-|[TypeScript](typescript/blob/README.md)|-|-|Marc, Gwyneth, Christian
|Queue Trigger & Bindings|[.NET Core](dotnetcore31/queue/README.md)|-|-|-|-|Marc
|Table Bindings|[.NET Core](dotnetcore31/table/README.md)|-|-|-|-|Marc
|Deployment to Azure|[.NET Core](dotnetcore31/deployment/README.md)|[.NET 6](dotnet6/deployment/README.md)|-|-|[Python](python/http/http-lesson-deploy.md)|Marc, Dana
|Cosmos DB Trigger & Bindings|[.NET Core](dotnetcore31/cosmosdb/README.md)|-|-|-|-|Gabriela, Marc
|Durable Functions I |-|-|[TypeScript](typescript/durable-functions/chaining/README.md)|-|-|Christian, Marc
|Durable Functions II |-|-|[TypeScript](typescript/durable-functions/advanced/README.md)|-|-|Christian, Marc
|Configuration|[.NET Core](dotnetcore31/configuration/README.md)|-|-|-|-|Stacy, Marc
|Logging|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/10)
|SignalR|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|EventGrid|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|Security|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/6)
|[Contribute a new topic?](https://github.com/marcduiker/azure-functions-university/issues/new?assignees=&labels=content&template=content_request.md&title=Content+Request%3A+%3CTITLE%3E)|-|-

## How to use the lessons

All lessons are stand-alone, there's is no strict order in following them, although some lessons refer to others occasionally.
We do suggest you start with the HTTP lesson since that requires the least setup.

Each lesson consist of the following:

* A [YouTube video](http://bit.ly/az-func-uni-playlist)
* A markdown file with instructions. Best viewed on GitHub.
* An Azure Functions project with a working solution.
* CodeTour files to guide you through the solution. This requires the VSCode [CodeTour extension](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour). `*`
* A VSCode workspace file that contains only the specific lesson folders and files you need.

`*` *Not all lessons have CodeTours yet.*

When you want to use CodeTour or run the Function projects you need to clone this repo and open the (language specific) VSCode workspace file located in the [workspaces/{language}](../workspaces) folder.

## Legend

Throughout the lesson exercises, you will find a few symbols and call-outs:

* 📝 __Tip__ - A tip to indicate a recommended practice.
* 🔎 __Observation__ - An observation to provide more context or deeper explanation.
* ❔ __Question__ - A question which you should try to answer to get a better understanding of the material.
