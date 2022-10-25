namespace Owner.Script.ShopHandle
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
    async void Start()
    {
        // Start Shop
        // Initialize data
        this.handleLocalData = new HandleLocalData();
        this.ReloadData();
       
        this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
        this.FakeDataIfLoadFail.LoadSpecialItemData();
        this.CreateButton();
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
        Debug.Log(CurrentSpecialItem.Instance.SpecialData.Count);
        for (int i = 0; i < CurrentSpecialItem.Instance.SpecialData.Count; i++)
        {
            try
            {
                SpecialItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[i]);
                SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[i];
                this.diContainer.InjectGameObject(SpecialItemObject.gameObject);
            }
            catch { }
        }
        if (checkCurrentShop == "BattleShipShop")
        {
            Debug.Log("create button");
        }
    }
    
    }
}