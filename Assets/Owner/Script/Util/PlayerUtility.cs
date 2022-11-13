using Assets.Owner.Script.Network.HttpRequests;
using Owner.Script.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.Util
{
    using UnityEngine;

    public class PlayerUtility
    {
        public static async Task<PlayerData>  GetMyPlayerData()
        {
            HandleLocalData handleLocalData = new HandleLocalData();

            PlayerData data = handleLocalData.LoadData<PlayerData>("PlayerData");

            if (data == null)
            {
                data = await new PlayerRequest().GetPlayerExtraInfo(LoginUtility.GLOBAL_TOKEN);
               
                handleLocalData.SaveData("PlayerData", data);
            }

            return data;
        }

        public static async Task<bool> EquipShip(int id) => await new PlayerRequest().EquipShip(LoginUtility.GLOBAL_TOKEN, id);
    }
}
