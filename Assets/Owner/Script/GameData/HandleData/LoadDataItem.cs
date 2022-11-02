namespace Owner.Script.GameData.HandleData
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Assets.Owner.Script.GameData;
    using Newtonsoft.Json;
    using Owner.Script.ShopHandle;

    public class ListItemData
    {
        public ItemData[] item;
    }
    public class LoadDataItem
    {
        
        string path = "D:\\download\\SPR22\\SWP\\Project\\ProjectBattleShip\\Assets/Owner/Script/TempData/TempDataItem.txt";
        public ListItemData LoadData()
        {
            ListItemData listItemData;
            
            string       jsonData = "";
            while (jsonData == "")
            {
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        jsonData = reader.ReadToEnd();
                    }
                }

                if (jsonData != "")
                {
                    listItemData = JsonConvert.DeserializeObject<ListItemData>(jsonData);
                    return listItemData;
                }
                else
                {
                    this.SetupData();
                }
            }
            

            return null;
        }

        public void SaveData(ListItemData listItemData)
        {
            string data = JsonConvert.SerializeObject(listItemData);
            File.WriteAllText(path,data);
        }

        public void SetupData()
        {
            ListItemData   ListItemDataArray = new ListItemData();
            List<ItemData> ListItemData      =new();
            ListItemData.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK      = 5,BonusHP  = 10,BonusSpeed    = 0,BonusRota = 0,Price = 10,
                Addressable = "cannon1",IsOwner = false,IsEquipped = false,Name = "cannon1"
            });
            ListItemData.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK     = 7,BonusHP = 5,BonusSpeed     = 0,BonusRota = 0,Price = 5,
                Addressable = "cannon2",IsOwner = true,IsEquipped = true,Name = "cannon2"
            });
            ListItemData.Add(new ItemData
            {
                Type        = 1,ImageURL        = "",BonusATK     = 5,BonusHP  = 15,BonusSpeed    = 5,BonusRota = 5,Price = 15,
                Addressable = "cannon1",IsOwner = true,IsEquipped = false,Name = "cannon1"
            });
            ListItemDataArray.item = ListItemData.ToArray();
            SaveData(ListItemDataArray);
        }
    }
}