using Newtonsoft.Json;

namespace AzureFunctionsUniversity.Cosmos.Models
{
    public class Player
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nickName")]
        public string NickName { get; set; }

        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}