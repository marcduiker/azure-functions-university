using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Blob.Models;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class RegisterPlayerWithStringOutputDynamic
    {
        [FunctionName(nameof(RegisterPlayerWithStringOutputDynamic))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            IBinder binder
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

            var blobAttribute = new BlobAttribute($"players/out/dynamic-{player.Id}.json");
            using (var output = await binder.BindAsync<TextWriter>(blobAttribute))
            {
                await output.WriteAsync(JsonConvert.SerializeObject(player));
            }

            return result;
        }
    }
}
