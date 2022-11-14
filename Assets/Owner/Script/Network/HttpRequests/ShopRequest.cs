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

        public static readonly string BUY_SHIP = $"{HttpRequest.BASE_URL}/api/buy-ship";

        public static readonly string BUY_SPECIAL_ITEM = $"{HttpRequest.BASE_URL}/api/buy-sp-item";

        public static readonly string GET_SPECIAL_ITEMS = $"{HttpRequest.BASE_URL}/api/get-sp-items";
        
        public static readonly string USE_SPECIAL_ITEM = $"{HttpRequest.BASE_URL}/api/use-sp-item";

        public string Token { get; private set; }

        public ShopRequest() { }

        public ShopRequest(string token) { Token = token; }

        public async Task<List<ItemData>> GetAllItems(bool isMine = false)
        {
            using var www = UnityWebRequest.Post(GET_ALL_ITEMS, new Dictionary<string, string>
            {
                { "token", isMine ? Token : "" }
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
                    { "token", Token },
                    { "itemid", itemId.ToString() }
                });

                var response = await new HttpRequest(www).Send<BuyResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        public async Task<bool> BuySpecialItem(int id, int amount)
        {
            try
            {
                using var www = UnityWebRequest.Post(BUY_SPECIAL_ITEM, new Dictionary<string, string>
                {
                    { "token", Token },
                    { "id", id.ToString() },
                    { "amount", amount.ToString() },
                });
                var response = await new HttpRequest(www).Send<BuyResponse>();
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
                { "token", isMine ? Token : "" }
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

        public async Task<bool> BuyShip(int shipID)
        {
            try
            {
                using var www = UnityWebRequest.Post(BUY_SHIP, new Dictionary<string, string>
                {
                    { "token", Token },
                    { "shipid", shipID.ToString() }
                });

                var response = await new HttpRequest(www).Send<BuyResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        
        public async Task<bool> UseSPItem(int id, int amount = 1)
        {
            try
            {
                using var www = UnityWebRequest.Post(USE_SPECIAL_ITEM, new Dictionary<string, string>
                {
                    { "token", Token },
                    { "id", id.ToString() },
                    { "amount", amount.ToString() },
                });

                var response = await new HttpRequest(www).Send<BuyResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        public async Task<List<SpecialItemResponse>> GetSPItems()
        {
            try
            {
                using var www = UnityWebRequest.Post(GET_SPECIAL_ITEMS, new Dictionary<string, string>
                {
                    { "token", Token }
                });

                var response = await new HttpRequest(www).Send<GetSpecialItemsResponse>();

                if (response.Success)
                {
                    return response.Items;
                }

                return null;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        struct GetAllShipsResponse
        {
            [JsonProperty("success")] public bool Success { get; set; }

            [JsonProperty("ships")] public List<BattleShipData> Ships { get; set; }

            [JsonProperty("error")] public string Error { get; set; }
        }

        struct GetAllItemsResponse
        {
            [JsonProperty("success")] public bool Success { get; set; }

            [JsonProperty("items")] public List<ItemData> Items { get; set; }

            [JsonProperty("error")] public string Error { get; set; }
        }

        public struct SpecialItemResponse
        {
            public struct ResourceData
            {
                [JsonProperty("addressable")] public string Addressable { get; set; }

                [JsonProperty("imageURL")] public string ImageURL { get; set; }

                [JsonProperty("name")] public string Name { get; set; }

                [JsonProperty("description")] public string Description { get; set; }

                [JsonProperty("id")] public int ID { get; set; }

                [JsonProperty("type")] public int Type { get; set; }
            }

            [JsonProperty("amount")] public int Amount { get; set; }

            [JsonProperty("resource")] public ResourceData Resource { get; set; }
        }

        struct GetSpecialItemsResponse
        {
            [JsonProperty("success")] public bool Success { get; set; }

            [JsonProperty("data")] public List<SpecialItemResponse> Items { get; set; }
        }

        struct BuyResponse
        {
            [JsonProperty("success")] public bool Success { get; set; }

            [JsonProperty("error")] public string Error { get; set; }
        }
    }
}