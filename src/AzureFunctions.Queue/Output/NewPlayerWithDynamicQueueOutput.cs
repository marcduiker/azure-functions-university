using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithDynamicQueueOutput
    {
        [FunctionName(nameof(NewPlayerWithDynamicQueueOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
                IBinder binder)
        {
            var serializedPlayer = JsonConvert.SerializeObject(player);
            var cloudQueueMessage = new CloudQueueMessage(serializedPlayer); // Not WindowsAzure.Storage.Queue!
            IActionResult result = null;
            if (string.IsNullOrEmpty(player.Id))
            {
                var queueAttribute = new QueueAttribute("newplayer-error-items");
                var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttribute);
                await cloudQueue.AddMessageAsync(cloudQueueMessage);
                
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                var queueAttribute = new QueueAttribute("newplayer-items");
                var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttribute);
                await cloudQueue.AddMessageAsync(cloudQueueMessage);
                
                result = new AcceptedResult();
            }

            return result;
        }
    }
}