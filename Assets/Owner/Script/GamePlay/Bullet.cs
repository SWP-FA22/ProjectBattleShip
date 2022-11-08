namespace Owner.Script.GamePlay
{
    using System;
    using Cysharp.Threading.Tasks;
    using Photon.Pun;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {

        public float damage;

        public  string playerID;

        public  string     bulletType = "freeze";
        public  PhotonView photonView;
        private void Start()
        {
            this.AutoDestroy();
            this.photonView = this.GetComponent<PhotonView>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (gameObject.tag == "Bullet")
            {
                Debug.Log("des");
                if (col.transform.CompareTag("island") || col.CompareTag("Player")||col.CompareTag("GoldBox"))
                {
                    if (col.GetComponent<PlayerBoxCollider>() != null)
                    {
                        if (col.CompareTag("Player"))
                        {
                            Debug.Log("lose health");
                            GameObject otherPlayer = col.gameObject;
                            int        score       =  GameObject.Find("GameController").GetComponent<GameManage>().score;
                            if (otherPlayer.GetComponent<PlayerBoxCollider>().baseHP - this.damage <= 0)
                            {
                                Debug.Log("aaaaaaaaaaaa");
                                score                                                                       += 20;
                                GameObject.Find("GameController").GetComponent<GameManage>().score          =  score;
                                GameObject.Find("GameController").GetComponent<GameManage>().scoreText.text =  "SCORE: "+score.ToString();
                            }
                        }
                    

                    }
                    this.photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
                    
                }
            }
            
        }

        [PunRPC]
        public void DestroyBullet()
        {
            Destroy(this.gameObject);
        }

        

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("CurrentPlayer"))
            {
                gameObject.tag = "Bullet";
            } else if (other.CompareTag("Player"))
            {
                gameObject.tag = "Bullet";
            }
        }


        private async void AutoDestroy()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(2500));
            try
            {
                if (gameObject != null)
                {
                    this.photonView.RPC("DestroyBullet", RpcTarget.AllBuffered);
                   
                }
            }
            catch
            {
                
            }
            
        }
    }
}