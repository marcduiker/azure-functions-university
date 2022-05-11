using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Refit;
using System.Net.Http;

var builder = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(ConfigureServices)
	;

var host = builder.Build();

host.Run();

const string HttpBinOrgApiHost = "http://httpbin.org";
static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
{
	services
		.AddHttpClient(nameof(IHttpBinOrgApi), ConfigureHttpClient)
		.AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c));

	services
		.AddHttpClient(nameof(IHttpBinOrgApi3), ConfigureHttpClient)
		.AddTypedClient(c => RestService.For<IHttpBinOrgApi3>(c));
}

static void ConfigureHttpClient(IServiceProvider provider, HttpClient client)
{
	client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
	client.DefaultRequestHeaders.Add("Accept", "application/json");
}