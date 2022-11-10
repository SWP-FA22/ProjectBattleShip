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
        public SpecialItemData SpecialItemData;
        public TextMeshProUGUI isBuy;
        string                 jsonData = "";
        public bool            checkIsUseItem;
        public bool            isClicked = false;

        private void Start()
        {
            Debug.Log("signal:" + this.signalBus);
            HandleLocalData = new HandleLocalData();
            view = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline = gameObject.GetComponent<Outline>();
            this.lockIcon.SetActive(false);

        }

        
        
        public void ShowPopup()
        {
            if (this.isClicked)
            {
                this.signalBus.Fire<ClosePopup>();
                this.isClicked = false;
            }
            else
            {
                this.signalBus.Fire(new ShowPopupSignal{Position = gameObject.transform.position,SpecialItemData = this.SpecialItemData});
                this.isClicked = true;
            }
        }
        private void Update()
        {
            if (this.SpecialItemData == null) return;
            if (this.isBuy.text.Contains("USE x"))
            {
                this.isBuy.text = "USE x" + CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID].Amount;
            }
           

        }

        public void SetUpData(SpecialItemData data)
        {
            this.SpecialItemData = data;
            this.name = data.Name;
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text = "Price: " + data.Price;
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }

        public void SetUpDataForBag(SpecialItemData data)
        {
            this.SpecialItemData                                                   =  data;
            this.name                                                              =  data.Name;
            this.priceText                                                         =  gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text                                                    =  "";
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
            this.isBuy.text                                                        =  "USE x" + data.Amount;
            this.lockIcon.SetActive(false);
        }
        


        public void BuyItem()
        {
            if (this.checkIsUseItem)
            {
                this.UseItem();
            }
            else
            {
                //TODO: call api to check data when buy item
                PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
                //ShopUtility.BuyItem(ItemData.ID).Result
                if (playerData.Extra?.Gold >= this.SpecialItemData.Price)
                {
                    this.SpecialItemData.Amount += 1;
                    playerData.Extra.Gold   -= (int)this.SpecialItemData.Price;
                    this.UpdateData();
                    this.HandleLocalData.SaveData("PlayerData", playerData);
                    this.signalBus.Fire<ReloadResourceSignal>();
                }
            }
            GameObject.Find("Pick").GetComponent<AudioSource>().Play();
            
            
        }

        public void UseItem()
        {
            if (this.SpecialItemData.Amount > 0)
            {
                this.SpecialItemData.Amount     -= 1;
                this.SpecialItemData.CurrentUse += 1;
                this.isBuy.text                 =  "USE x" + this.SpecialItemData.Amount;
                this.UpdateData();
                this.signalBus.Fire(new UseItemSignal{ID = this.SpecialItemData.ID});
            }
            
        }

        public void UpdateData()
        {
            CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID] = this.SpecialItemData;
        }


    }
}
