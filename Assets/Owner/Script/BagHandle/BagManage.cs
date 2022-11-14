namespace Owner.Script.BagHandle
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.ShopHandle;
    using Owner.Script.Signals;
    using TMPro;
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
        public GameObject  popupError;
        
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
            this.signalBus.Subscribe<ErrorSignal>(x=>ShowPopupError(x.Message));
            
        }
        public void ShowPopupError(string message)
        {
            this.popupError.SetActive(true);
            this.popupError.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        }
        
        public void CreateButton()
        {
            foreach (var item in CurrentSpecialItem.Instance.SpecialData.Keys)
            {
                try
                {
                    if (CurrentSpecialItem.Instance.SpecialData[item].Amount > 0||CurrentSpecialItem.Instance.SpecialData[item].CurrentUse>0)
                    {
                        SpecialItem SpecialItemObject = Instantiate(this.specialItem, _parentContainBtn);
                        SpecialItemObject.SetUpData(CurrentSpecialItem.Instance.SpecialData[item]);
                        SpecialItemObject.GetComponent<SpecialItem>().SpecialItemData = CurrentSpecialItem.Instance.SpecialData[item];
                        this.diContainer.InjectGameObject(SpecialItemObject.gameObject);
                        SpecialItemObject.GetComponent<SpecialItem>().lockIcon.SetActive(false);
                        SpecialItemObject.GetComponent<SpecialItem>().isBuy.text = "USE "+"x"+CurrentSpecialItem.Instance.SpecialData[item].Amount;
                    }
                    
                }
                catch { }
            }
            
            
        }

        
    }
}