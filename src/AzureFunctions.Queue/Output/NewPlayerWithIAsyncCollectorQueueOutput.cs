using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Queue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureFunctionsUniversity.Demo.Queue.Output
{
    public static class NewPlayerWithIAsyncCollectorQueueOutput
    {
        [FunctionName(nameof(NewPlayerWithIAsyncCollectorQueueOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player[] players,
            [Queue(QueueConfig.NewPlayerItems)] IAsyncCollector<Player> collector)
        {
            IActionResult result = null;

            if (players.Any())
            {
                foreach (var player in players)
                {
                    await collector.AddAsync(player);
                }

                result = new AcceptedResult();
            }
            else
            {
                result = new BadRequestObjectResult("No player data in request.");
            }

            return result;
        }
    }
}
