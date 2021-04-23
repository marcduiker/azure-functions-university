# Lessons Index

Lesson|Language specific lesson|Contributions by
|-|-|-
|General Prerequisites|[.NET Core](dotnet/prerequisites/prerequisites-dotnet.md) , [TypeScript](typescript/prerequisites/prerequisites-ts.md)|Marc & Gwyneth (.NET Core), Christian & Marc (TypeScript)
|HTTP Trigger|[.NET Core](dotnet/http/http-lesson-dotnet.md), [TypeScript](typescript/http/http-lesson-ts.md)|Marc & Gwyneth (.NET Core) , Christian & Marc (TypeScript)
|Blob Trigger & Bindings|[.NET Core](dotnet/blob/blob-lesson-dotnet.md)|Marc & Gwyneth
|Queue Trigger & Bindings|[.NET Core](dotnet/queue/queue-lesson-dotnet.md)|Marc
|Table Bindings|[.NET Core](dotnet/table/table-lesson-dotnet.md)|Marc
|[Deployment to Azure](deployment.md)|-|Marc
|Cosmos DB Trigger & Bindings|[.NET Core](cosmosdb-dotnet.md)|Gabriela & Marc
|Durable Functions I |[TypeScript](durablefunctions/chaining-ts.md)|Christian & Marc
|Configuration|[.NET Core](configuration-dotnet.md)|Stacy & Marc
|Logging|-|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/10)
|SignalR|-|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|EventGrid|-|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|Security|-|[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/6)
|[Contribute a new topic?](https://github.com/marcduiker/azure-functions-university/issues/new?assignees=&labels=content&template=content_request.md&title=Content+Request%3A+%3CTITLE%3E)|-|-

## How to use the lessons

All lessons are stand-alone, there's is no strict order in following them, although some lessons refer to others occasionally.
We do suggest you start with the HTTP lesson since that requires the least setup.

We recommend you to clone this repo and open the specific VSCode workspace file located in `workflows\{language}`.

Each lesson consist of the following:

* A [YouTube video](http://bit.ly/az-func-uni-playlist)
* A markdown file with instructions. Best viewed on GitHub.
* An Azure Functions project with a working solution.
* CodeTour files to guide you through the solution. This requires the VSCode [CodeTour extension](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour). *
* A VSCode workspace file that contains only the specific lesson folders and files you need. *

*`*` Not all lessons have CodeTour and workspaces yet.*

## Legend

Throughout the lesson exercises, you will find a few symbols and call-outs:

* 📝 __Tip__ - A tip to indicate a recommended practice.
* 🔎 __Observation__ - An observation to provide more context or deeper explanation.
* ❔ __Question__ - A question which you should try to answer to get a better understanding of the material.
