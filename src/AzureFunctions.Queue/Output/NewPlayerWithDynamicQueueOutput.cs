using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Queue;
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
            var serializedPlayer = JsonConvert.SerializeObject(player);
            var cloudQueueMessage = new CloudQueueMessage(serializedPlayer); // Not WindowsAzure.Storage.Queue!
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

            var queueAttribute = new QueueAttribute(queueName);
            var cloudQueue = await binder.BindAsync<CloudQueue>(queueAttribute);
            await cloudQueue.AddMessageAsync(cloudQueueMessage);

            return result;
        }
    }
}
