using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithTypedQueueOutput
    {
        [FunctionName(nameof(NewPlayerWithTypedQueueOutput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            [Queue(QueueConfig.NewPlayerItems)] out Player playerOutput)
        {
            IActionResult result = null;
            playerOutput = null;
            if (string.IsNullOrEmpty(player.Id))
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                result = new AcceptedResult();
                playerOutput = player;
            }

            return result;
        }
    }
}
