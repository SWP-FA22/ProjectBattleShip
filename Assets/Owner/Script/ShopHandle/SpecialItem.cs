namespace Owner.Script.ShopHandle
{
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.Signals;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;
    using Zenject;

    public class SpecialItem : MonoBehaviour
    {
        public TextMeshProUGUI priceText;
        public Outline outline;
        public GameObject lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView view;
        [Inject]
        private SignalBus signalBus;
        public SpecialItemData    SpecialItemData;
        public TextMeshProUGUI    isBuy;
        string                    jsonData = "";
        

        private void Start()
        {
            Debug.Log("signal:" + this.signalBus);
            HandleLocalData = new HandleLocalData();
            view = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline = gameObject.GetComponent<Outline>();
            
        }
        private void Update()
        {
            if (this.SpecialItemData == null) return;
            

        }

        public void SetUpData(SpecialItemData data)
        {
            this.SpecialItemData = data;
            this.name = data.Name;
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text = "Price: " + data.Price;
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }


        public void BuyItem()
        {
            //TODO: call api to check data when buy item
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            //ShopUtility.BuyItem(ItemData.ID).Result
            if (playerData.Extra?.Diamond >= this.SpecialItemData.Price)
            {
                    this.SpecialItemData.Amount +=1;
                    playerData.Extra.Diamond -= (int)this.SpecialItemData.Price;
                    this.UpdateData();
                    this.HandleLocalData.SaveData("PlayerData", playerData);
                    this.signalBus.Fire<ReloadResourceSignal>();
            }
            
        }

        public void UseItem()
        {
            this.SpecialItemData.Amount -= 1;
            this.UpdateData();
        }

        public void UpdateData()
        {
            CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID] = this.SpecialItemData;
        }


    }
}
