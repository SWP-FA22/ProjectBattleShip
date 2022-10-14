using Newtonsoft.Json;

namespace Owner.Script.GameData
{
    public class PlayerData
    {
        // EXTRA ATTRIBUTES

        public struct ExtraData
        {
            [JsonProperty("ship")]
            public BattleShipData Ship { get; set; }

            [JsonProperty("gold")]
            public int Gold { get; set; }

            [JsonProperty("ruby")]
            public int Ruby { get; set; }

            [JsonProperty("diamond")]
            public int Diamond { get; set; }
        }

        [JsonProperty("extra")]
        public ExtraData? Extra { get; set; }
        
        // BASE ATTRIBUTES

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}