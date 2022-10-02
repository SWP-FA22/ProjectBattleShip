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
            using var www = new HttpClient();

            try
            {
                var response = await www.PostAsync($"{GET_ALL_ITEMS}", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"token", (isMine ? Token : "") },
                }));

                var json = JsonConvert.DeserializeObject<GetAllItemsResponse>(response.Content.ReadAsStringAsync().Result);

                if (!json.Success)
                {
                    return null;
                }

                return json.Items;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
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
    }
}
