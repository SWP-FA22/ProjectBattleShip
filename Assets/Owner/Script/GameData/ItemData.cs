using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.GameData
{
    public class ItemData : ShopItemDataBase
    {
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
    }
}
