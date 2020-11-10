using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace AzureFunctionsUniversity.Demo.Blob.Input
{
    public static class GetPlayerWithStringInputDynamic
    {
        [FunctionName(nameof(GetPlayerWithStringInputDynamic))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request,
            IBinder binder)
        {
            string id = request.Query["id"];

            IActionResult result;
            
            if (string.IsNullOrEmpty(id))
            {
                result = new BadRequestObjectResult("No player data in request.");
            }
            else
            {
                string content;
                var blobAttribute = new BlobAttribute($"players/in/player-{id}.json");
                using (var input = await binder.BindAsync<TextReader>(blobAttribute))
                {
                    content = await input.ReadToEndAsync();
                }

                result = new ContentResult
                {
                    Content = content,
                    ContentType = MediaTypeNames.Application.Json,
                    StatusCode = 200
                };
            }
            
            return result;
        }
    }
}
