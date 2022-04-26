using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class HelloWorldHttpTrigger2
{
	private readonly ILogger _logger;
	private readonly IHttpBinOrgApi _client;

	public HelloWorldHttpTrigger2(
		ILoggerFactory loggerFactory,
		IHttpBinOrgApi client
	)
	{
		_logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger2>();
		_client = client;
	}

	[Function(nameof(HelloWorldHttpTrigger2))]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
	{
		_logger.LogInformation("C# HTTP trigger function processed a request.");

		var response = req.CreateResponse(HttpStatusCode.OK);

		try
		{
			var content = await _client.GetRequest();
			var text = await content.ReadAsStringAsync();

			response.Headers.Add("Content-Type", "application/json; charset=utf-8");
			await response.WriteStringAsync(text);
		}
		catch (Refit.ApiException e)
		{
			response.StatusCode = HttpStatusCode.InternalServerError;
			response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
			await response.WriteStringAsync(e.Message);
		}

		return response;
	}
}