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
                    Debug.Log("lose health");
                    
                }

                this.gameObject.SetActive(false);
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
            if (gameObject != null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}