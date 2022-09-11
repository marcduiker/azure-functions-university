# Lessons Index

Lesson|.NET Core|.NET 6|Typescript|PowerShell|Python|Contributions by
|-|-|-|-|-|-|-
|Prerequisites|[✔](dotnetcore31/prerequisites/README.md)|[✔](dotnet6/prerequisites/README.md)|[✔](typescript/prerequisites/README.md)|[✔](PowerShell/prerequisites/README.md)|[✔](python/prerequisites/README.md)|Marc, Gwyneth, Barbara, Christian, Dana
|HTTP Trigger|[✔](dotnetcore31/http/README.md)|[✔](dotnet6/http/README.md)|[✔](typescript/http/README.md)|[✔ (VS Code)](PowerShell/http/README.md), <br /> [✔ (portal)](PowerShell/http/http-lesson-powershell-portal.md)|[✔](python/http/README.md)|Marc, Gwyneth, Barbara, Caroline, Christian, Dana
|Calling 3rd party REST APIs with Refit|-|[✔](dotnet6/http-refit/README.md)|-|-|-|Maxime, Marc
|Advanced scenarios with Refit|-|[✔](dotnet6/http-refit-auth/README.md)|-|-|-|Maxime
|Blob Trigger & Bindings|[✔](dotnetcore31/blob/README.md)|-|[✔](typescript/blob/README.md)|-|-|Marc, Gwyneth, Christian
|Queue Trigger & Bindings|[✔](dotnetcore31/queue/README.md)|-|-|-|-|Marc
|Table Bindings|[✔](dotnetcore31/table/README.md)|-|-|-|-|Marc
|Deployment to Azure|[✔](dotnetcore31/deployment/README.md)|[✔](dotnet6/deployment/README.md)|-|-|[✔](python/http/http-lesson-deploy.md)|Marc, Dana
|Cosmos DB Trigger & Bindings|[✔](dotnetcore31/cosmosdb/README.md)|-|-|-|-|Gabriela, Marc
|Durable Functions I |-|-|[✔](typescript/durable-functions/chaining/README.md)|-|-|Christian, Marc
|Durable Functions II |-|-|[✔](typescript/durable-functions/advanced/README.md)|-|-|Christian, Marc
|Configuration|[✔](dotnetcore31/configuration/README.md)|-|-|-|-|Stacy, Marc
|Logging||||||[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/10)
|SignalR||||||[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|EventGrid||||||[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/13)
|Security||||||[Contribute as author/presenter?](https://github.com/marcduiker/azure-functions-university/issues/6)
|📝||||||[Contribute a new topic?](https://github.com/marcduiker/azure-functions-university/issues/new?assignees=&labels=content&template=content_request.md&title=Content+Request%3A+%3CTITLE%3E)|-|-

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

* 📝 __Tip__ - A recommendation for further reading or investigation.
* 🔎 __Observation__ - An observation to provide more context or deeper explanation.
* ❔ __Question__ - A question which you should try to answer to get a better understanding of the material.
