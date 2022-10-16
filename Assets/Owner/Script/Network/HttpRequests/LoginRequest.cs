using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Assets.Owner.Script.Network.HttpRequests
{


    public class LoginRequest
    {
        public static readonly string LOGIN_URL = $"{HttpRequest.BASE_URL}/api/login";
        public static readonly string VERIFY_TOKEN_URL = $"{HttpRequest.BASE_URL}/api/verify";

        public string Token { get; private set; }

        public LoginRequest()
        {

        }

        public LoginRequest(string token) : this()
        {
            this.Token = token;
        }

        public async Task<bool> Login(string username, string password)
        {
            using var www = UnityWebRequest.Post(LOGIN_URL, new Dictionary<string, string>
            {
                {"username", username },
                {"password", password }
            });


            try
            {
                var response = await new HttpRequest(www).Send<LoginResponse>();

                if (!response.Success)
                {
                    return false;
                }


                this.Token = response.Token;

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        public async Task<bool> VerifyToken()
        {
            if (Token?.Length == 0) return false;

            using var www = UnityWebRequest.Post(VERIFY_TOKEN_URL, new Dictionary<string, string>
            {
                {"token", Token }
            });

            try
            {
                var response = await new HttpRequest(www).Send<VerifyTokenResponse>();

                if (!response.Success)
                    throw new System.Exception(response.Error);

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        private struct LoginResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }

        private struct VerifyTokenResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }
    }
}