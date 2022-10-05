using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Util;
using Owner.Script.GameData;
using Owner.Script.ShopHandle;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ShopBattleShipManage : MonoBehaviour
{
    public                                        ShopBattleShipItem       item;
    public                                        List<ShopItemDataBase>   listShopItems = new List<ShopItemDataBase>();
    public                                        Transform                _parentContainBtn;
    public                                        List<ShopBattleShipItem> listItem;
    public                                        LoadResourceData         LoadResourceData;
    [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI          goldValue;
    [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI          rubyValue;
    [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI          diamondValue;
    public                                        HandleLocalData          handleLocalData;

    void Start()
    {
        Debug.Log("start shop");
        this.LoadResourceData = new LoadResourceData();
        this.handleLocalData  = new HandleLocalData();
        this.LoadResourceData.GetDataFromServer();
        PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
        this.goldValue.text    = playerData.Gold.ToString();
        this.rubyValue.text    = playerData.Ruby.ToString();
        this.diamondValue.text = playerData.Diamond.ToString();
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship1", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship", IsOwner = false, IsEquipped = false });
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship1", IsOwner = true, IsEquipped = false });
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship2", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship2", IsOwner = true, IsEquipped = true });
        listShopItems.AddRange(new ShopUtility().GetAllItems().Result);
        this.CreateButton();
    }

    public void CreateButton()
    {
        Debug.Log("create button");
        for (int i = 0; i < this.listShopItems.Count; i++)
        {
            try
            {
                
                ShopBattleShipItem ShipItemObject = Instantiate(this.item, _parentContainBtn);
                ShipItemObject.SetUpData(this.listShopItems[i]);
                this.listItem.Add(ShipItemObject);
            }
            catch { }

        }
    }


}
