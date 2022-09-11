using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Refit;

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
		.AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c))
		.AddHttpMessageHandler<AuthenticationHandler>()
		.AddHttpMessageHandler<MockedUnauthorizedHandler>();

	services
		.AddHttpClient(nameof(IAuthentication), ConfigureHttpClient)
		.AddTypedClient(c => RestService.For<IAuthentication>(c))
		.AddHttpMessageHandler<MockedAuthenticationServerHandler>();

	services.AddTransient<AuthenticationHandler>();
	services.AddTransient<MockedUnauthorizedHandler>();
	services.AddTransient<MockedAuthenticationServerHandler>();

    services.AddTransient<IRequestToken, RequestToken>();

	services.AddSingleton(new GetAccessTokenRequest(){
		ClientId = "please-include-client-id-here",
		ClientSecret = "please-retrieve-client-secret-from-application-settings",
		Resource = HttpBinOrgApiHost
	});
}

static void ConfigureHttpClient(IServiceProvider provider, HttpClient client)
{
	client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
	client.DefaultRequestHeaders.Add("Accept", "application/json");
}