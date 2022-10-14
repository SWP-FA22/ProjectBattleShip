namespace Owner.Script.ShopHandle
{
    using System;
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Owner.Script.GameData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;
    using Zenject;

    public class ItemShopManage : MonoBehaviour
    {
        public                                        LoadResourceData LoadResourceData;
        [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI  goldValue;
        [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI  rubyValue;
        [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI  diamondValue;
        public                                        HandleLocalData  HandleLocalData;
        [Inject]
        private SignalBus signalBus;
        [Inject]
        private DiContainer diContainer;

        public List<ItemData> ListCannonItem = new();
        public List<ItemData> ListEngineItem = new();
        public List<ItemData> ListSailItem   = new();
        public Transform      _parentContainBtn;
        public List<ShopItem> listItem = new();
        

        //item prefab
        [FormerlySerializedAs("ShopItem")] public ShopItem shopItem;

        private void Start()
        {
            Debug.Log("start shop");
            this.LoadResourceData = new LoadResourceData();
            this.HandleLocalData  = new HandleLocalData();
            this.LoadResourceData.GetDataFromServer();
            //TODO: load data cannon from server
            //fake data, delete after get data success
        
            this.ListCannonItem.Add(new ItemData
            {
                Type = 1,ImageURL = "",BonusATK = 5,BonusHP = 10,BonusSpeed = 0,BonusRota = 0,Price = 10,
                Addressable = "cannon1",IsOwner = false,IsEquipped = false,Name = "cannon1",Amount = 0
            });
            this.ListCannonItem.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK      = 7,BonusHP  = 5,BonusSpeed    = 0,BonusRota = 0,Price = 5,
                Addressable = "cannon2",IsOwner = true,IsEquipped = true,Name = "cannon2",Amount = 1
            });
            this.ListCannonItem.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK      = 5,BonusHP  = 15,BonusSpeed    = 5,BonusRota = 5,Price = 15,
                Addressable = "cannon1",IsOwner = true,IsEquipped = false,Name = "cannon1",Amount = 1
            });
            this.ReloadData();
            //CreateButton(this.ListCannonItem);
            
        }
        
        public void ReloadData()
        { 
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            this.goldValue.text    = playerData.Gold.ToString();
            this.rubyValue.text    = playerData.Ruby.ToString();
            this.diamondValue.text = playerData.Diamond.ToString();
        }

        public void CreateButton(List<ItemData>listItems)
        {
            foreach (var item in this.listItem)
            {
                Destroy(item.gameObject);
            }
            this.listItem.Clear();
            for (int i = 0; i < listItems.Count; i++)
            {
                try
                {
                    ShopItem ShipItemObject = Instantiate(this.shopItem, _parentContainBtn);
                    ShipItemObject.SetUpData(listItems[i]);
                    ShipItemObject.GetComponent<ShopItem>().ItemData = listItems[i];
                    this.listItem.Add(ShipItemObject);
                    this.diContainer.InjectGameObject(ShipItemObject.gameObject);
                }
                catch { }
            }
        }

        public void ChangeToCannonTag()
        {
            Debug.Log("click to cannon");
            CreateButton(this.ListCannonItem);
        }

        public void ChangeToEngineTag()
        {
            Debug.Log("click to engine");
            CreateButton(this.ListEngineItem);
        }

        public void ChangeToSailTag()
        {
            CreateButton(this.ListSailItem);
        }
        
    }
}