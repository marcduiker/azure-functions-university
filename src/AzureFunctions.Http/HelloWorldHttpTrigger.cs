using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AzureFunctionsUniversity.Demo
{
    public static class HelloWorldHttpTrigger
    {
        [FunctionName(nameof(HelloWorldHttpTrigger))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                nameof(HttpMethods.Get), 
                nameof(HttpMethods.Post), 
                Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = default;
            if (req.Method.Method == HttpMethods.Get)
            {
                var collection = req.RequestUri.ParseQueryString();
                name = collection["name"];
            }
            else if (req.Method.Method == HttpMethods.Post)
            {
                var person = await req.Content.ReadAsAsync<Person>();
                name = person.Name;
            }

            ObjectResult result;
            if(string.IsNullOrEmpty(name))
            {
                var responseMessage = "Pass a name in the query string or in the request body for a personalized response.";
                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {
                var responseMessage = $"Hello, {name}. This HTTP triggered function executed successfully.";
                result = new OkObjectResult(responseMessage);
            }

            return result;
        }

        [FunctionName(nameof(PersonTypeHttpTrigger))]
        public static IActionResult PersonTypeHttpTrigger(
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
