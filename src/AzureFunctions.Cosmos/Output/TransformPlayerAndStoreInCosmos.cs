using System;
using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Cosmos.Output
{
    public static class TransformPlayerAndStoreInCosmos
    {
        [FunctionName(nameof(TransformPlayerAndStoreInCosmos))]
        public static void Run(
            [QueueTrigger(
                "newplayer-items",
                Connection = "QueueConnection")]string myQueueItem,
            [CosmosDB(
                databaseName: "Players",
                collectionName: "players",
                ConnectionStringSetting = "CosmosDBConnection")]out dynamic document,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            Player player = JsonConvert.DeserializeObject<Player>(myQueueItem);

            /* The code for the data transformation should be added here. */
            player.NickName = player.NickName.ToUpperInvariant();

            // Return the player data in the document variable used by the output binding.
            document = player;

            log.LogInformation($"C# Queue trigger function inserted one document.");
            log.LogInformation($"Description={myQueueItem}");
        }
    }
}