using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using AzureFunctionsUniversity.Demo.Blob.Models;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class StorePlayerWithStringBlobOutput
    {
        [FunctionName(nameof(StorePlayerWithStringBlobOutput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] HttpRequestMessage message,
            [Blob(
                "players/out/string-{rand-guid}.json",
                FileAccess.Write)] out string playerBlob
        )
        {
            var player = message.Content.ReadAsAsync<Player>().GetAwaiter().GetResult();

            IActionResult result;
            if (player == null)
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                result = new AcceptedResult();
            }

            playerBlob = JsonConvert.SerializeObject(player, Formatting.Indented);

            return result;
        }
    }
}
