using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunctionsUniversity.Cosmos.Models;

public class PlayerOutputType
{
    [CosmosDBOutput(databaseName: "Players", collectionName: "Players", ConnectionStringSetting = "CosmosDBConnection")]
    public Player? UpdatedPlayer { get; set; }
    public HttpResponseData? HttpResponse { get; set; }

}