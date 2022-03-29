using Refit;

public interface IAuthentication
{

	[Post("/oauth/token")]
	Task<GetAccessTokenResponse> GetAccessToken([Body(BodySerializationMethod.UrlEncoded)] GetAccessTokenRequest request);
}