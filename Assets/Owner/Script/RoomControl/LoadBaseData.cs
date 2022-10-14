using System.Collections;
using System.Collections.Generic;
using Owner.Script.GameData;
using UnityEngine;

public class LoadBaseData : MonoBehaviour
{
    public HandleLocalData HandleLocalData;
    // Start is called before the first frame update
    void Start()
    {
        this.HandleLocalData = new();
        PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
        if (playerData == null)
        {
            playerData = new PlayerData { ShipName = "ship3", Diamond = 20, Gold = 20, Ruby = 20 };
            this.HandleLocalData.SaveData("PlayerData",playerData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
