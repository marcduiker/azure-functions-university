using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Demo
{
    public static class PersonTypeHttpTrigger
    {
        [FunctionName(nameof(PersonTypeHttpTrigger))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,  
                nameof(HttpMethods.Post), 
                Route = null)] Person person,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ObjectResult result;
            if(string.IsNullOrEmpty(person.Name))
            {
                var responseMessage = "Pass a name in the query string or in the request body for a personalized response.";
                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {
                var responseMessage = $"Hello, {person.Name}. This HTTP triggered function executed successfully.";
                result = new OkObjectResult(responseMessage);
            }

            return result;
        }
    }
}
