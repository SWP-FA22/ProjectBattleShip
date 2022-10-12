namespace Owner.Script.GamePlay
{
    using System;
    using Photon.Pun;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class GoldBoxControl : MonoBehaviour
    {
        public  float      healthAmount = 1.6f;
        public  GameObject healthBar;
        private Vector3    localScale;
        public  PhotonView view;
        public  float      baseHP = 1.6f;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("island"))
            {
                Vector3 randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
                gameObject.transform.position = randomPosition;
            }
            if (col.CompareTag("Bullet"))
            {
                float dam = col.GetComponent<Bullet>().damage;
                this.view.RPC("LoseHealth", RpcTarget.AllBuffered,dam);
               
            }
        }
        private void Start()
        {
            this.localScale = this.healthBar.transform.localScale;
        }
        private void Update()
        {
            this.localScale.x                   = this.baseHP;
            this.healthBar.transform.localScale = this.localScale;
            if(gameObject.GetComponent<GoldBoxControl>().baseHP<=0){
                this.view.RPC("DestroyGoldBox", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        public void LoseHealth(float lose)
        {
            if (gameObject.GetComponent<GoldBoxControl>() != null)
            {
                Debug.Log("gold box lose health");
                if (gameObject.GetComponent<GoldBoxControl>().healthAmount > 0)
                {
                    gameObject.GetComponent<GoldBoxControl>().baseHP       -= (lose*this.healthAmount/this.baseHP);
                    gameObject.GetComponent<GoldBoxControl>().healthAmount -= lose;

                }
                
            }
            
        }
        [PunRPC]

        public void DestroyGoldBox()
        {
            gameObject.SetActive(false);
        }
    }
}