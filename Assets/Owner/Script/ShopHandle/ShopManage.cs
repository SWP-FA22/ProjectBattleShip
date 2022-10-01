using System.Collections;
using System.Collections.Generic;
using Owner.Script.ShopHandle;
using UnityEngine;

public class ShopManage : MonoBehaviour
{
    public  ShopItem             item;
    public  List<BattleShipData> listBattleShip = new List<BattleShipData>();
    public  Transform            _parentContainBtn;
    public List<ShopItem>     listItem;
    void Start()
    {
        Debug.Log("start shop");
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship1",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship",IsOwner=false,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship3",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship1",IsOwner=true,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship2",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship2",IsOwner=true,IsEquipped = true});
        this.CreateButton();
    }
    
    public void CreateButton()
    {
        Debug.Log("create button");
        for (int i = 0; i < this.listBattleShip.Count; i++)
        {
            try
            {
                ShopItem ShipItemObject = Instantiate(this.item, _parentContainBtn);
                ShipItemObject.SetUpData(this.listBattleShip[i]);
                this.listItem.Add(ShipItemObject);
            }catch{}
            
        }
    }

    
}
