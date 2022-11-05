﻿namespace Owner.Script.ShopHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Zenject;

    public class SpecialShopManage : MonoBehaviour
    { 
        public  SpecialItem       specialItem;
   
    public  Transform             _parentContainBtn;
    public  ShopItem              item;
    public  List<SpecialItemData> ListSpecialItemData = new();
    private string                checkCurrentShop;
    private FakeDataIfLoadFail    FakeDataIfLoadFail = new();
    
    [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI  goldValue;
    [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI  rubyValue;
    [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI  diamondValue;
    public                                        HandleLocalData  handleLocalData;
    [Inject]
    private SignalBus signalBus;
    [Inject]
    private DiContainer diContainer;

    public GameObject popupInfo;

    public bool checkIsInBag;
    async void Start()
    {
        // Start Shop
        // Initialize data
        this.handleLocalData = new HandleLocalData();
        if (!this.checkIsInBag)
        {
            this.ReloadData();
        }
        
       
        this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
        this.FakeDataIfLoadFail.LoadSpecialItemData();
        this.CreateButton();
        this.signalBus.Subscribe<ShowPopupSignal>(x=>ShowPopupInfo(x.Position,x.SpecialItemData));
        this.signalBus.Subscribe<ClosePopup>(this.ClosePopUp);
    }
    
    public void ShowPopupInfo(Vector3 position,SpecialItemData specialItemData)
    {
        if (specialItemData != null)
        {
            this.popupInfo.SetActive(true);
            this.popupInfo.transform.position = position + new Vector3(160,160,0);
            this.popupInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                $"Name: {specialItemData.Name}\nBonus HP: {specialItemData.BonusHP}\nBonus Attack: {specialItemData.BonusATK}\nBonus Speed: {specialItemData.BonusSpeed}\nBonus Rotate: {specialItemData.BonusRate}";
            
        }
    }
    public void ClosePopUp()
    {
        this.popupInfo.SetActive(false);
    }
        

    public void ReloadData()
    {
        
        PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
        if (playerData == null)
        {
            PlayerUtility.GetMyPlayerData();
            playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
        }
        if (playerData == null)
        {
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            playerData              = this.FakeDataIfLoadFail.LoadPlayerData();
        }
        this.goldValue.text    = playerData.Extra?.Gold.ToString() ?? "";
        this.rubyValue.text    = playerData.Extra?.Ruby.ToString() ?? "";
        this.diamondValue.text = playerData.Extra?.Diamond.ToString() ?? "";
    }

    public void CreateButton()
    {
        Debug.Log(CurrentSpecialItem.Instance.SpecialData.Count);
        for (int i = 0; i < CurrentSpecialItem.Instance.SpecialData.Count; i++)
        {
            if (CurrentSpecialItem.Instance.SpecialData[i].Amount > 0)
            {
                try
                {
                    SpecialItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                    if (this.checkIsInBag)
                    {
                        SpecialItemObject.SetUpDataForBag(CurrentSpecialItem.Instance.SpecialData[i]);
                        SpecialItemObject.checkIsUseItem = true;
                    }
                    else
                    {
                        SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[i]);
                        SpecialItemObject.checkIsUseItem = false;
                    }
                    SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[i];
                    this.diContainer.InjectGameObject(SpecialItemObject.gameObject);
                }
                catch { }
            }
            
        }
        if (checkCurrentShop == "BattleShipShop")
        {
            Debug.Log("create button");
        }
    }
    
    }
}