using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithCloudQueueMessageOutput
    {
        [FunctionName(nameof(NewPlayerWithCloudQueueMessageOutput))]
        [return: Queue("newplayer-items")]
        public static CloudQueueMessage Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player)
        {
            var serializedPlayer = JsonConvert.SerializeObject(player);

            return new CloudQueueMessage(serializedPlayer);
        }
    }
}