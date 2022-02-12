using System.Collections.Generic;
using System.Text.Json.Serialization;

public sealed class GetRequestResponse
{
	public GetRequestResponse()
	{
		Args = new Dictionary<string, string>();
		Headers = new Dictionary<string, string>();
		
	}
	[JsonPropertyName("args")]
	public Dictionary<string, string> Args { get; set; }
	[JsonPropertyName("data")]
	public string Data { get; set; }
	[JsonPropertyName("headers")]
	public Dictionary<string, string> Headers { get; set; }
}