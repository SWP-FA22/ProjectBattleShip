namespace Owner.Script.ShopHandle
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;

    public class ShopItem : MonoBehaviour
    {
        public TextMeshProUGUI priceText;
        public float           price;
        public string          imagepath;
        public string          name;
        private void Start()
        {
            this.priceText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public void SetUpData(float price,string addressable,string name)
        {
            this.name                                                  =  name;
            this.priceText                                             =  gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            this.priceText.text                                        =  "Price: " + price;
            Addressables.LoadAssetAsync<Sprite>(addressable).Completed += (player) => { gameObject.transform.GetChild(0).GetComponent<Image>().sprite = player.Result; };
                
        }
    }
}