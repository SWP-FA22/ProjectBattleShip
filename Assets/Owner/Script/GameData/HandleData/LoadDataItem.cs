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
        
        
        public ListItemData LoadData()
        {
            if (CurrentItemData.Instance.Items.Count == 0)
            {
                this.SetupData();
            }

            ListItemData listItemData = new ListItemData();
            listItemData.item = CurrentItemData.Instance.Items.ToArray();

            return listItemData;
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
            ListItemDataArray.item         = ListItemData.ToArray();
            CurrentItemData.Instance.Items = ListItemData;
        }
    }
}