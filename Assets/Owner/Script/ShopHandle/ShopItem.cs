namespace Owner.Script.ShopHandle
{
    using Assets.Owner.Script.GameData;
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
        public ShopItemDataBase data;
        public Outline outline;
        public GameObject lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView view;
        [Inject]
        private SignalBus      signalBus;
        public ItemData  ItemData;
        public TextMeshProUGUI isBuy;
        private void Start()
        {
            Debug.Log("signal:"+this.signalBus);
            HandleLocalData      = new HandleLocalData();
            view                 = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText       = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline         = gameObject.GetComponent<Outline>();
            SetUpData(this.ItemData);
        }
        private void Update()
        {
            if (this.data == null) return;
            
                if (this.ItemData.IsEquipped)
                {
                    this.outline.effectDistance = new Vector2(5, 5);
                }
                else
                {
                    this.outline.effectDistance = new Vector2(1, 1);
                }

                
        }
        
        public void SetUpData(ItemData data)
        {
            this.data = data;
            this.name = data.Name;
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text = "Price: " + data.Price;
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }
        // public void ChangeModel()
        // {
        //     if (this.view.IsMine)
        //     {
        //         if (data is BattleShipData shipData && shipData.IsOwner)
        //         {
        //             GameObject.Find("ItemScroll").GetComponent<ShopBattleShipManage>().listItem.ForEach(x =>
        //             {
        //                 if (x.data is BattleShipData) (x.data as BattleShipData).IsEquipped = false;
        //             });
        //             shipData.IsEquipped = true;
        //             this.HandleLocalData.SaveData("ItemStaff",this.ItemData);
        //             
        //         }
        //     }
        // }

        public void BuyItem()
        {
            //TODO: call api to check data when buy item
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            if (playerData.Gold >= this.ItemData.Price)
            {
                
            }

        }
    }
}