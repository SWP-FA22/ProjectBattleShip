namespace Owner.Script.GamePlay
{
    using System;
    using Cysharp.Threading.Tasks;
    using Photon.Pun;
    using UnityEngine;

    public class CannonControl : MonoBehaviour
    {
        public PhotonView view;
        public GameObject bullet;
        private void Start()
        {
            view = gameObject.GetComponent<PhotonView>();
        }
        private bool state = true;
        private void Update()
        {
            if (this.view.IsMine)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward,mousePos-gameObject.transform.position);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Debug.Log("fire");
                    this.view.RPC("CreateBullet", RpcTarget.AllBuffered);
                }
            }
        }
        [PunRPC]
        public async void CreateBullet()
        {
            if (this.state)
            {
                var newBullet = Instantiate(this.bullet, gameObject.transform.position, gameObject.transform.rotation);
                
                newBullet.GetComponent<Rigidbody2D>().velocity = (this.gameObject.transform.up  * 10f);
                this.state                                       = false;
                await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
                this.state = true;
            }
            
        }
    }
}