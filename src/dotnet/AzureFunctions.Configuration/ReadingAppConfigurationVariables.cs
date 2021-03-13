using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Configuration
{
	public class ReadingAppConfigurationVariables
	{
		public IConfiguration _configuration { get; }

		public ReadingAppConfigurationVariables(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[FunctionName("ReadingAppConfigurationVariables")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("ReadingEnvironmentVariables Triggered via HTTP");

			var config = _configuration["ConfigurationValue"];

			return new OkObjectResult($"ConfigurationValue: {config}");
		}
	}
}
