using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Blob
{
    public class StorePlayerWithStringBlobOutput
    {
        private readonly ILogger _logger;

        public StorePlayerWithStringBlobOutput(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StorePlayerWithStringBlobOutput>();
        }

        [Function(nameof(StorePlayerWithStringBlobOutput))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
