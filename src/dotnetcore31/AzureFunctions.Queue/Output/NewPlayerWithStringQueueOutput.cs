using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithStringQueueOutput
    {
        [FunctionName(nameof(NewPlayerWithStringQueueOutput))]
        [return: Queue(QueueConfig.NewPlayerItems)]
        public static string Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player)
        {
            return JsonConvert.SerializeObject(player);
        }
    }
}
