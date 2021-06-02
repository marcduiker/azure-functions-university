using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Demo.Configuration
{
    public static class ReadingEnvironmentVariables
    {
        [FunctionName(nameof(ReadingEnvironmentVariables))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var config = Environment.GetEnvironmentVariable("ConfigurationValue");

            return new OkObjectResult($"ConfigurationValue: {config}");
        }
    }
}
