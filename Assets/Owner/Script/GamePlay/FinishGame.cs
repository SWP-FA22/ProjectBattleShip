namespace Owner.Script.GamePlay
{
    using System;
    using Assets.Owner.Script.Network.HttpRequests;
    using Assets.Owner.Script.Util;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;

    public class FinishGame : MonoBehaviour

    {
        public  HandleLocalData handleLocalData = new();
        public  PlayerData      playerData;
        private string          addressableOfRank;
        public  Image           RankImage;
        public  TextMeshProUGUI score;
        public  TextMeshProUGUI rankReward;
        public  TextMeshProUGUI goldReward;
        private GameObject      gameManage;
        private void Start()
        {
            this.gameManage = GameObject.Find("GameController");
            //TODO: parse from score to resource
            int              score           = CurrentPlayerData.Instance.Score;
            int              gold            = score / 10 * 5;                
            ResourcesRequest resourceRequest = new ResourcesRequest(LoginUtility.GLOBAL_TOKEN);
            resourceRequest.updateResource( 2, gold);
            this.handleLocalData = new HandleLocalData();
            PlayerData data = this.handleLocalData.LoadData<PlayerData>("PlayerData");
            //TODO: use api to update score in server;
            int rank = data.Rank;
            int scoreRank = Cal(score, rank);
            PlayerRequest playerRequest = new PlayerRequest();
            playerRequest.UpdateScore(LoginUtility.GLOBAL_TOKEN, score);
            this.score.text      = "Total Score: "+CurrentPlayerData.Instance.Score;
            this.rankReward.text = "Rank Score Reward: "+scoreRank;
            this.goldReward.text = "Gold: " + gold;
            this.ControlRank();

        }
        private void Update()
        {
            PlayerData data = this.handleLocalData.LoadData<PlayerData>("PlayerData");
            //TODO: use api to update score in server;
            int rank      = data.Rank;
            int score     = CurrentPlayerData.Instance.Score;
            int scoreRank = Cal(score, rank);
            int gold      = score / 10 * 5;       
            this.score.text      = "Total Score: "+CurrentPlayerData.Instance.Score;
            this.rankReward.text = "Rank Score Reward: "+scoreRank;
            this.goldReward.text = "Gold: " + gold;
        }

        private int Cal(int currentScore,int currentRank)
        {
            double ans = 0, k;
            if (currentRank < 1600) k      = 2.5;
            else if (currentRank < 2000) k = 2;
            else if (currentRank < 2400) k = 1.5;
            else k                         = 1;
            double predict = (currentRank - 1000) / 40 + 10;
            ans = k * (currentScore - predict);
            return (int)ans;

        }

        public void ControlRank()
        {
            this.playerData = PlayerUtility.GetMyPlayerData().Result;
            if (this.playerData == null)
            {
                FakeDataIfLoadFail fakeDataIfLoadFail = new();
                fakeDataIfLoadFail.LoadPlayerData();
                this.playerData = new HandleLocalData().LoadData<PlayerData>("PlayerData");
            }
            this.playerData.Rank = 1800;
            if(this.playerData.Rank<1600){
                this.addressableOfRank = "bonze";
            } else if(this.playerData.Rank<2000){
                this.addressableOfRank = "sliver";
            } else if(this.playerData.Rank<2400){
                this.addressableOfRank = "gold";
            } else {
                this.addressableOfRank = "master";
            }
            Addressables.LoadAssetAsync<Sprite>(addressableOfRank).Completed += (rank) => { this.RankImage.sprite = rank.Result; };
        }
    }
}