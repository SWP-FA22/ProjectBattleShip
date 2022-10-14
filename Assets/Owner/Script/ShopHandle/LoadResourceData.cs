namespace Owner.Script.ShopHandle
{
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;

    public class LoadResourceData
    {
        public PlayerData GetDataFromServer()
        {
            HandleLocalData handleLocalData = new HandleLocalData();
            PlayerData data;
            data = handleLocalData.LoadData<PlayerData>("PlayerData");
            if (data == null)
            {
                // TODO: must be check again
                data = PlayerUtility.GetMyPlayerData().Result;
                handleLocalData.SaveData("PlayerData", data);
            }

            return data;
        }


    }
}