namespace Owner.Script.BagHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Owner.Script.GameData;
    using Owner.Script.ShopHandle;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Zenject;

    public class BagManage : MonoBehaviour
    {
        public HandleLocalData HandleLocalData;
        [Inject]
        private SignalBus signalBus;
        [Inject]
        private DiContainer diContainer;

        public List<ItemData> ListCannonItem = new();
        public List<ItemData> ListEngineItem = new();
        public List<ItemData> ListSailItem   = new();
        public Transform      _parentContainBtn;
        public List<ShopItem> listItem = new();
        
        public LoadResourceData LoadResourceData;
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
                Type        = 1,ImageURL        = "",BonusATK      = 5,BonusHP  = 10,BonusSpeed    = 0,BonusRota = 0,Price = 10,
                Addressable = "cannon1",IsOwner = false,IsEquipped = false,Name = "cannon1"
            });
            this.ListCannonItem.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK     = 7,BonusHP = 5,BonusSpeed     = 0,BonusRota = 0,Price = 5,
                Addressable = "cannon2",IsOwner = true,IsEquipped = true,Name = "cannon2"
            });
            this.ListCannonItem.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK     = 5,BonusHP  = 15,BonusSpeed    = 5,BonusRota = 5,Price = 15,
                Addressable = "cannon1",IsOwner = true,IsEquipped = false,Name = "cannon1"
            });
            
            //CreateButton(this.ListCannonItem);
            
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

        
    }
}