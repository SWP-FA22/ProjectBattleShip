namespace Owner.Script.ShopHandle
{
    using System;
    using System.IO;
    using Assets.Owner.Script.GameData;
    using Assets.Owner.Script.Util;
    using Newtonsoft.Json;
    using Owner.Script.GameData;
    using Owner.Script.Signals;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;
    using Zenject;


    public class ShopItem : MonoBehaviour
    {
        public TextMeshProUGUI priceText;
        public Outline outline;
        public GameObject lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView view;
        [Inject]
        private SignalBus signalBus;
        public ItemData ItemData;
        public TextMeshProUGUI isBuy;
        string jsonData = "";

        private bool isClicked = false;

        private void Start()
        {
            Debug.Log("signal:" + this.signalBus);
            HandleLocalData = new HandleLocalData();
            view = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline = gameObject.GetComponent<Outline>();
            
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
                this.signalBus.Fire(new ShowPopupSignal{Position = gameObject.transform.position,ItemData = this.ItemData});
                this.isClicked = true;
            }
        }
        private void Update()
        {
            if (this.ItemData == null) return;
            if (this.ItemData.IsOwner)
            {
                if (this.ItemData.IsEquipped)
                {
                    this.outline.effectDistance = new Vector2(5, 5);
                }
                else
                {
                    this.outline.effectDistance = new Vector2(1, 1);
                }
                this.lockIcon.SetActive(false);
                this.isBuy.text = "Use";
            }
        }

        public void SetUpData(ItemData data)
        {
            this.ItemData = data;
            this.name = data.Name;
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text = "Price: " + data.Price;
            try
            {
                Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


        public void BuyItem()
        {
            //TODO: call api to check data when buy item
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            if (this.ItemData.IsOwner == false)
            {
                if (playerData.Extra?.Diamond >= this.ItemData.Price && ShopUtility.BuyItem(ItemData.ID).Result)
                {
                    this.ItemData.IsOwner = true;
                    playerData.Extra.Diamond -= (int)this.ItemData.Price;
                    this.ChangeItem();
                    this.HandleLocalData.SaveData("PlayerData", playerData);
                    this.signalBus.Fire<ReloadResourceSignal>();
                }
                else
                {
                    this.signalBus.Fire(new ErrorSignal(){Message = "Not Enough!!"});
                }
            }
            else
            {
                this.ChangeItem();
            }
            GameObject.Find("Pick").GetComponent<AudioSource>().Play();
        }

        public void ChangeItem()
        {
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            switch (this.ItemData.Type)
            {
                case 1:
                    playerData.CannonID = this.ItemData.ID;
                    break;
                case 2:
                    playerData.EngineID = this.ItemData.ID;
                    break;
                case 3:
                    playerData.SailID = this.ItemData.ID;
                    break;
            }
            GameObject.Find("ItemScroll").GetComponent<ItemShopManage>().listItem.ForEach(x =>
            {
                x.ItemData.IsEquipped = false;
            });
            this.ItemData.IsEquipped = true;
            this.HandleLocalData.SaveData("PlayerData", playerData);
        }


    }
}