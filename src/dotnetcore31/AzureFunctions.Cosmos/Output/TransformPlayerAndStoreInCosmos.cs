using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Cosmos.Output
{
    public static class TransformPlayerAndStoreInCosmos
    {
        [FunctionName(nameof(TransformPlayerAndStoreInCosmos))]
        public static void Run(
            [QueueTrigger(
                "newplayer-items",
                Connection = "QueueConnection")]Player playerMessage,
            [CosmosDB(
                databaseName: "Players",
                collectionName: "players",
                ConnectionStringSetting = "CosmosDBConnection")]out Player playerDocument,
            ILogger log)
        {
            // The code for the data transformation should be added here.
            playerMessage.NickName = playerMessage.NickName.ToUpperInvariant();

            // Return the player data in the document variable used by the output binding.
            playerDocument = playerMessage;

            log.LogInformation($"C# Queue trigger function inserted one document into CosmosDB.");
        }
    }
}