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
    public class ShopUtility
    {
        private const string FILE_PATH = ".shop-data";

        public string Token { get; private set; }

        public async Task<List<ItemData>> GetAllItems()
        {
            try
            {
                if (!File.Exists(FILE_PATH))
                {
                    throw null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<ItemData>>(File.ReadAllText(FILE_PATH));
                }
            }
            catch
            {
                List<ItemData> items = await new ShopRequest().GetAllItems();
                File.WriteAllText(FILE_PATH, JsonConvert.SerializeObject(items));
                return items;
            }
        }

        public void LoadTokenFromFile(string filename)
        {
            try
            {
                string token = File.ReadAllText(filename);
                Debug.Log(token);
                if (token?.Length > 0)
                {
                    Token = token;
                }
            }
            catch (System.Exception)
            {
                Token = "";
            }
        }
    }
}
