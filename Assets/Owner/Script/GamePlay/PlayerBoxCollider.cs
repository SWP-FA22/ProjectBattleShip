namespace Owner.Script.GamePlay
{
    using System;
    using Photon.Pun;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PlayerBoxCollider : MonoBehaviour
    {
        public  float      healthAmount = 1;
        public  GameObject healthBar;
        private Vector3    localScale;
        public  GameObject player;
        public  PhotonView view;
        private void Start()
        {
            this.localScale = this.healthBar.transform.localScale;
            this.view       = gameObject.GetComponent<PhotonView>();
        }
        private void Update()
        {
            this.localScale.x                   = this.healthAmount;
            this.healthBar.transform.localScale = this.localScale;
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
                gameObject.GetComponent<PlayerBoxCollider>().healthAmount -= lose;
            }
            
        }
    }
}