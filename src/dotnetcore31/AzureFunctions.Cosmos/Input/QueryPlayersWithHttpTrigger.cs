using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Cosmos.Input
{
    public static class QueryPlayerWithHttpTrigger
    {
        [FunctionName(nameof(QueryPlayerWithHttpTrigger))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                nameof(HttpMethods.Get),
                Route = "{collectionName}/{partitionKey}/{id}")]HttpRequest req,
            [CosmosDB(
                databaseName: "Players",
                collectionName: "{collectionName}",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}",
                PartitionKey = "{partitionKey}")] Player playerItem,
                ILogger log)
        {            
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (playerItem == null)
            {
                return new NotFoundResult();                
            }

            return new OkObjectResult(playerItem);
        }
    }
}