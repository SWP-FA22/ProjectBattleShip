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
            PlayerData.ExtraData extraData      = new PlayerData.ExtraData { Diamond = 500, Gold = 1000, Ruby = 500, Ship = battleShipData };
            PlayerData playerData = new PlayerData
                { CannonID = 1, Email = "khai1412200213@gmail.com", Extra = extraData, EngineID = 1, ID = 1, Name = "Khai1412", Rank = 1000, SailID = 1, Username = "khai1412" };
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
            if (CurrentSpecialItem.Instance.SpecialData.Count <= 0)
            {
                CurrentSpecialItem.Instance.SpecialData.Add(5,new SpecialItemData{ID = 5,Addressable = "Special1",Amount = 0,BonusATK = 0.1f,BonusHP = 0f,BonusRate = 0,BonusSpeed = 0,Description = "Bonus attack",ImageURL = "",IsDouble = false,Name = "special1",IsTriple = false,Price = 100,MaxUse = 5});
                CurrentSpecialItem.Instance.SpecialData.Add(4,new SpecialItemData{ID = 4,Addressable = "Special2",Amount = 0,BonusATK = 0,BonusHP = 0.5f,BonusRate = 0,BonusSpeed = 2f,Description = "Bonus Speed",ImageURL = "",IsDouble = false,Name = "special2",IsTriple = false,Price = 100,MaxUse = 5});
                CurrentSpecialItem.Instance.SpecialData.Add(7,new SpecialItemData{ID = 7,Addressable = "Special3",Amount = 0,BonusATK = 0,BonusHP = 0f,BonusRate = 100,BonusSpeed = 0,Description = "Bonus Rate",ImageURL = "",IsDouble = false,Name = "special1",IsTriple =false,Price = 100,MaxUse = 3});
                CurrentSpecialItem.Instance.SpecialData.Add(8,new SpecialItemData{ID = 8,Addressable = "Special4",Amount = 0,BonusATK = 0,BonusHP = 0f,BonusRate = 0,BonusSpeed = 0,Description = "Double bullet",ImageURL = "",IsDouble = true,Name = "special1",IsTriple = false,Price = 100,MaxUse = 1});
                CurrentSpecialItem.Instance.SpecialData.Add(11,new SpecialItemData{ID = 11,Addressable = "Special5",Amount = 0,BonusATK = 0,BonusHP = 0f,BonusRate = 0,BonusSpeed = 0,Description = "Triple bullet",ImageURL = "",IsDouble = false,Name = "special1",IsTriple = true,Price = 100,MaxUse = 1});
                //double 12
            }
            
        }
    }
}