using System.Collections.Generic;
using System.Net;
using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Cosmos.Output;

public class TransformPlayerAndStoreInCosmos
{
    private readonly ILogger _logger;

    public TransformPlayerAndStoreInCosmos(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TransformPlayerAndStoreInCosmos>();
    }

    [Function("TransformPlayerAndStoreInCosmos")]
    public PlayerOutputType Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "players/{id}")] HttpRequestData req, string nickName,
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
            return new PlayerOutputType()
            {
                UpdatedPlayer = null,
                HttpResponse = req.CreateResponse(HttpStatusCode.NotFound)
            };
        }

        player.NickName = nickName;


        return new PlayerOutputType()
        {
            UpdatedPlayer = player,
            HttpResponse = response
        };
    }
}
