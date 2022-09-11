using System.Net.Http.Headers;

public sealed class AuthenticationHandler : DelegatingHandler
{
    private readonly IRequestToken _requestToken;

    public AuthenticationHandler(IRequestToken requestToken)
    {
        _requestToken = requestToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var tokenResponse = await _requestToken.GetAccessToken();
		if (tokenResponse != null)
		{
            var accessToken = tokenResponse.AccessToken;
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		}

        return await base.SendAsync(request, cancellationToken);
    }
}