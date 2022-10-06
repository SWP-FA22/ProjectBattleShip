namespace Owner.Script.ShopHandle
{
    using Owner.Script.GameData;

    public class LoadResourceData
    {
        public PlayerData GetDataFromServer()
        {
            HandleLocalData handleLocalData = new HandleLocalData();
            PlayerData      data;
            data          = handleLocalData.LoadData<PlayerData>("PlayerData");
            if (data == null)
            {
                data          = new PlayerData();
                data.ShipName = "ship3";
                data.Gold     = 10;
                data.Ruby     = 10;
                data.Diamond  = 5;
                handleLocalData.SaveData("PlayerData",data);
            }
            
            return data;
        }
        
        
    }
}