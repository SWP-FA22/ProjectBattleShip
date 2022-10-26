using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Owner.Script.Network.HttpRequests
{
    public class HttpRequest
    {
        //public const string BASE_URL = "http://103.185.184.47:8080/HttpServer";
        public const string BASE_URL = "http://103.179.185.205:8080/HttpServer";
        private UnityWebRequest www;

        public HttpRequest(UnityWebRequest www)
        {
            this.www = www;
        }

        public async Task<T> Send<T>()
        {
            var op = www.SendWebRequest();

            var start = DateTime.Now;

            Debug.Log($"Http Request: [{www.method}] {www.url}\nData: {Encoding.UTF8.GetString(www.uploadHandler.data)}");

            while (!op.isDone)
                Task.Delay(100).Wait();

            var end = DateTime.Now;
            Debug.Log($"Http Response ({end.Subtract(start).Milliseconds}ms): [{www.method}] {www.url}\nRaw Data: {www.downloadHandler.text}");

            return JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
        }
    }
}
