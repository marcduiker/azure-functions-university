using System.Net;
using System.Text.Json;
using AzFuncUni.Blob.Models;
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
        public async Task<HttpAndBlobOutput> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            var player = await req.ReadFromJsonAsync<Player>();
            HttpResponseData response;
            string serializedPlayer = string.Empty;
            
            if (player == null) 
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            else 
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                serializedPlayer = JsonSerializer.Serialize(player);
            }

            return new HttpAndBlobOutput(serializedPlayer, response);
        }
    }
}
