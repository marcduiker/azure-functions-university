using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Refit;
using System.Net.Http;

namespace src
{
	public class Program
	{
		public static void Main()
		{
			var builder = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.ConfigureServices(ConfigureServices)
				;

			var host = builder.Build();

			host.Run();
		}

		private const string HttpBinOrgApiHost = "http://httpbin.org";
		private static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
		{
			services
				.AddHttpClient("HttpBinOrgApi", ConfigureHttpClient)
				.AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c));

			services
				.AddHttpClient("HttpBinOrgApi3", ConfigureHttpClient)
				.AddTypedClient(c => RestService.For<IHttpBinOrgApi3>(c));
		}

		private static void ConfigureHttpClient(IServiceProvider provider, HttpClient client)
		{
			client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.Add("User-Agent", "dotnet-core/3.1");
		}
	}
}