using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using AzureFunctionsUniversity.Demo.Cosmos.Models;
using Newtonsoft.Json;
using System;

namespace Company.Function
{
    public class MyFunction
    {
        private readonly CosmosClient _cosmosClient;

        public MyFunction(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName("MyFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
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