using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Demo.Cosmos.Models
{
    public class TeamPlayer
    {        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("playerName")]
        public string playerName { get; set; }
        
        [JsonProperty("teamId")]
        public int teamId { get; set; }
        
        [JsonProperty("region")]
        public string Region { get; set; }
    }
}