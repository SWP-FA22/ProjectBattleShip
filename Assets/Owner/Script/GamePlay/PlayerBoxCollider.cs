namespace Owner.Script.GamePlay
{
    using System;
    using Photon.Pun;
    using Unity.VisualScripting;
    using UnityEngine;
    using Zenject;
    using TMPro;
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
        public  TextMeshPro healthStaff;
        
        [Inject]
        private SignalBus signalBus;
        public GameObject popup;
        private void Start()
        {
            this.HandleLocalData = new HandleLocalData();
            this.battleShipData  = HandleLocalData.LoadData<BattleShipData>("ShipStaff");
            this.localScale      = this.healthBar.transform.localScale;
            this.view            = gameObject.GetComponent<PhotonView>();
            this.healthAmount    = this.battleShipData.BaseHP;
            this.popup           = GameObject.Find("PopupLose");
            if (this.popup != null)
            {
                this.popup.SetActive(false);
            }
            
        }
        private void Update()
        {
            this.localScale.x                   = this.baseHP;
            this.healthBar.transform.localScale = this.localScale;
            this.healthStaff.text               = (this.healthAmount*100).ToString();
            if(gameObject.GetComponent<PlayerBoxCollider>().baseHP<=0){
                Debug.Log("lose");
                if (this.view.IsMine)
                {
                    this.popup.SetActive(true);
                }
                this.view.RPC("DestroyShip", RpcTarget.AllBuffered);
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
            player.SetActive(false);
        }
    }
}