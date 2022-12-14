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
        public  float           timeRate = 0;
        public  bool            checkDouble;
        public  bool            checkTriple;
        public  bool            checkSlow;
        private void Start()
        {
            this.handleLocalData = new HandleLocalData();
            this.LoadDataItem    = new LoadDataItem();
            this.ChangeStaff();


            BattleShipData battleShipData =
                handleLocalData.LoadData<BattleShipData>("ShipStaff") ??
                PlayerUtility.GetMyPlayerData().Result.Extra?.Ship;
        }
        private bool state = true;
        private void Update()
        {
            if (this.view.IsMine)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - gameObject.transform.position);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    this.view.RPC("CreateBullet", RpcTarget.All, this.damage);
                }
            }
        }

        public void ChangeStaff()
        {
            //change by ship
            BattleShipData battleShipData = handleLocalData.LoadData<BattleShipData>("ShipStaff");
            if (battleShipData == null)
            {
                battleShipData = new BattleShipData
                {
                    ID         = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true,
                    IsEquipped = false
                };
            }

            this.damage += battleShipData.BaseAttack/1000;

            //change by item
            this.listItemData = this.LoadDataItem.LoadData();
            PlayerData playerData = this.handleLocalData.LoadData<PlayerData>("PlayerData");
            foreach (var item in this.listItemData.item)
            {
                if (item.ID == playerData.CannonID || item.ID == playerData.EngineID || item.ID == playerData.SailID)
                {
                    this.damage += item.BonusATK/1000;
                }
            }

            //change by special item
            foreach (var item in CurrentSpecialItem.Instance.SpecialData)
            {
                Debug.Log("number of spec:" + item.Value.CurrentUse + ", atk:" + item.Value.BonusATK);
                this.damage   += item.Value.BonusATK * item.Value.CurrentUse;
                this.timeRate += item.Value.BonusRate * item.Value.CurrentUse;
            }

            if (this.view.IsMine)
            {
                CurrentPlayerData.Instance.ATK  = this.damage;
                CurrentPlayerData.Instance.Rate = this.timeRate;
            }

            foreach (var item in CurrentSpecialItem.Instance.SpecialData)
            {
                if (item.Value.CurrentUse > 0)
                {
                    if (item.Value.IsDouble)
                    {
                        this.checkDouble = true;
                    }

                    if (item.Value.IsTriple)
                    {
                        this.checkTriple = true;
                    }

                    if (item.Value.IsFreeze)
                    {
                        this.checkSlow = true;
                    }
                }
                
            }
            
        }

        [PunRPC]
        public async void CreateBullet(float damage)
        {
            if (this.state)
            {
                this.checkSlow = false;
                if (this.checkDouble)
                {
                    this.CreateNewBullet("normal", false);
                    await UniTask.Delay(TimeSpan.FromMilliseconds(150));
                    this.CreateNewBullet("normal", false);
                    this.state = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000 - this.timeRate));
                    this.state = true;
                }

                if (this.checkTriple)
                {
                    this.CreateNewBullet("normal", true);
                    this.state = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000 - this.timeRate));
                    this.state = true;
                }

                if (this.checkSlow)
                {
                    this.CreateNewBullet("freeze", false);
                    this.state = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000 - this.timeRate));
                    this.state = true;
                }

                if (!this.checkDouble && !this.checkTriple && !this.checkSlow)
                {
                    this.CreateSingleBullet();
                    this.state                                     = false;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1000 - this.timeRate));
                    this.state = true;
                }
            }
        }

        public void CreateNewBullet(string type, bool isTriple)
        {
            if (isTriple)
            {
                int[] rotatepoint = { -20, 0, 20 };
                for (int i = 0; i < 3; i++)
                {
                    var newBullet = PhotonNetwork.Instantiate(this.bullet.name, gameObject.transform.position, gameObject.transform.rotation);
                    newBullet.transform.Rotate(new Vector3(0, 0, rotatepoint[i]));
                    //Vector3 shootPosition = this.gameObject.transform.up + new Vector3(rotatepoint[i], 0, 0);
                    newBullet.GetComponent<Bullet>().damage     = this.damage;
                    newBullet.GetComponent<Bullet>().playerID   = this.playerID;
                    newBullet.GetComponent<Bullet>().bulletType = type;
                }
            }
        }

        public void CreateSingleBullet()
        {
            var newBullet = PhotonNetwork.Instantiate(this.bullet.name, gameObject.transform.position, gameObject.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = (this.gameObject.transform.up * 15f);
            newBullet.GetComponent<Bullet>().damage        = this.damage;
            newBullet.GetComponent<Bullet>().playerID      = this.playerID;
            newBullet.GetComponent<Bullet>().bulletType    = "normal";
        }
    }
}