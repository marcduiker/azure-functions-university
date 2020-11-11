using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace AzureFunctionsUniversity.Demo.Blob.Input
{
    public static class GetPlayerWithStreamInput
    {
        [FunctionName(nameof(GetPlayerWithStreamInput))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = "GetPlayerWithStreamInput/{id}")] HttpRequest request,
            string id,
            [Blob(
                "players/in/player-{id}.json",
                FileAccess.Read)] Stream playerStream)
        {
            IActionResult result;
            if (string.IsNullOrEmpty(id))
            {
                result = new BadRequestObjectResult("No player id route.");
            }
            else
            {
                using var reader = new StreamReader(playerStream);
                var content = await reader.ReadToEndAsync();
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
