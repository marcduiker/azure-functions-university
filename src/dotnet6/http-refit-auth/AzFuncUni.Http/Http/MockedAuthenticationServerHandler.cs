using System.Text;
using System.Text.Json;

public sealed class MockedAuthenticationServerHandler : DelegatingHandler
{
	private const string token_ = "eyJhbGciOiJoczI1NiIsInR5cCI6ICJKV1QifQ.eyJzdWIiOiJtZSJ9.signature";
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var tokenResponse = new GetAccessTokenResponse
		{
			AccessToken = token_,
		};

		var tokenResponseJson = JsonSerializer.Serialize(tokenResponse);

		var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
		response.Content = new StringContent(tokenResponseJson, Encoding.UTF8, "application/json");

		return Task.FromResult(response);

		// by *NOT* proceeding any further
		// we will short-circuit the pipeline
		// HTTP request will *NOT* be sent over the wire

		// return base.SendAsync(request, cancellationToken);
	}
}