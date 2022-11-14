namespace Owner.Script.GameData
{
    using System;
    using Assets.Owner.Script.GameData;
    using Newtonsoft.Json;
    
    [Serializable]
    public class SpecialItemData : ShopItemDataBase
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

        [JsonProperty("bonusRate")]
        public float BonusRate { get; set; }
        
        [JsonProperty("isDouble")]
        public bool IsDouble {get; set; }

       
        [JsonProperty("isTriple")]
        public bool IsTriple {get; set;}

        [JsonProperty("isFreeze")]
        public bool IsFreeze {get; set;}
        
        [JsonProperty("amount")]
        public int Amount {get; set;}
        
        [JsonProperty("maxUse")]
        public int MaxUse {get; set;}
        
        [JsonProperty("currentUse")]
        public int CurrentUse {get; set;}
    }
}