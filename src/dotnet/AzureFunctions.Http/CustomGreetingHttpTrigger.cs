using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AzureFunctionsUniversity.Demo
{
    public static class CustomGreetingHttpTrigger
    {
        [FunctionName(nameof(CustomGreetingHttpTrigger))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                nameof(HttpMethods.Get),
                Route = "CustomGreetingHttpTrigger/{greeting:alpha?}")] HttpRequestMessage req,
            string greeting,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var collection = req.RequestUri.ParseQueryString();
            string name = collection["name"];

            greeting = greeting ?? "Hello";

            ObjectResult result;
            if(string.IsNullOrEmpty(name))
            {
                var responseMessage = "Pass a name in the query string for a personalized response.";
                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {
                var responseMessage = $"{greeting}, {name}. This HTTP triggered function executed successfully.";
                result = new OkObjectResult(responseMessage);
            }

            return result;
        }
    }
}
