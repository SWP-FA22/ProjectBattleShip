namespace Owner.Script.ShopHandle
{
    using System;
    using Owner.Script.GameData;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;

    public class ShopItem : MonoBehaviour
    {
        public TextMeshProUGUI priceText;
        public BattleShipData  data;
        public Outline         outline;
        public GameObject      lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView      view;
        private void Start()
        {
            view                 = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText       = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline         = gameObject.GetComponent<Outline>();
        }
        private void Update()
        {
            if(this.data==null) return;
            if (data.IsEquipped)
            {
                this.outline.effectDistance = new Vector2(5, 5);
            }
            else
            {
                this.outline.effectDistance = new Vector2(1, 1);
            }

            if (data.IsOwner)
            {
                this.lockIcon.SetActive(false);
            }
        }

        public void SetUpData(BattleShipData data)
        {
            this.data                                                       =  data;
            this.name                                                       =  data.Name;
            this.priceText                                                  =  gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text                                             =  "Price: " + data.Price;
            Addressables.LoadAssetAsync<Sprite>(data.Addressable).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }

        public void ChangeModel()
        {
            if (this.view.IsMine)
            {
                if (this.data.IsOwner)
                {
                    GameObject.Find("ItemScroll").GetComponent<ShopManage>().listItem.ForEach(x=>x.data.IsEquipped = false);
                    this.data.IsEquipped = true;
                    PlayerData playerData = new PlayerData() { ShipName = this.data.Name };
                    Debug.Log("sve model");
                    this.HandleLocalData.SaveData("PlayerData",playerData);
                    Debug.Log(this.view.ViewID);
                    PhotonNetwork.LocalPlayer.CustomProperties[this.view.ViewID] = this.data.Name;
                    Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties[PhotonNetwork.LocalPlayer.ActorNumber]);
                }
            }
            
        }
    }
}