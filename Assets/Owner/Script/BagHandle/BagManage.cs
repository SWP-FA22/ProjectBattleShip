namespace Owner.Script.BagHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
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
        
        //item prefab
        [FormerlySerializedAs("ShopItem")] public ShopItem shopItem;
        
        private async void Start()
        {
            Debug.Log("start shop");
            this.HandleLocalData  = new HandleLocalData();

            foreach (var item in await ShopUtility.GetAllItems())
            {
                new List<ItemData>[] { ListCannonItem, ListEngineItem, ListSailItem }[item.Type - 1].Add(item);
            }
            
            CreateButton(this.ListCannonItem);
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