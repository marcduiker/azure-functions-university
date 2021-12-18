using System.Net;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Http
{
    public class HelloWorldHttpTrigger3
    {
        private readonly ILogger _logger;

        public HelloWorldHttpTrigger3(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger3>();
        }

        [Function(nameof(HelloWorldHttpTrigger3))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
            var name = queryStringCollection["name"];

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (string.IsNullOrEmpty(name))
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
