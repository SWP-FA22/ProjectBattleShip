namespace Owner.Script.GamePlay
{
    using System;
    using Cysharp.Threading.Tasks;
    using Photon.Pun;
    using UnityEngine;

    public class CannonControl : MonoBehaviour
    {
        public PhotonView      view;
        public GameObject      bullet;
        public string          playerID;
        public HandleLocalData handleLocalData;

        public float damage;
        private void Start()
        {
            view = gameObject.GetComponent<PhotonView>();
            this.handleLocalData = new HandleLocalData();
            BattleShipData battleShipData = handleLocalData.LoadData<BattleShipData>("ShipStaff");
            if (battleShipData == null)
            {
                battleShipData = new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false };
            }
            this.damage = battleShipData.BaseAttack;
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
                    
                    this.view.RPC("CreateBullet", RpcTarget.AllBuffered,this.damage);
                }
            }
        }
        
        [PunRPC]
        public async void CreateBullet(float damage)
        {
            if (this.state)
            {
                var newBullet = Instantiate(this.bullet, gameObject.transform.position, gameObject.transform.rotation);
                newBullet.GetComponent<Rigidbody2D>().velocity = (this.gameObject.transform.up  * 15f);
                newBullet.GetComponent<Bullet>().damage        = this.damage;
                newBullet.GetComponent<Bullet>().playerID      = this.playerID;
                this.state                                     = false;
                await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
                this.state = true;
                
            }
            
        }
    }
}