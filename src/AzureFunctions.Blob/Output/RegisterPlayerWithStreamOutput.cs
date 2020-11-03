using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using AzureFunctionsUniversity.Demo.Blob.Models;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class RegisterPlayerWithStreamOutput
    {
        [FunctionName(nameof(RegisterPlayerWithStreamOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            [Blob("players/out/stream-{rand-guid}.json", FileAccess.Write)] Stream playerStream
        )
        {
            var player = await message.Content.ReadAsAsync<Player>();

            IActionResult result;
            if (player == null)
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                result = new AcceptedResult();
            }

            using var writer = new StreamWriter(playerStream);
            var jsonData = JsonConvert.SerializeObject(player);
            await writer.WriteLineAsync(jsonData);

            return result;
        }
    }
}
