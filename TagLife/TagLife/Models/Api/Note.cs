using Newtonsoft.Json;

namespace TagLife.Models.Api
{
    public class Note
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }
    }
}