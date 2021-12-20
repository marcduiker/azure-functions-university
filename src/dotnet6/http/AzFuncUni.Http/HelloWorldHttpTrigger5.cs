using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Http
{
    public class HelloWorldHttpTrigger5
    {
        private readonly ILogger _logger;

        public HelloWorldHttpTrigger5(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger5>();
        }

        [Function(nameof(HelloWorldHttpTrigger5))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var person = await req.ReadFromJsonAsync<Person>();

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (string.IsNullOrEmpty(person.Name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync($"Please provide a value for the name in JSON format.");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteStringAsync($"Hello, {person.Name}");
            }

            return response;
        }
    }
}
