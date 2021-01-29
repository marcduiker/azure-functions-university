using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos.Fluent;
using AzureFunctionsUniversity.Cosmos;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AzureFunctionsUniversity.Cosmos
{
    public class Startup : FunctionsStartup
    {               
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


           builder.Services.AddSingleton(s => {
                var connectionString = config["CosmosDBConnection"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Please specify a valid Cosmos DB Connection in the appSettings.json file or your Azure Functions Settings.");
                }

                return new CosmosClientBuilder(connectionString).WithConnectionModeDirect()
                    .Build();
           });
        }
    }
}