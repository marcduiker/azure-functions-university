using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class HelloWorldHttpTrigger
{
	private readonly IHttpBinOrgApi _client;
	private readonly ILogger _logger;

	public HelloWorldHttpTrigger(
		IHttpBinOrgApi client,
		ILoggerFactory loggerFactory
	)
	{
		_client = client;
		_logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger>();
	}

	[Function(nameof(HelloWorldHttpTrigger))]
	public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
    )
	{
		var response = req.CreateResponse(HttpStatusCode.OK);

		try
		{
			var result = await _client.GetRequest(req.Body);
			await response.WriteAsJsonAsync(result);
		}
		catch (Refit.ApiException e)
		{
			response.StatusCode = e.StatusCode;
			response.Headers.Add("Content-Type", "text/plain");
			await response.WriteStringAsync(e.Message);
		}

        return response;
	}
}
