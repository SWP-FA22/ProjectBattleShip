using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManage : MonoBehaviour
{
    
    public List<BattleShipData> listBattleShip = new List<BattleShipData>();
    
    void Start()
    {
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship1",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="Ship1",IsOwner=false,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship2",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="Ship2",IsOwner=false,IsEquipped = false});
        listBattleShip.Add(new BattleShipData{ID=1,Name="ship3",Description="aaaaaa",BaseAttack=0.1f,BaseHP=1.6f,BaseSpeed=5f,BaseRota=5f,Price=10f,Addressable="Ship3",IsOwner=false,IsEquipped = false});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
