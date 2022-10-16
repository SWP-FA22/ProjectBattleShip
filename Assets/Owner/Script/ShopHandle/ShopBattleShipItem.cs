﻿namespace Owner.Script.ShopHandle
{
    using System;
    using Assets.Owner.Script.GameData;
    using Owner.Script.GameData;
    using Owner.Script.Signals;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;
    using Zenject;

    public class ShopBattleShipItem : MonoBehaviour
    {
        public TextMeshProUGUI priceText;
        public ShopItemDataBase data;
        public Outline outline;
        public GameObject lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView view;
        [Inject]
        private SignalBus      signalBus;
        public BattleShipData  battleShipData;
        public TextMeshProUGUI isBuy;
        private void Start()
        {
            Debug.Log("signal:"+this.signalBus);
            HandleLocalData      = new HandleLocalData();
            view                 = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText       = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline         = gameObject.GetComponent<Outline>();
        }
        private void Update()
        {
            if (this.data == null) return;
            if (data is BattleShipData shipData)
            {
                if (shipData.IsEquipped)
                {
                    this.outline.effectDistance = new Vector2(5, 5);
                }
                else
                {
                    this.outline.effectDistance = new Vector2(1, 1);
                }

                if (shipData.IsOwner)
                {
                    this.lockIcon.SetActive(false);
                    this.isBuy.text = "Use";
                }
            }
            
        }
        public void SetUpData(ShopItemDataBase data)
        {
            this.data = data;
            this.name = data.Name;
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text = "Price: " + data.Price;
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }
        public void ChangeModel()
        {
            if (this.view.IsMine)
            {
                if (data is BattleShipData shipData && shipData.IsOwner)
                {
                    GameObject.Find("ItemScroll").GetComponent<ShopBattleShipManage>().listItem.ForEach(x =>
                    {
                        if (x.data is BattleShipData) (x.data as BattleShipData).IsEquipped = false;
                    });
                    shipData.IsEquipped = true;
                
                    PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
                    playerData.ShipName = this.data.Name;
                    Debug.Log("sve model");
                    this.HandleLocalData.SaveData("PlayerData", playerData);
                    this.HandleLocalData.SaveData("ShipStaff",battleShipData);
                    Debug.Log(this.view.ViewID);
                    PhotonNetwork.LocalPlayer.CustomProperties[this.view.ViewID] = this.data.Name;
                    Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties[PhotonNetwork.LocalPlayer.ActorNumber]);
                }
            }
        }

        public void BuyItem()
        {
            if (this.battleShipData.IsOwner == false)
            {
                PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
                if (playerData.Gold >= this.data.Price)
                {
                    playerData.Gold -= (int)this.data.Price;
                    this.HandleLocalData.SaveData("PlayerData",playerData);
                    this.signalBus.Fire<ReloadResourceSignal>();
                }
            }
            else
            {
                this.ChangeModel();
            }
           
            
        }
    }
}