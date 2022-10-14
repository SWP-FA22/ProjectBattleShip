using Assets.Owner.Script.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Owner.Script.Network.HttpRequests
{
    public class ShopRequest
    {
        public static readonly string GET_ALL_ITEMS = $"{HttpRequest.BASE_URL}/api/get-all-items";

        public static readonly string GET_ALL_SHIPS = $"{HttpRequest.BASE_URL}/api/get-all-ships";

        public static readonly string BUY_ITEM = $"{HttpRequest.BASE_URL}/api/buy-item";

        public string Token { get; private set; }

        public ShopRequest()
        {

        }

        public ShopRequest(string token)
        {
            Token = token;
        }

        public async Task<List<ItemData>> GetAllItems(bool isMine = false)
        {
            using var www = UnityWebRequest.Post(GET_ALL_ITEMS, new Dictionary<string, string>
            {
                {"token", isMine ? Token : "" }
            });

            try
            {
                var response = await new HttpRequest(www).Send<GetAllItemsResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Items;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        public async Task<bool> BuyItem(int itemId)
        {
            try
            {
                using var www = UnityWebRequest.Post(BUY_ITEM, new Dictionary<string, string>
                {
                    {"token", Token },
                    {"item_id", itemId.ToString() }
                });

                var response = await new HttpRequest(www).Send<BuyItemResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        public async Task<List<BattleShipData>> GetAllShips(bool isMine = false)
        {
            using var www = UnityWebRequest.Post(GET_ALL_SHIPS, new Dictionary<string, string>
            {
                {"token", isMine ? Token : "" }
            });

            try
            {
                var response = await new HttpRequest(www).Send<GetAllShipsResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Ships;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        struct GetAllShipsResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("ships")]
            public List<BattleShipData> Ships { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }
        
        struct GetAllItemsResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("items")]
            public List<ItemData> Items { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }

        struct BuyItemResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }
    }
}
