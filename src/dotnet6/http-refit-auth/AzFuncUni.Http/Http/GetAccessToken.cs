public sealed class GetAccessTokenRequest
{
	public string ClientId { get; set; } = default!;
	public string ClientSecret { get; set; } = default!;
	public string GrantType { get; set; } = "client_credentials";
	public string? Resource { get; set; }
}

public sealed class GetAccessTokenResponse
{
	public string AccessToken { get; set; } = default!;
}