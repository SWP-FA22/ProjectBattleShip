namespace Owner.Script.GamePlay
{
    using System;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.ShopHandle;
    using Owner.Script.Signals;
    using Photon.Pun;
    using Unity.VisualScripting;
    using UnityEngine;
    using Zenject;
    using TMPro;
    using Photon.Pun.Demo.PunBasics;
    using Assets.Owner.Script.Network.HttpRequests;
    using Assets.Owner.Script.Util;
    using Assets.Owner.Script.GameData;
    using Cysharp.Threading.Tasks;

    public class PlayerBoxCollider : MonoBehaviour
    {
        public  float           healthAmount = 0.6f;
        public  GameObject      healthBar;
        private Vector3         localScale;
        public  GameObject      player;
        public  PhotonView      view;
        public  HandleLocalData HandleLocalData;
        public  BattleShipData  battleShipData;
        public  float           baseHP = 1.6f;
        public  TextMeshPro     healthStaff;
        public  string          playerID;
        public  int             score;
        private ListItemData    listItemData;
        public  LoadDataItem    LoadDataItem;
        
        [Inject]
        private SignalBus signalBus;
        public  GameObject popup;
        private bool       check = true;
        private void Start()
        {
            this.score           = 0;
            this.HandleLocalData = new HandleLocalData();
            this.LoadDataItem    = new LoadDataItem();
            this.localScale      = this.healthBar.transform.localScale;
            this.view            = gameObject.GetComponent<PhotonView>();
            this.ChangeStaff();
            this.popup           = GameObject.Find("PopupLose");
            if (this.popup != null)
            {
                this.popup.SetActive(false);
            }
            
        }
        private int Cal(int currentScore,int currentRank)
        {
            double ans = 0,k;
            if (currentRank < 1600) k = 2.5;
            else if (currentRank < 2000) k = 2;
            else if (currentRank < 2400) k = 1.5;
            else k = 1;
            double predict = (currentRank - 1000) / 40 + 10;
            ans = k * (currentScore - predict);
            return (int)ans;

        }
        private void Update()
        {
            this.localScale.x                   = this.baseHP;
            this.healthBar.transform.localScale = this.localScale;
            this.healthStaff.text               = (this.healthAmount*100).ToString();
            if(gameObject.GetComponent<PlayerBoxCollider>().baseHP<=0&&check){
                this.healthBar.SetActive(false);
                Debug.Log("lose");
                GameObject gameManage = GameObject.Find("GameController");
                int score = gameManage.GetComponent<GameManage>().score;
                //TODO: parse from score to resource
                int gold = score / 10 * 5;                
                ResourcesRequest resourceRequest = new ResourcesRequest(LoginUtility.GLOBAL_TOKEN);
                resourceRequest.updateResource( 2, gold);
                this.HandleLocalData = new HandleLocalData();
                PlayerData data = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
                //TODO: use api to update score in server;
                int rank = data.Rank;
                score = Cal(score, rank);
                PlayerRequest playerRequest = new PlayerRequest();
                playerRequest.UpdateScore(LoginUtility.GLOBAL_TOKEN, score);

                PlayerUtility playerUtility = new PlayerUtility();
                PlayerData playerData = PlayerUtility.GetMyPlayerData().Result;
                this.view.RPC("DestroyShip", RpcTarget.AllBuffered);
                this.check = false;
            }
        }

        public void ChangeStaff()
        {
            //change by ship
            this.battleShipData = HandleLocalData.LoadData<BattleShipData>("ShipStaff");
            if (this.battleShipData == null)
            {
                this.battleShipData = new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false };
            }
            this.healthAmount = this.battleShipData.BaseHP;
            
            //change by item
            this.listItemData = this.LoadDataItem.LoadData();
            PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            foreach (var item in this.listItemData.item)
            {
                if (item.ID == playerData.CannonID || item.ID == playerData.EngineID || item.ID == playerData.SailID)
                {
                    this.healthAmount += item.BonusHP;
                }
            }
            
            CurrentSpecialItem currentSpecialItem = CurrentSpecialItem.Instance;
            foreach (var item in currentSpecialItem.SpecialData)
            {
                this.healthAmount += item.Value.BonusHP;
            }
            //change by special item
            foreach (var item in CurrentSpecialItem.Instance.SpecialData)
            {
                this.healthAmount += item.Value.BonusHP*item.Value.Amount;
            }
            
            
        }

        private async void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Bullet"))
            {
                float  dam          = col.GetComponent<Bullet>().damage;
                string typeOfBullet = col.GetComponent<Bullet>().bulletType;
                this.view.RPC("LoseHealth", RpcTarget.AllBuffered,dam);
                if (typeOfBullet == "freeze")
                {
                    float speed = gameObject.transform.parent.GetComponent<PlayerControl>().speed;
                    float tempspeed                                                           = speed * 0.8f;
                    gameObject.transform.parent.GetComponent<PlayerControl>().speed =  tempspeed;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
                    gameObject.transform.parent.GetComponent<PlayerControl>().speed = speed;
                }
                Debug.Log(gameObject.tag+", "+gameObject.GetComponent<PlayerBoxCollider>().healthAmount);
            }
        }
        
        [PunRPC]
        public void LoseHealth(float lose)
        {
            if (gameObject.GetComponent<PlayerBoxCollider>() != null)
            {
                Debug.Log("lose health rpc");
                if (gameObject.GetComponent<PlayerBoxCollider>().healthAmount > 0)
                {
                    gameObject.GetComponent<PlayerBoxCollider>().baseHP       -= (lose*this.healthAmount/this.baseHP);
                    gameObject.GetComponent<PlayerBoxCollider>().healthAmount -= lose;

                }
                
            }
            
        }

        [PunRPC]
        public void DestroyShip(){
            ListCurrentPlayers.Instance.listPlayer.Remove(gameObject);
            this.popup.SetActive(true);
            player.SetActive(false);
        }

        [PunRPC]
        public void AddScore(string playerID)
        {
            if (this.playerID == playerID)
            {
                this.score += 10;
                this.signalBus.Fire(new AddScoreSignal{Score = this.score});
            }
        }
    }
}