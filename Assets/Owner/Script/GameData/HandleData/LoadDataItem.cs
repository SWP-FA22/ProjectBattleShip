namespace Owner.Script.GameData.HandleData
{
    using System.IO;
    using Newtonsoft.Json;
    using Owner.Script.ShopHandle;

    public class LoadDataItem
    {
        string path = "Assets/Owner/Script/TempData/TempDataItem.txt";
        public ListItemData LoadData()
        {
            ListItemData listItemData;
            
            string       jsonData = "";
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

            return null;
        }

        public void SaveData(ListItemData listItemData)
        {
            string data = JsonConvert.SerializeObject(listItemData);
            File.WriteAllText(path,data);
        }
    }
}