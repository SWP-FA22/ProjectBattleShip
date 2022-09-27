using System.Collections;
using System.Collections.Generic;
using Owner.Script.ShopHandle;
using UnityEngine;

public class ShopManage : MonoBehaviour
{
    public                     ShopItem             item;
    public                     List<BattleShipData> listBattleShip = new List<BattleShipData>();
    public Transform            _parentContainBtn;
    void Start()
    {
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship1",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship",IsOwner=false,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship2",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship1",IsOwner=false,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship3",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="ship2",IsOwner=false,IsEquipped = false});
        this.CreateButton();
    }
    
    public void CreateButton()
    {
        for (int i = 0; i < this.listBattleShip.Count; i++)
        {
            ShopItem ShipItemObject = Instantiate(this.item, _parentContainBtn);
            ShipItemObject.SetUpData(this.listBattleShip[i].Price,this.listBattleShip[i].Addressable,this.listBattleShip[i].Name);
            
        }
    }
}
