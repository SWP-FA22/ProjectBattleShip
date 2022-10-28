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
        /// <summary>
        /// Get all items of the shop or player owned items
        /// if isMine is true, then get player owned items
        /// else get all items of the shop. Default false
        /// </summary>
        /// <returns>List of ItemData</returns>
        public static async Task<List<ItemData>> GetAllItems(bool isMine = false)
        {
            List<ItemData> items = await new ShopRequest(LoginUtility.GLOBAL_TOKEN).GetAllItems(isMine);

            if (items == null)
            {
                Debug.LogException(new Exception("Cannot get items from server"));
                return null;
            }

            return items;
        }


        /// <summary>
        /// Get all ships of the shop or player owned ships
        /// if isMine is true, then get player owned ships
        /// else get all ships of the shop. Default false
        /// </summary>
        /// <returns>List of ItemData</returns>
        public static async Task<List<BattleShipData>> GetAllShips(bool isMine = false)
        {
            List<BattleShipData> ships = await new ShopRequest(LoginUtility.GLOBAL_TOKEN).GetAllShips(isMine);

            if (ships == null)
            {
                Debug.LogException(new Exception("Cannot get ships from server"));
                return null;
            }

            return ships;
        }


        /// <summary>
        /// Buy item from shop with item id
        /// </summary>
        /// <param name="itemId">item id</param>
        /// <returns>true if buy successful, otherwise false</returns>
        public static async Task<bool> BuyItem(int itemId) => await new ShopRequest(LoginUtility.GLOBAL_TOKEN).BuyItem(itemId);

        /// <summary>
        /// Buy item from shop with item id
        /// </summary>
        /// <param name="shipId">ship id</param>
        /// <returns>true if buy successful, otherwise false</returns>
        public static async Task<bool> BuyShip(int shipId) => await new ShopRequest(LoginUtility.GLOBAL_TOKEN).BuyShip(shipId);
    }
}
