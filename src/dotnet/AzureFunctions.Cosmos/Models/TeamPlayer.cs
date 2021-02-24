using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Cosmos.Models
{
    public class TeamPlayer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}