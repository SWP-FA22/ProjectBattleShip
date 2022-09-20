namespace Owner.Script.GamePlay
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        private void Start()
        {
            this.autoDestroy();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("des");
            if (col.transform.tag == "Player" || col.transform.tag == "island")
            {
               this.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("des");
            if (other.transform.tag == "Player" || other.transform.tag == "island")
            {
                Destroy(this.gameObject);
            }
        }
       

        public async void autoDestroy()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(2500));
            if (gameObject != null)
            {
                Destroy(this.gameObject);
            }
           
        }
    }
}