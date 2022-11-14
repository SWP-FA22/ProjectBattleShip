namespace Owner.Script.ShopHandle
{
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Network.HttpRequests;
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
    public GameObject popupError;

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
        
        this.CreateButton();
        this.signalBus.Subscribe<ShowPopupSignal>(x=>ShowPopupInfo(x.Position,x.SpecialItemData));
        this.signalBus.Subscribe<ClosePopup>(this.ClosePopUp);
        this.signalBus.Subscribe<ErrorSignal>(x=>ShowPopupError(x.Message));
        
    }
    
    public void ShowPopupError(string message)
    {
        this.popupError.SetActive(true);
        this.popupError.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;

    }

    
    public void ShowPopupInfo(Vector3 position,SpecialItemData specialItemData)
    {
        if (specialItemData != null)
        {
            this.popupInfo.SetActive(true);
            this.popupInfo.transform.position = position + new Vector3(160,160,0);
            if (specialItemData.IsDouble)
            {
                this.popupInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Double Bullet";
            }
            if (specialItemData.IsTriple)
            {
                this.popupInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Triple Bullet";
            }

            if (!specialItemData.IsDouble && !specialItemData.IsTriple)
            {
                this.popupInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    $"Name: {specialItemData.Name}\nBonus HP: {specialItemData.BonusHP}\nBonus Attack: {specialItemData.BonusATK}\nBonus Speed: {specialItemData.BonusSpeed}\nBonus Rotate: {specialItemData.BonusRate}";
            }
           
        }
    }
    public void ClosePopUp()
    {
        this.popupInfo.SetActive(false);
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

    public void CreateButton()
    {
        Debug.Log(CurrentSpecialItem.Instance.SpecialData.Count);
        int amount = 0;
        if (this.checkIsInBag)
        {
            amount = 1;
        }

        foreach (var key in CurrentSpecialItem.Instance.SpecialData.Keys)
        {
            if (CurrentSpecialItem.Instance.SpecialData[key].Amount >= amount||CurrentSpecialItem.Instance.SpecialData[key].CurrentUse>0)
            {
                try
                {
                    SpecialItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                    if (this.checkIsInBag)
                    {
                        SpecialItemObject.SetUpDataForBag(CurrentSpecialItem.Instance.SpecialData[key]);
                        SpecialItemObject.checkIsUseItem = true;
                    }
                    else
                    {
                        SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[key]);
                        SpecialItemObject.checkIsUseItem = false;
                    }
                    SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[key];
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