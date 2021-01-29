using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using AzureFunctionsUniversity.Cosmos.Models;
using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Cosmos.Output
{
    public class StorePlayerWithDI
    {
        private readonly CosmosClient _cosmosClient;

        public StorePlayerWithDI(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(StorePlayerWithDI))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post))] HttpRequest req)
        {
            var myItem = await req.ReadAsStringAsync();

            Player player = JsonConvert.DeserializeObject<Player>(myItem);            
            player.NickName = player.NickName.ToUpperInvariant();      
            player.Id = Guid.NewGuid().ToString();           

            /* Add any validations here */      
            var container =  _cosmosClient.GetContainer("Players", "players");        

            try
            {
                ItemResponse<Player> item  = await container.UpsertItemAsync<Player>(player, new PartitionKey(player.Region));
                return new OkObjectResult(item);
            }
            catch (CosmosException)
            {
                return new BadRequestResult();
            }
            
        }
    }
}