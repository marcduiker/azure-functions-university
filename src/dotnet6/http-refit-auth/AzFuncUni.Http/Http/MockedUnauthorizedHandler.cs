using System.Net;
using System.Net.Http.Headers;

public sealed class MockedUnauthorizedHandler : DelegatingHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var authorization = request.Headers?.Authorization ?? new AuthenticationHeaderValue("Bearer");
		if (String.IsNullOrWhiteSpace(authorization.Parameter))
		{
			var unauthorized = new HttpResponseMessage(HttpStatusCode.Unauthorized)
			{
				RequestMessage = new(),
			};

			return Task.FromResult(unauthorized);
		}

		return base.SendAsync(request, cancellationToken);
	}
}