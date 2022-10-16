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
    
    
    [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI  goldValue;
    [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI  rubyValue;
    [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI  diamondValue;
    public                                        HandleLocalData  handleLocalData;
    [Inject]
    private SignalBus signalBus;
    [Inject]
    private DiContainer diContainer;
    async void Start()
    {
        // Start Shop
        // Initialize data
        this.handleLocalData = new HandleLocalData();
        this.ReloadData();
        checkCurrentShop = PlayerPrefs.GetString("Shop");

        if (checkCurrentShop == "BattleShipShop")
        {
            listShopItems.AddRange(await ShopUtility.GetAllShips());
        }
        
        this.CreateButton();
        this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
    }

    public void ReloadData()
    {
        PlayerData playerData = PlayerUtility.GetMyPlayerData().Result;
        this.goldValue.text = playerData.Extra?.Gold.ToString() ?? "";
        this.rubyValue.text = playerData.Extra?.Ruby.ToString() ?? "";
        this.diamondValue.text = playerData.Extra?.Diamond.ToString() ?? "";
    }

    public void CreateButton()
    {
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

        if (checkCurrentShop == "BattleShipShop")
        {
            Debug.Log("create button");
            
        }
    }
}
