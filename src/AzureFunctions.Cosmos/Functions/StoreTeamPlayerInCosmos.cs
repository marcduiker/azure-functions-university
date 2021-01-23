using System;
using System.Collections.Generic;
using AzureFunctionsUniversity.Demo.Cosmos.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azureFunctionsApp.Functions
{
    public static class StoreTeamPlayerInCosmos
    {
        [FunctionName("StoreTeamPlayerInCosmos")]
        public static async void Run([CosmosDBTrigger(
            databaseName: "Players",
            collectionName: "players",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
                [CosmosDB(
                databaseName: "Players",
                collectionName: "teamplayers",
                ConnectionStringSetting = "CosmosDBConnection")]IAsyncCollector<TeamPlayer> teamPlayerItemsOut,
                ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
                foreach (var item in input)
                {
                    Player player = JsonConvert.DeserializeObject<Player>(item.ToString());
                    // Any changes to the new items are here
                    TeamPlayer teamPlayer = new TeamPlayer() { Id = player.Id, playerName = player.NickName, Region = player.Region, teamId = 1 };
                    await teamPlayerItemsOut.AddAsync(teamPlayer);
                }
            }
        }
    }
}
