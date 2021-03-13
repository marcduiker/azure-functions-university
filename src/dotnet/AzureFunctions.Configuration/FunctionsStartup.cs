using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(AzureFunctionsUniversity.Demo.Configuration.Startup))]

namespace AzureFunctionsUniversity.Demo.Configuration
{
	class Startup : FunctionsStartup
	{
		public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
		{
			builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
			{
				options.Connect(Environment.GetEnvironmentVariable("AppConfigurationConnectionString"));
			});

		}

		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddAzureAppConfiguration();
		}
	}
}