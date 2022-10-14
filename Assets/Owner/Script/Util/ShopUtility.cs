using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Network.HttpRequests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Assets.Owner.Script.Util
{
    public static class ShopUtility
    {
        public const string FILE_PATH = ".shop-data";

        /// <summary>
        /// Get all items of the shop or player owned items
        /// if isMine is true, then get player owned items
        /// else get all items of the shop. Default false
        /// </summary>
        /// <returns>List of ItemData</returns>
        public static async Task<List<ItemData>> GetAllItems(bool isMine = false)
        {
            try
            {
                if (!File.Exists(FILE_PATH))
                {
                    throw null;
                }
                return JsonConvert.DeserializeObject<List<ItemData>>(File.ReadAllText(FILE_PATH));
            }
            catch
            {
                List<ItemData> items = await new ShopRequest(LoginUtility.GLOBAL_TOKEN).GetAllItems(isMine);

                if (items == null)
                {
                    Debug.LogException(new Exception("Cannot get items from server"));
                    return null;
                }

                File.WriteAllText(FILE_PATH, JsonConvert.SerializeObject(items));
                return items;
            }
        }

        /// <summary>
        /// Buy item from shop with item id
        /// </summary>
        /// <param name="itemId">item id</param>
        /// <returns>true if buy successful, otherwise false</returns>
        public static async Task<bool> BuyItem(int itemId) => await new ShopRequest(LoginUtility.GLOBAL_TOKEN).BuyItem(itemId);
    }
}
