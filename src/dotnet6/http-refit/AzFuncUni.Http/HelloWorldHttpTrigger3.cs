using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFuncUni.Http
{
	public class HelloWorldHttpTrigger3
	{
		private readonly ILogger _logger;
		private readonly IHttpBinOrgApi3 _client;

		public HelloWorldHttpTrigger3(
			ILoggerFactory loggerFactory,
			IHttpBinOrgApi3 client
		)
		{
			_logger = loggerFactory.CreateLogger<HelloWorldHttpTrigger3>();
			_client = client;
		}

		[Function(nameof(HelloWorldHttpTrigger3))]
		public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
		{
			_logger.LogInformation("C# HTTP trigger function processed a request.");

			var response = req.CreateResponse(HttpStatusCode.OK);

			try
			{
				var result = await _client.GetRequest();
				await response.WriteAsJsonAsync(result);
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
}
