using System.Net;
using System.Text.Json;
using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsUniversity.Output
{
    public class StorePlayerWithDI
    {
        private readonly ILogger _logger;
        private readonly PlayersRepository _repository;

        public StorePlayerWithDI(ILoggerFactory loggerFactory, PlayersRepository repository)
        {
            _logger = loggerFactory.CreateLogger<StorePlayerWithDI>();
            _repository = repository;

        }

        [Function("StorePlayerWithDI")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req, string nickName, string region)
        {
            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            var newPlayer = new Player()
            {
                NickName = nickName,
                Region = region,
            };

            await _repository.AddPlayerAsync(newPlayer);
            string jsonString = JsonSerializer.Serialize(newPlayer);
            response.WriteString(jsonString);

            return response;
        }
    }
}
