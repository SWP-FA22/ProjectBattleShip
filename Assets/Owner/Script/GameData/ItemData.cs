using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.GameData
{
    [Serializable]
    public class ItemData 
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("addressable")]
        public string Addressable { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }

        [JsonProperty("bonusATK")]
        public float BonusATK { get; set; }

        [JsonProperty("bonusHP")]
        public float BonusHP { get; set; }

        [JsonProperty("bonusSpeed")]
        public float BonusSpeed { get; set; }

        [JsonProperty("bonusRota")]
        public float BonusRota { get; set; }
        
        [JsonProperty("isOwner")]
        public bool IsOwner {get; set; }

        [JsonProperty("isEquipped")]
        public bool IsEquipped {get; set;}
        
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
