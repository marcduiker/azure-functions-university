public interface IRequestToken
{
	Task<GetAccessTokenResponse> GetAccessToken();
}
public sealed class RequestToken : IRequestToken
{
	private readonly IAuthentication _client;
	private readonly GetAccessTokenRequest _credentials;
	public RequestToken(IAuthentication client, GetAccessTokenRequest credentials)
	{
		_client = client;
		_credentials = credentials;
	}
	public Task<GetAccessTokenResponse> GetAccessToken()
	{
		// this requests a token on every call
		// implement caching for more performance

		return _client.GetAccessToken(_credentials);
	}
}
