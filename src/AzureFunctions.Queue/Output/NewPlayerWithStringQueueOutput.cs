using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithStringQueueOutput
    {
        [FunctionName(nameof(NewPlayerWithStringQueueOutput))]
        [return: Queue("newplayer-items")]
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