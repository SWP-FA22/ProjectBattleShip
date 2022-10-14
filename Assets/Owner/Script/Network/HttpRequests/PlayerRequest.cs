using Assets.Owner.Script.GameData;
using Newtonsoft.Json;
using Owner.Script.GameData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Owner.Script.Network.HttpRequests
{
    public class PlayerRequest
    {
        public static readonly string PLAYER_INFO = $"{HttpRequest.BASE_URL}/api/player-info";

        public async Task<PlayerData> GetPlayerInfo(int id)
        {
            using var www = UnityWebRequest.Get(PLAYER_INFO + $"?id={id}");

            try
            {
                var response = await new HttpRequest(www).Send<PlayerInfoResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Player;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        public async Task<PlayerData> GetPlayerInfo(string username)
        {
            using var www = UnityWebRequest.Get(PLAYER_INFO + $"?username={username}");

            try
            {
                var response = await new HttpRequest(www).Send<PlayerInfoResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Player;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        public async Task<PlayerData> GetPlayerExtraInfo(string token)
        {
            using var www = UnityWebRequest.Post(PLAYER_INFO, new Dictionary<string, string>
            {
                { "token", token }
            });

            try
            {
                var response = await new HttpRequest(www).Send<PlayerInfoResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Player;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        struct PlayerInfoResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("player")]
            public PlayerData Player { get; set; }
        }
    }
}
