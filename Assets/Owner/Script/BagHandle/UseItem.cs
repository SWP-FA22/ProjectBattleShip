namespace Owner.Script.BagHandle
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

    public class UseItem : MonoBehaviour
    {
        public TextMeshProUGUI  priceText;
        public ShopItemDataBase data;
        public Outline          outline;
        public GameObject       lockIcon;
        public HandleLocalData  HandleLocalData;
        public PhotonView       view;
        [Inject]
        private SignalBus signalBus;
        public TextMeshProUGUI isBuy;
        public SpecialItemData SpecialItemData;
        public int             currentUse = 0;

        private void Start()
        {
            this.outline.enabled = false;
            this.lockIcon.SetActive(false);
            this.priceText.text = "";
            this.isBuy.text     = "x" + this.SpecialItemData.Amount;
            this.SetUpData(this.SpecialItemData);
            //this.signalBus.Subscribe<UseItemSignal>(x=>ChangeDataItem(x.ID));
        }

        public void UnEquipItem()
        {
            if (CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID].CurrentUse > 0)
            {
                CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID].CurrentUse -= 1;
                CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID].Amount     += 1;
            }
            GameObject.Find("Pick").GetComponent<AudioSource>().Play();
            
        }

        
        private void Update()
        {
            this.isBuy.text = "x" + CurrentSpecialItem.Instance.SpecialData[this.SpecialItemData.ID].CurrentUse;
        }
        
        public void SetUpData(SpecialItemData data)
        {
            this.SpecialItemData                                                   =  data;
            this.name                                                              =  data.Name;
            this.priceText                                                         =  gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text                                                    =  "";
            Addressables.LoadAssetAsync<Sprite>(data.Addressable.Trim()).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
        }
        
        
    }
}