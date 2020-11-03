using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Blob.Models;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class RegisterPlayerWithBlobContainerOutput
    {
        [FunctionName(nameof(RegisterPlayerWithBlobContainerOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            [Blob("players", FileAccess.Read)] CloudBlobContainer cloudBlobContainer
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

            var blob = cloudBlobContainer.GetBlockBlobReference($"out/cloudblob-{player.NickName}.json");
            var playerBlob = JsonConvert.SerializeObject(player);
            await blob.UploadTextAsync(playerBlob);

            return result;
        }
    }
}
