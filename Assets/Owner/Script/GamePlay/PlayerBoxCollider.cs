namespace Owner.Script.GamePlay
{
    using System;
    using Photon.Pun;
    using Unity.VisualScripting;
    using UnityEngine;
    using Zenject;
    public class PlayerBoxCollider : MonoBehaviour
    {
        public  float      healthAmount = 0.6f;
        public  GameObject healthBar;
        private Vector3    localScale;
        public  GameObject player;
        public  PhotonView view;

        [Inject]
        private SignalBus signalBus;
        public GameObject popup;
        private void Start()
        {
            this.localScale   = this.healthBar.transform.localScale;
            this.view         = gameObject.GetComponent<PhotonView>();
            this.healthAmount = 0.6f;
            this.popup        = GameObject.Find("PopupLose");
            if (this.popup != null)
            {
                this.popup.SetActive(false);
            }
        }
        private void Update()
        {
            this.localScale.x                   = this.healthAmount;
            this.healthBar.transform.localScale = this.localScale;
            if(gameObject.GetComponent<PlayerBoxCollider>().healthAmount<=0){
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
                this.view.RPC("LoseHealth", RpcTarget.AllBuffered,0.1f);
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