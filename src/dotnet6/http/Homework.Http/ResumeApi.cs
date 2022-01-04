using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Homework.Http
{
    public class ResumeApi
    {
        private readonly ILogger _logger;

        public ResumeApi(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ResumeApi>();
        }

        [Function(nameof(ResumeApi))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var resume = new Resume
            {
                Name = "Marc Duiker",
                Website = "https://linktr.ee/marcduiker",
                Country = "The Netherlands",
                Skills = ".NET, Azure, pixelart",
                CurrentRole = "Developer Advocate at Ably"
            };
            var response = req.CreateResponse(HttpStatusCode.OK);
            // Don't add a Content-Type header yourself here 
            // as the WriteAsJsonAsync method will do it for you.
            await response.WriteAsJsonAsync(resume);

            return response;
        }
    }
}
