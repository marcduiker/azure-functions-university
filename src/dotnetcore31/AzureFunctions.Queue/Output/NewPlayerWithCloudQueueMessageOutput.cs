using Azure.Storage.Queues.Models;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithCloudQueueMessageOutput
    {
        [FunctionName(nameof(NewPlayerWithCloudQueueMessageOutput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            [Queue(QueueConfig.NewPlayerItems)] out QueueMessage message)
        {
            IActionResult result = null;
            message = null;

            if (string.IsNullOrEmpty(player.Id))
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                var serializedPlayer = JsonConvert.SerializeObject(player);
                message = QueuesModelFactory.QueueMessage(null, null, serializedPlayer, 0);
                result = new AcceptedResult();
            }

            return result;
        }
    }
}
