namespace Owner.Script.GameData.HandleData
{
    public class FakeDataIfLoadFail
    {
        private HandleLocalData handleLocalData = new HandleLocalData();
        public BattleShipData LoadDataBattleShip()
        {
            BattleShipData battleShipData = new BattleShipData
                { Addressable = "ship3", BaseAttack = 5, BaseRota = 5, BaseHP = 1.6f, BaseSpeed = 5, Description = "dfdsf", ID = 1, IsEquipped = true, IsOwner = true, Name = "ship2" };
            this.handleLocalData.SaveData("BattleShipData",battleShipData);
            return battleShipData;
        }

        public PlayerData LoadPlayerData()
        {
            BattleShipData       battleShipData = this.LoadDataBattleShip();
            PlayerData.ExtraData extraData      = new PlayerData.ExtraData { Diamond = 5, Gold = 10, Ruby = 5, Ship = battleShipData };
            PlayerData playerData = new PlayerData
                { CannonID = 1, Email = "khai1412200213@gmail.com", Extra = extraData, EngineID = 1, ID = 1, Name = "Khai1412", Rank = 10, SailID = 1, Username = "khai1412" };
            this.handleLocalData.SaveData("PlayerData",playerData);
            return playerData;
        }
    }
}