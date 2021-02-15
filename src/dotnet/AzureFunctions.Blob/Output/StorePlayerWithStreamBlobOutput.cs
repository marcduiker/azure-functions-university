using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using AzureFunctionsUniversity.Demo.Blob.Models;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class StorePlayerWithStreamBlobOutput
    {
        [FunctionName(nameof(StorePlayerWithStreamBlobOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            [Blob(
                "players/out/stream-{rand-guid}.json",
                FileAccess.Write)] Stream playerStream)
        {
            IActionResult result;
            if (player == null)
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                using var writer = new StreamWriter(playerStream);
                var jsonData = JsonConvert.SerializeObject(player);
                await writer.WriteLineAsync(jsonData);
                result = new AcceptedResult();
            }

            return result;
        }
    }
}
