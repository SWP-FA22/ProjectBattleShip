namespace Owner.Script.ShopHandle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Assets.Owner.Script.GameData;
    using Owner.Script.GameData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;
    using Zenject;
    using Newtonsoft.Json;
    using Owner.Script.Signals;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData.HandleData;

    public class ItemShopManage : MonoBehaviour
    {
        [FormerlySerializedAs("GoldValue")]    public TextMeshProUGUI  goldValue;
        [FormerlySerializedAs("RubyValue")]    public TextMeshProUGUI  rubyValue;
        [FormerlySerializedAs("DiamondValue")] public TextMeshProUGUI  diamondValue;
        public                                        HandleLocalData  HandleLocalData;
        [Inject]
        private SignalBus signalBus;
        [Inject]
        private DiContainer diContainer;

        public List<ItemData>     ListCannonItem = new();
        public List<ItemData>     ListEngineItem = new();
        public List<ItemData>     ListSailItem   = new();
        public Transform          _parentContainBtn;
        public List<ShopItem>     listItem = new();
        public TextAsset          textJson;
        public FakeDataIfLoadFail FakeDataIfLoadFail;

        //item prefab
        [FormerlySerializedAs("ShopItem")] public ShopItem shopItem;

       

        private void Start()
        {
            Debug.Log("start shop");
            this.HandleLocalData    = new HandleLocalData();
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            this.BindData();
            this.ReloadData();
            CreateButton(this.ListCannonItem);
            CreateButton(this.ListEngineItem);
            CreateButton(this.ListSailItem);
            this.signalBus.Subscribe<ReloadResourceSignal>(this.ReloadData);
            
        }
        

        public void BindData()
        {
            ShopUtility.GetAllItems();
            foreach (var item in CurrentItemData.Instance.Items)
            {
                if (item.Type == 1)
                {
                    this.ListCannonItem.Add(item);
                }

                if (item.Type == 2)
                {
                    this.ListEngineItem.Add(item);
                }

                if (item.Type == 3)
                {
                    this.ListSailItem.Add(item);
                }
            }
        }

        public void ReloadData()
        { 
            PlayerUtility.GetMyPlayerData();
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            if (playerData == null)
            {
                this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
                playerData              = this.FakeDataIfLoadFail.LoadPlayerData();
            }
            this.goldValue.text    = playerData.Extra?.Gold.ToString() ?? "";
            this.rubyValue.text    = playerData.Extra?.Ruby.ToString() ?? "";
            this.diamondValue.text = playerData.Extra?.Diamond.ToString() ?? "";
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