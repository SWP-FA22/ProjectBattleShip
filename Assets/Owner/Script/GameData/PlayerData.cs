using Assets.Owner.Script.GameData;
using Newtonsoft.Json;
using System;

namespace Owner.Script.GameData
{
    public class PlayerData
    {
        // EXTRA ATTRIBUTES

        public class ExtraData
        {
            [JsonProperty("ship"), JsonRequired]
            public BattleShipData Ship { get; set; }

            [JsonProperty("gold")]
            public int Gold { get; set; }

            [JsonProperty("ruby")]
            public int Ruby { get; set; }

            [JsonProperty("diamond")]
            public int Diamond { get; set; }

            [JsonProperty("weapon")]
            public ItemData Weapon { get; set; }

            [JsonProperty("engine")]
            public ItemData Engine { get; set; }

            [JsonProperty("sail")]
            public ItemData Sail { get; set; }
        }

        [JsonProperty("extra"), JsonRequired]
        public ExtraData Extra { get; set; }

        // BASE ATTRIBUTES

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("weaponID")]
        public int CannonID { get; set; }

        [JsonProperty("sailID")]
        public int SailID { get; set; }

        [JsonProperty("engineID")]
        public int EngineID { get; set; }

        public float CalcHP()
        {
            var result = this.Extra?.Ship?.BaseHP ?? 0.0f;
            return CustomCalcHP?.Invoke(result) ?? result;
        }

        public float CalcATK()
        {
            var result = this.Extra?.Ship?.BaseAttack ?? 0.0f;
            return CustomCalcAttack?.Invoke(result) ?? result;
        }

        public float CalcSpeed()
        {
            var result = this.Extra?.Ship?.BaseSpeed ?? 0.0f;
            return CustomCalcSpeed?.Invoke(result) ?? result;
        }

        public float CalcRota()
        {
            var result = this.Extra?.Ship?.BaseRota ?? 0.0f;
            return CustomCalcRota?.Invoke(result) ?? result;
        }

        public Func<float, float> CustomCalcHP { get; set; }

        public Func<float, float> CustomCalcAttack { get; set; }

        public Func<float, float> CustomCalcSpeed { get; set; }

        public Func<float, float> CustomCalcRota { get; set; }
    }
}