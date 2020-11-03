using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using AzureFunctionsUniversity.Demo.Blob.Models;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class RegisterPlayerWithStringOutput
    {
        [FunctionName(nameof(RegisterPlayerWithStringOutput))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            [Blob("players/out/string-{rand-guid}.json", FileAccess.Write)] out string playerBlob
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
