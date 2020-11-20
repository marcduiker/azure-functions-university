using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.AspNetCore.Mvc;

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
            [Queue("newplayer-items")] out CloudQueueMessage message)
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
                result = new AcceptedResult();
                message = new CloudQueueMessage(serializedPlayer);
            }

            return result;
        }
    }
}