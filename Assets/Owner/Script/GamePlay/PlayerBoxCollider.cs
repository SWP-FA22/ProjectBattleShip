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
        public GameObject popup;
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
        private async void Update()
        {
            this.localScale.x                   = this.baseHP;
            this.healthBar.transform.localScale = this.localScale;
            this.healthStaff.text               = (this.healthAmount*100).ToString();
            if(gameObject.GetComponent<PlayerBoxCollider>().baseHP<=0){
                Debug.Log("lose");
                GameObject gameManage = GameObject.Find("GameController");
                int score = gameManage.GetComponent<GameManage>().score;
                //TODO: parse from score to resource
                int gold = score / 10 * 5;                
                ResourcesRequest resourceRequest = new ResourcesRequest(LoginUtility.GLOBAL_TOKEN);
                resourceRequest.updateResource( 2, gold);
                
                //TODO: use api to update score in server;
                PlayerRequest playerRequest = new PlayerRequest();
                playerRequest.UpdateScore(LoginUtility.GLOBAL_TOKEN, score);

                PlayerUtility playerUtility = new PlayerUtility();
                PlayerData playerData = PlayerUtility.GetMyPlayerData().Result;
                this.view.RPC("DestroyShip", RpcTarget.AllBuffered);
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
            
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Bullet"))
            {
                float dam = col.GetComponent<Bullet>().damage;
                this.view.RPC("LoseHealth", RpcTarget.AllBuffered,dam);
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