using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Util;
using Owner.Script.ShopHandle;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManage : MonoBehaviour
{
    public ShopItem item;
    public List<ShopItemDataBase> listShopItems = new List<ShopItemDataBase>();
    public Transform _parentContainBtn;
    public List<ShopItem> listItem;

    void Start()
    {
        Debug.Log("start shop");
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship1", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship", IsOwner = false, IsEquipped = false });
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship1", IsOwner = true, IsEquipped = false });
        // listShopItems.Add(new BattleShipData { ID = 1, Name = "ship2", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10f, Addressable = "ship2", IsOwner = true, IsEquipped = true });

        listShopItems.AddRange(new ShopUtility().GetAllItems().Result);
        foreach (var item in listShopItems)
        {
            Debug.Log(item.Addressable);
        }

        this.CreateButton();
    }

    public void CreateButton()
    {
        Debug.Log("create button");
        for (int i = 0; i < this.listShopItems.Count; i++)
        {
            try
            {
                ShopItem ShipItemObject = Instantiate(this.item, _parentContainBtn);
                ShipItemObject.SetUpData(this.listShopItems[i]);
                this.listItem.Add(ShipItemObject);
            }
            catch { }

        }
    }


}
