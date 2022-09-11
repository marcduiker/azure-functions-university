using System.Text.Json.Serialization;

public sealed class GetRequestResponse
{
	public GetRequestResponse()
	{
		Headers = new Dictionary<string, string>();

	}
	[JsonPropertyName("data")]
	public string Data { get; set; }
	[JsonPropertyName("headers")]
	public Dictionary<string, string> Headers { get; set; }
}