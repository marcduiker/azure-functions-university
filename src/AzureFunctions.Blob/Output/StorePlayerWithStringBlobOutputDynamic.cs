using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Blob.Models;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class StorePlayerWithStringBlobOutputDynamic
    {
        [FunctionName(nameof(StorePlayerWithStringBlobOutputDynamic))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            IBinder binder)
        {
            IActionResult result;
            if (player == null)
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                var blobAttribute = new BlobAttribute($"players/out/dynamic-{player.Id}.json");
                using (var output = await binder.BindAsync<TextWriter>(blobAttribute))
                {
                    await output.WriteAsync(JsonConvert.SerializeObject(player));
                }
                result = new AcceptedResult();
            }

            return result;
        }
    }
}
