using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
           builder.Services.AddSingleton(s => {
                var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Please specify a valid Cosmos DB Connection in the Application Settings.");
                }

                return new CosmosClientBuilder(connectionString)
                    .WithConnectionModeDirect()
                    .Build();
           });
        }
    }
}