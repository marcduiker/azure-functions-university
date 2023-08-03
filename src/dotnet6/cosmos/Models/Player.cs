

using System.Text.Json.Serialization;

namespace AzureFunctionsUniversity.Cosmos.Models;

public class Player
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("nickName")]
    public string NickName { get; set; }

    [JsonPropertyName("playerId")]
    public int PlayerId { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}