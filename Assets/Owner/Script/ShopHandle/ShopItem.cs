namespace Owner.Script.ShopHandle
{
    using System.IO;
    using Assets.Owner.Script.GameData;
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
        public Outline         outline;
        public GameObject      lockIcon;
        public HandleLocalData HandleLocalData;
        public PhotonView      view;
        [Inject]
        private SignalBus      signalBus;
        public ItemData        ItemData;
        public TextMeshProUGUI isBuy;
        string                 jsonData = "";

        public ListItemData ListItemData;
        string              path = "Assets/Owner/Script/TempData/TempDataItem.txt";
        private void Start()
        {
            Debug.Log("signal:"+this.signalBus);
            HandleLocalData      = new HandleLocalData();
            view                 = gameObject.transform.parent.GetComponent<PhotonView>();
            this.HandleLocalData = new();
            this.priceText       = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.outline         = gameObject.GetComponent<Outline>();
            SetUpData(this.ItemData);
            this.LoadAllData();
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
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }
        

        public void BuyItem()
        {
            //TODO: call api to check data when buy item
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            if (this.ItemData.IsOwner == false)
            {
                if (playerData.Gold >= this.ItemData.Price)
                {
                    this.ItemData.IsOwner =  true;
                    playerData.Gold       -= (int)this.ItemData.Price;
                    this.ChangeItem();
                    this.HandleLocalData.SaveData("PlayerData",playerData);
                    this.signalBus.Fire<ReloadResourceSignal>();
                    int index = this.FindItemByID(this.ItemData);
                    this.ListItemData.item[index] = this.ItemData;
                    this.SaveData();
                }
            }
            else
            {
                this.ChangeItem();
            }
            

        }

        public void LoadAllData()
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    jsonData = reader.ReadToEnd();
                }
            }

            if (jsonData != "")
            {
                this.ListItemData = JsonConvert.DeserializeObject<ListItemData>(jsonData);
            }
        }

        public void SaveData()
        {
            string data = JsonConvert.SerializeObject(this.ListItemData);
            Debug.Log("data:"+data);
            File.WriteAllText(path,data);
        }

        public int FindItemByID(ItemData itemData)
        {
            for (int i = 0; i < this.ListItemData.item.Length; i++)
            {
                if (this.ItemData.ID == this.ListItemData.item[i].ID)
                {
                    return i;
                }
            }

            return -1;
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
            this.HandleLocalData.SaveData("PlayerData",playerData);
        }
        
        
    }
}