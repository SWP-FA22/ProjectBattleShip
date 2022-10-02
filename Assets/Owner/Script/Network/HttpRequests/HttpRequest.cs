using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Owner.Script.Network.HttpRequests
{
    public class HttpRequest
    {
        //public const string BASE_URL = "http://103.185.184.47:8080/HttpServer";
        public const string BASE_URL = "http://127.0.0.1:9999/HttpServer";


        private UnityWebRequest www;

        public HttpRequest(UnityWebRequest www)
        {
            this.www = www;
        }

        public async Task<T> Send<T>()
        {
            var op = www.SendWebRequest();

            while (!op.isDone)
                await Task.Yield();
            Debug.Log(www.downloadHandler.text);
            return JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
        }
    }
}
