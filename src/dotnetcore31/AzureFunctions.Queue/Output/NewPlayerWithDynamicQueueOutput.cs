using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

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
            IActionResult result;
            string queueName;

            if (string.IsNullOrEmpty(player.Id))
            {
                queueName = QueueConfig.NewPlayerErrorItems;
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                queueName = QueueConfig.NewPlayerItems;
                result = new AcceptedResult();
            }

            var serializedPlayer = JsonConvert.SerializeObject(player);
            var queueMessage = QueuesModelFactory.QueueMessage(null, null, serializedPlayer, 0);
            var queueAttribute = new QueueAttribute(queueName);

            // CloudQueue does not exist any more, what to use instead?
            var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttribute);
            await cloudQueue.SendMessageAsync(queueMessage);

            return result;
        }
    }
}
