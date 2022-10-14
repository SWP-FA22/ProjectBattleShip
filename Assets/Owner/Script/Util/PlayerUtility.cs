using Assets.Owner.Script.Network.HttpRequests;
using Owner.Script.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.Util
{
    public class PlayerUtility
    {
        public static async Task<PlayerData> GetMyPlayerData()
        {
            return await new PlayerRequest().GetPlayerExtraInfo(LoginUtility.GLOBAL_TOKEN);
        }
    }
}
