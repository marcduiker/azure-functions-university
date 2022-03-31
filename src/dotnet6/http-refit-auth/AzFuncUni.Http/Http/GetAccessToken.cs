    using System.Text.Json.Serialization;
    
    public sealed class GetAccessTokenRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = default!;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = default!;
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "client_credentials";
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }
    }
    
    public sealed class GetAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;
    }
