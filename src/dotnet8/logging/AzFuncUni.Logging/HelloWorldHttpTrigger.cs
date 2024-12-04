using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Logging
{
    public class HelloWorldHttpTrigger
    {
        [Function("HelloWorldHttpTrigger")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
            FunctionContext context
        )
        {
            ILogger _logger = context.GetLogger(nameof(HelloWorldHttpTrigger));

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
