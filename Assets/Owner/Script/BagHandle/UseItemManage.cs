namespace Owner.Script.BagHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.ShopHandle;
    using Owner.Script.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Zenject;

    public class UseItemManage : MonoBehaviour
    {
        public HandleLocalData HandleLocalData;
        [Inject]
        private SignalBus signalBus;
        [Inject]
        private DiContainer diContainer;

        public FakeDataIfLoadFail FakeDataIfLoadFail;
        public Transform          _parentContainBtn;
        
        public UseItem specialItem;
        
       
        
        private void Start()
        {
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            //get data from server
            if (CurrentSpecialItem.Instance.SpecialData.Count == 0)
            {
                this.FakeDataIfLoadFail.LoadSpecialItemData();
            }
            //this.signalBus.Subscribe<LoadItem>(this.CreateButton);
            this.CreateButton();
            
            
        }
        
        public void CreateButton()
        {
            foreach (var key  in CurrentSpecialItem.Instance.SpecialData.Keys)
            {
                try
                {
                    if (CurrentSpecialItem.Instance.SpecialData[key].Amount > 0||CurrentSpecialItem.Instance.SpecialData[key].CurrentUse>0)
                    {
                        UseItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                        SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[key]);
                        SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[key];
                        this.diContainer.InjectGameObject(SpecialItemObject.gameObject);
                        SpecialItemObject.GetComponent<SpecialItem>().lockIcon.SetActive(false);
                        SpecialItemObject.GetComponent<SpecialItem>().isBuy.text = "USE "+"x"+CurrentSpecialItem.Instance.SpecialData[key].Amount;
                    }
                }
                catch { }
            }
        }

        
    }
}