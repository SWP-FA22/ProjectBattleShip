namespace Owner.Script.GamePlay
{
    using System;
    using Assets.Owner.Script.Util;
    using Cysharp.Threading.Tasks;
    using Owner.Script.GameData;
    using Owner.Script.GameData.HandleData;
    using Owner.Script.ShopHandle;
    using Photon.Pun;
    using UnityEngine;

    public class CannonControl : MonoBehaviour
    {
        public  PhotonView      view;
        public  GameObject      bullet;
        public  string          playerID;
        public  HandleLocalData handleLocalData;
        private ListItemData    listItemData;
        public  LoadDataItem    LoadDataItem;
        public  float           damage;
        public  float           timeRate =0;
        public  bool            checkDouble;
        public  bool            checkTriple;
        public  bool            checkSlow;
        private void Start()
        {
            view                 = gameObject.GetComponent<PhotonView>();
            this.handleLocalData = new HandleLocalData();
            this.LoadDataItem    = new LoadDataItem();
            this.ChangeStaff();
            
            
            BattleShipData battleShipData =
                handleLocalData.LoadData<BattleShipData>("ShipStaff") ??
                PlayerUtility.GetMyPlayerData().Result.Extra?.Ship;

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

        public void ChangeStaff()
        {
            //change by ship
            BattleShipData battleShipData = handleLocalData.LoadData<BattleShipData>("ShipStaff");
            if (battleShipData == null)
            {
                battleShipData = new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false };
            }
            this.damage = battleShipData.BaseAttack;
            //change by item
            this.listItemData = this.LoadDataItem.LoadData();
            PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
            foreach (var item in this.listItemData.item)
            {
                if (item.ID == playerData.CannonID || item.ID == playerData.EngineID || item.ID == playerData.SailID)
                {
                    this.damage += item.BonusATK;
                }
            }
            
            CurrentSpecialItem currentSpecialItem = CurrentSpecialItem.Instance;
            foreach (var item in currentSpecialItem.SpecialData)
            {
                this.damage   += item.Value.BonusATK;
                this.timeRate += item.Value.BonusRate;
            }
            //change by special item
            foreach (var item in CurrentSpecialItem.Instance.SpecialData)
            {
                this.damage += item.Value.BonusATK*item.Value.Amount;
                if (item.Value.IsDouble)
                {
                    this.checkDouble = true;
                }

                if (item.Value.IsTriple)
                {
                    this.checkTriple = true;
                }
            }
        }
        
        [PunRPC]
        public async void CreateBullet(float damage)
        {
            if (this.state)
            {
                if (this.checkDouble)
                {
                    this.CreateNewBullet("normal");
                    await UniTask.Delay(TimeSpan.FromMilliseconds(200));
                    this.CreateNewBullet("normal");
                    this.state = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000-this.timeRate));
                    this.state = true;
                }
                if (this.checkTriple)
                {
                    
                }

                if (this.checkSlow)
                {
                    this.CreateNewBullet("freeze");
                    this.state = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000 - this.timeRate));
                    this.state = true;
                }
                


            }
            
        }

        public async void CreateNewBullet(string type)
        {
            var newBullet = Instantiate(this.bullet, gameObject.transform.position, gameObject.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (this.gameObject.transform.up  * 15f);
            newBullet.GetComponent<Bullet>().damage        = this.damage;
            newBullet.GetComponent<Bullet>().playerID      = this.playerID;
            newBullet.GetComponent<Bullet>().bulletType     = type;
        }
    }
}