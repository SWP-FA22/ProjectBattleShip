namespace Owner.Script.Information
{
    using System;
    using Owner.Script.GameData;
    using TMPro;
    using UnityEngine;

    public class InformationControl : MonoBehaviour
    {
        public  TextMeshProUGUI textData;
        private HandleLocalData handleLocalData = new();
       
        private void Update()
        {
            CurrentPlayerData playerData = CurrentPlayerData.Instance;
            string            value      = $"ATK: {playerData.ATK}\nBaseHP: {playerData.BaseHP}\nSpeed: {playerData.Speed}\nRotate: {playerData.Rotate}";
            this.textData.text = value;
        }
    }
}