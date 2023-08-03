using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Cosmos
{
    public class QueryPlayersWithHttpTrigger
    {
        private readonly ILogger _logger;

        public QueryPlayersWithHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QueryPlayersWithHttpTrigger>();
        }

        [Function("QueryPlayersWithHttpTrigger")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "players/{region}/{id}")] HttpRequestData req,
        [CosmosDBInput(databaseName: "Players",
                           collectionName: "Players",
                           ConnectionStringSetting = "CosmosDBConnection",
                           Id ="{id}",
                           PartitionKey ="{partitionKey}")] Player player)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            if (player == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound); ;
            }
            string jsonString = JsonSerializer.Serialize(player);
            response.WriteString(jsonString);
            return response;
        }
    }
}
