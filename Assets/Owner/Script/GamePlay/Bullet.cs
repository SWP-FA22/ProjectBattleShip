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
        private void   Start() { this.AutoDestroy(); }

        private void OnTriggerEnter2D(Collider2D col)
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
                Destroy(gameObject);
            }
        }

        

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("CurrentPlayer"))
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
                    Destroy(this.gameObject);
                }
            }
            catch
            {
                
            }
            
        }
    }
}