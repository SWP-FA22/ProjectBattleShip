namespace Owner.Script.GameData.HandleData
{
    using System.Collections.Generic;

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

        public List<BattleShipData> LoadBattleShipDatas()
        {
            List<BattleShipData> listShopItems = new List<BattleShipData>();
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship1", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 1.6f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship", IsOwner = false, IsEquipped = false });
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false });
            listShopItems.Add(new BattleShipData { ID = 1, Name = "ship2", Description = "aaaaaa", BaseAttack = 0.1f, BaseHP = 2.5f, BaseSpeed = 10f, BaseRota = 5f, Price = 10, Addressable = "ship2", IsOwner = true, IsEquipped = true });
            return listShopItems;
        }

        public void LoadSpecialItemData()
        {
            CurrentSpecialItem.Instance.SpecialData.Add(0,new SpecialItemData{ID = 0,Addressable = "Special1",Amount = 2,BonusATK = 5,BonusHP = 0f,BonusRate = 0,BonusSpeed = 0,Description = "Bonus attack",ImageURL = "",IsDouble = false,Name = "special1",IsTriple = false,Price = 10});
            CurrentSpecialItem.Instance.SpecialData.Add(1,new SpecialItemData{ID = 1,Addressable = "Special2",Amount = 2,BonusATK = 0,BonusHP = 0.5f,BonusRate = 0,BonusSpeed = 0,Description = "Bonus HP",ImageURL = "",IsDouble = false,Name = "special2",IsTriple = false,Price = 10});
            CurrentSpecialItem.Instance.SpecialData.Add(2,new SpecialItemData{ID = 2,Addressable = "Special3",Amount = 3,BonusATK = 0,BonusHP = 0f,BonusRate = 0,BonusSpeed = 0,Description = "triple bullet",ImageURL = "",IsDouble = false,Name = "special1",IsTriple = true,Price = 10});
        }
    }
}