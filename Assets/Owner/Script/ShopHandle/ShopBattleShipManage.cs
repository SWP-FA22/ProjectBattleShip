using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Util;
using Owner.Script.GameData;
using Owner.Script.ShopHandle;
using Owner.Script.Signals;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Zenject;

public class ShopBattleShipManage : MonoBehaviour
{
    public  ShopBattleShipItem       battleShip;
    public  List<BattleShipData>     listShopItems = new List<BattleShipData>();
    public  Transform                _parentContainBtn;
    public  List<ShopBattleShipItem> listItem;
    public  ShopItem                 item;
    public  List<ItemData>           ListItemData = new();
    private string                   checkCurrentShop;
    
    
    public                                        LoadResourceData LoadResourceData;
    [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI  goldValue;
    [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI  rubyValue;
    [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI  diamondValue;
    public                                        HandleLocalData  handleLocalData;
    [Inject]
    private SignalBus signalBus;
    [Inject]
    private DiContainer diContainer;
    void Start()
    {
        Debug.Log("start shop");
        this.LoadResourceData = new LoadResourceData();
        this.handleLocalData  = new HandleLocalData();
        this.LoadResourceData.GetDataFromServer();
        this.ReloadData();
        checkCurrentShop = PlayerPrefs.GetString("Shop");
        if (checkCurrentShop == "BattleShipShop")
        {
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship1", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship", IsOwner = false, IsEquipped = false });
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship1", IsOwner = true, IsEquipped = false });
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship2", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 2.5f, BaseSpeed = 10f, BaseRota = 5f, Price = 10f, Addressable = "ship2", IsOwner = true, IsEquipped = true });
        }
        
        
        this.CreateButton();
        this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
    }

    public void ReloadData()
    { 
        PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
        this.goldValue.text    = playerData.Gold.ToString();
        this.rubyValue.text    = playerData.Ruby.ToString();
        this.diamondValue.text = playerData.Diamond.ToString();
    }

    public void CreateButton()
    {
        
        if (checkCurrentShop == "BattleShipShop")
        {
            Debug.Log("create button");
            for (int i = 0; i < this.listShopItems.Count; i++)
            {
                try
                {
                    ShopBattleShipItem ShipItemObject = Instantiate(this.battleShip, _parentContainBtn);
                    ShipItemObject.SetUpData(this.listShopItems[i]);
                    ShipItemObject.GetComponent<ShopBattleShipItem>().battleShipData = listShopItems[i];
                    this.listItem.Add(ShipItemObject);
                    this.diContainer.InjectGameObject(ShipItemObject.gameObject);
                }
                catch { }

            } 
        }

        
        
    }


}
