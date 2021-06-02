using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AzureFunctionsUniversity.HomeWork
{
    public static class ResumeFromBlob
    {
        [FunctionName("ResumeFromBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, 
                nameof(HttpMethods.Get), 
                Route = null)] HttpRequest req,
            [Blob("resume/resume.json", FileAccess.Read)] Stream resumeStream, 
            ILogger log)
        {
            IActionResult result;
            using var reader = new StreamReader(resumeStream);
            var content = await reader.ReadToEndAsync();
            result = new ContentResult 
            { 
                Content = content,
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = 200
            };

            return result;
        }
    }
}
