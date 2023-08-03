using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunctionsUniversity.Cosmos.Models;

public class TeamPlayerOutputType
{
    [CosmosDBOutput(databaseName: "Players", collectionName: "TeamPlayers", ConnectionStringSetting = "CosmosDBConnection")]
    public TeamPlayer? UpdatedTeamPlayer { get; set; }
    public HttpResponseData? HttpResponse { get; set; }

}