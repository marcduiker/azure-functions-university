using System.Text.Json.Serialization;

namespace AzureFunctionsUniversity.Cosmos.Models
{
    public class TeamPlayer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("playerName")]
        public string PlayerName { get; set; }

        [JsonPropertyName("teamId")]
        public int TeamId { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }
    }
}