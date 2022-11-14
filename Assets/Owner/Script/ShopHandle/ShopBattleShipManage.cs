using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Util;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using Owner.Script.ShopHandle;
using Owner.Script.Signals;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

public class ShopBattleShipManage : MonoBehaviour
{
    public                                        ShopBattleShipItem       battleShip;
    public                                        List<BattleShipData>     listShopItems = new List<BattleShipData>();
    public                                        Transform                _parentContainBtn;
    public                                        List<ShopBattleShipItem> listItem;
    public                                        ShopItem                 item;
    public                                        List<ItemData>           ListItemData = new();
    private                                       string                   checkCurrentShop;
    private                                       FakeDataIfLoadFail       FakeDataIfLoadFail = new();
    public                                        GameObject               popupInfo;
    public                                        GameObject               popupError;
    [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI          goldValue;
    [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI          rubyValue;
    [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI          diamondValue;
    public                                        HandleLocalData          handleLocalData;
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
        checkCurrentShop   = PlayerPrefs.GetString("Shop");
        //this.listShopItems = this.FakeDataIfLoadFail.LoadBattleShipDatas();
        listShopItems.AddRange(await ShopUtility.GetAllShips(true));
        this.CreateButton();
        this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
        this.signalBus.Subscribe<ShowPopupSignal>(x=>ShowPopupInfo(x.Position,x.BattleShipData));
        this.signalBus.Subscribe<ClosePopup>(this.ClosePopUp);
        this.signalBus.Subscribe<ErrorSignal>(x=>ShowPopupError(x.Message));
    }

    public void ShowPopupError(string message)
    {
        this.popupError.SetActive(true);
        this.popupError.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;

    }

    public void ClosePopUp()
    {
        this.popupInfo.SetActive(false);
        
    }

    public void ShowPopupInfo(Vector3 position,BattleShipData battleShipData)
    {
        if (battleShipData != null)
        {
            this.popupInfo.SetActive(true);
            this.popupInfo.transform.position = position + new Vector3(160,160,0);
            this.popupInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                $"Name: {battleShipData.Name}\nBase HP: {battleShipData.BaseHP}\nBase Attack: {battleShipData.BaseAttack}\nBase Speed: {battleShipData.BaseSpeed}\nBase Rotate: {battleShipData.BaseRota}";
            
        }
    }

    public void ReloadData()
    {
        PlayerUtility.GetMyPlayerData();
        PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
        if (playerData == null)
        {
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            playerData              = this.FakeDataIfLoadFail.LoadPlayerData();
        }
        this.goldValue.text    = ResolveData(playerData.Extra.Gold);
        this.rubyValue.text    = ResolveData(playerData.Extra.Ruby);
        this.diamondValue.text = ResolveData(playerData.Extra.Diamond);
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

    public string ResolveData(int data)
    {
        string result = "";
        if (data >= 1000)
        {
            int temp = data / 100 - (data / 1000)*10;
            result += temp!=0?(data / 1000) + "k" + (temp):(data / 1000) + "k";
            return result;
        }
        else
        {
            return data.ToString();
        }
    }
}
