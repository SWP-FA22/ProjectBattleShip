namespace Owner.Script.BagHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.ShopHandle;
    using Owner.Script.Signals;
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

        public FakeDataIfLoadFail FakeDataIfLoadFail;
        public Transform          _parentContainBtn;
        public List<SpecialItem>     listItem = new();

        public SpecialItem specialItem;
        
        //item prefab
        [FormerlySerializedAs("ShopItem")] public SpecialItem shopItem;
        
        private async void Start()
        {
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            //get data from server
            if (CurrentSpecialItem.Instance.SpecialData.Count == 0)
            {
                this.FakeDataIfLoadFail.LoadSpecialItemData();
            }
            this.signalBus.Fire<LoadItem>();
            CreateButton();
            
        }
        
        public void CreateButton()
        {
            for (int i = 0; i < CurrentSpecialItem.Instance.SpecialData.Count; i++)
            {
                try
                {
                    if (CurrentSpecialItem.Instance.SpecialData[i].Amount > 0)
                    {
                        SpecialItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                        SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[i]);
                        SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[i];
                        this.diContainer.InjectGameObject(SpecialItemObject.gameObject);
                        SpecialItemObject.GetComponent<SpecialItem>().lockIcon.SetActive(false);
                        SpecialItemObject.GetComponent<SpecialItem>().isBuy.text = "USE "+"x"+CurrentSpecialItem.Instance.SpecialData[i].Amount;
                    }
                    
                }
                catch { }
            }
            
        }

        
    }
}