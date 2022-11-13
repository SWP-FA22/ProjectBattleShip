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
        
        public static readonly string EQUIP_SHIP = $"{HttpRequest.BASE_URL}/api/equip-ship";
        
        public static readonly string UPDATE_SCORE = $"{HttpRequest.BASE_URL}/api/update-score";
        
        public static readonly string TOP_PLAYER = $"{HttpRequest.BASE_URL}/api/top-player";
        
        public async Task<List<PlayerData>> TopPlayer()
            {
            using var www = UnityWebRequest.Get(TOP_PLAYER);

            try
            {
                var response = await new HttpRequest(www).Send<TopPlayerResponse>();

                if (!response.Success)
                {
                    return null;
                }

                return response.Players;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }
    
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

        public async Task<bool> EquipShip(string token, int shipID)
        {
            using var www = UnityWebRequest.Post(EQUIP_SHIP, new Dictionary<string, string>
            {
                { "token", token },
                { "shipid", shipID.ToString() }
            });

            try
            {
                var response = await new HttpRequest(www).Send<EquipShipResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        public async Task<bool> UpdateScore(string token, int score)
        {
            using var www = UnityWebRequest.Post(UPDATE_SCORE, new Dictionary<string, string>
            {
                { "token", token },
                { "score", score.ToString() }
            });

            try
            {
                var response = await new HttpRequest(www).Send<EquipShipResponse>();

                return response.Success;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        struct TopPlayerResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("players")]
            public List<PlayerData> Players { get; set; }
        }
        struct UpdateScoreResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }
        struct EquipShipResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
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
