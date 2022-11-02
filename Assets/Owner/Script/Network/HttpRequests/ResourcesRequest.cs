using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Owner.Script.Network.HttpRequests
{
    internal class ResourcesRequest
    {
        public static readonly string UPDATE_RESOURCE = $"{HttpRequest.BASE_URL}/api/update-resource+";
        public string Token { get; private set; }
        public ResourcesRequest()
        {

        }

        public ResourcesRequest(string token)
        {
            Token = token;
        }
        public async Task<bool> updateResource(int resourceid, int amount)
        {
            using var www = UnityWebRequest.Post(UPDATE_RESOURCE, new Dictionary<string, string>
            {
                {"token", Token },
                {"resourceid", resourceid.ToString() },
                {"amount", amount.ToString() }
            });
            try
            {
                var response = await new HttpRequest(www).Send<UpdateResourceResponse>();
                if (!response.Success)
                {
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        struct UpdateResourceResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }
    }
}
