using System;
using System.Collections.Generic;
using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Trigger
{
    public class StoreTeamPlayerInCosmos
    {
        private readonly ILogger _logger;

        public StoreTeamPlayerInCosmos(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StoreTeamPlayerInCosmos>();
        }

        [Function("StoreTeamPlayerInCosmos")]
        public TeamPlayerOutputType Run([CosmosDBTrigger(
            databaseName: "Players",
            collectionName: "Players",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases")] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].Id);

                foreach (var item in input)
                {
                    Player player = JsonConvert.DeserializeObject<Player>(item.ToString());
                    TeamPlayer teamPlayer = new TeamPlayer()
                    {
                        Id = player.Id,
                        PlayerName = player.NickName,
                        Region = player.Region,
                        TeamId = 1
                    };
                    return new TeamPlayerOutputType()
                    {
                        UpdatedTeamPlayer = teamPlayer

                    };
                }
            }
            return new TeamPlayerOutputType()
            {
                UpdatedTeamPlayer = null

            };
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
