using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Blob.Models;
using Microsoft.AspNetCore.Http;

namespace AzureFunctionsUniversity.Demo.Blob.Output
{
    public static class StorePlayerWithContainerBlobOutput
    {
        [FunctionName(nameof(StorePlayerWithContainerBlobOutput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] Player player,
            [Blob(
                BlobConfig.Container,
                FileAccess.Write)] CloudBlobContainer cloudBlobContainer
        )
        {
            IActionResult result;
            if (player == null)
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                var blob = cloudBlobContainer.GetBlockBlobReference($"out/cloudblob-{player.NickName}.json");
                var playerBlob = JsonConvert.SerializeObject(player);
                await blob.UploadTextAsync(playerBlob);
                result = new AcceptedResult();
            }

            return result;
        }
    }
}
