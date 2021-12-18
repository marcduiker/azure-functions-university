using System.Net;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Http
{
    public class HelloWorldHttpTrigger2
    {
        private readonly ILogger _logger;

        public HelloWorldHttpTrigger2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger2>();
        }

        [Function(nameof(HelloWorldHttpTrigger2))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
            var name = queryStringCollection["name"];

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (name == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteStringAsync($"Please provide a value for the name query string parameter.");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.WriteStringAsync($"Hello, {name}");
            }

            return response;
        }
    }
}
