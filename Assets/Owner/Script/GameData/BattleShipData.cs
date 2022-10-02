using Assets.Owner.Script.GameData;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
public class BattleShipData : ShopItemDataBase
{
    [JsonProperty("baseATK")]
    public float BaseAttack { get;set;}

    [JsonProperty("baseHP")]
    public float  BaseHP { get; set;}

    [JsonProperty("baseSpeed")]
    public float BaseSpeed { get; set;}

    [JsonProperty("baseRota")]
    public float BaseRota { get; set; }

    [JsonProperty("isOwner")]
    public bool IsOwner {get;set; }

    [JsonProperty("isEquipped")]
    public bool IsEquipped {get; set;}

}
