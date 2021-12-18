using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Http
{
    public class HelloWorldHttpTrigger6
    {
        private readonly ILogger _logger;

        public HelloWorldHttpTrigger6(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger6>();
        }

        [Function(nameof(HelloWorldHttpTrigger6))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "HelloWorldHttpTrigger6/{greeting:alpha?}")] 
        HttpRequestData req,
        string greeting)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = default;
            if (req.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
            {
                var queryStringCollection = HttpUtility.ParseQueryString(req.Url.Query);
                name = queryStringCollection["name"];
            }
            else
            {
                name = await req.ReadAsStringAsync();
            }

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (string.IsNullOrEmpty(name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync($"Please provide a value for the name query string parameter or in the body as plain text.");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteStringAsync($"{greeting ?? "Hello"}, {name}");
            }

            return response;
        }
    }
}
