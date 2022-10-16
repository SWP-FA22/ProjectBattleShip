using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using Newtonsoft.Json;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using Owner.Script.GamePlay;
using Owner.Script.ShopHandle;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AddressableAssets;

public class PlayerControl : MonoBehaviour
{
    public string          playerID;
    public PhotonView      view;
    public GameObject      camera;
    public float           speed;
    public float           speedRotate;
    public PhotonView      viewOfCannon;
    public GameObject      cannon;
    public GameObject      healthBar;
    public TextMeshPro     playerName;
    public HandleLocalData HandleLocalData;
    public int             score;
    public BattleShipData  battleShipData;
    public string          shipname;
    public LoadDataItem    LoadDataItem;

    public GameObject                 gameManage;
    ExitGames.Client.Photon.Hashtable PropriedadesPlayer = new ExitGames.Client.Photon.Hashtable();

    private ListItemData listItemData;
    // Start is called before the first frame update
    void Start()
    {
        this.speed           = 0;
        this.gameManage      = GameObject.Find("GameController");
        this.HandleLocalData = new HandleLocalData();
        this.LoadDataItem    = new LoadDataItem();
        this.listItemData    = this.LoadDataItem.LoadData();
        view                 = gameObject.GetComponent<PhotonView>();
        if (this.view.IsMine)
        {
            Debug.Log("Send message to change");
            this.ChangeModel();
        }
        this.camera                                             =  GameObject.Find("CM vcam1");
        
        string shipName = PhotonNetwork.LocalPlayer.CustomProperties[this.playerID].ToString();
        foreach (var customPropertiesKey in PhotonNetwork.LocalPlayer.CustomProperties.Values)
        {
            Debug.Log(customPropertiesKey);
        }
        Addressables.LoadAssetAsync<Sprite>(shipName).Completed += (player) => { this.gameObject.transform.GetComponent<SpriteRenderer>().sprite = player.Result; };
        this.ChangeStaff();
        
        this.cannon.GetComponent<CannonControl>().playerID =  this.playerID;
        
    }

        
    public void ChangeStaff()
    {
        //change by ship
        this.battleShipData = HandleLocalData.LoadData<BattleShipData>("ShipStaff");
        if (this.battleShipData == null)
        {
            //this.battleShipData = new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false };
            Debug.LogError("Lost Ship!");
        }
        this.speed       += battleShipData.BaseSpeed;
        this.speedRotate += this.battleShipData.BaseRota;
        //change by item
        PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
        foreach (var item in this.listItemData.item)
        {
            if (item.ID == playerData.CannonID || item.ID == playerData.EngineID || item.ID == playerData.SailID)
            {
                
                this.speed       += item.BonusSpeed;
                this.speedRotate += item.BonusRota;
            }
        }
    }

    public void UpdateScore(object obj){
        this.score+=10;
        this.gameManage.GetComponent<GameManage>().score = this.score;
        this.gameManage.GetComponent<GameManage>().scoreText.text =  "SCORE: "+this.score.ToString();
        if (this.view.IsMine)
        {
            this.view.RPC("UpdateScoreServer",RpcTarget.AllBuffered,this.score);
        }
       
    }

    [PunRPC]
    public void UpdateScoreServer(int score)
    {
        if (this.view.IsMine)
        {
            gameObject.GetComponent<PlayerControl>().score = score;
        }
        
    }

    
    
    public void ChangeModel()
    {
        this.view.RPC("SetID", RpcTarget.AllBuffered);
        if (this.view.IsMine)
        {
            this.HandleLocalData = new HandleLocalData();
            PlayerData data     = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            if (data == null)
            {
                data = new PlayerData();
            }

            this.battleShipData = data.Extra.Ship;
            string     shipName = this.battleShipData.Name;
            Debug.Log("manage" + shipName);
            this.shipname           = shipName;
            this.view.RPC("SetData", RpcTarget.AllBuffered,shipName);
        }
        else
        {
            this.PropriedadesPlayer = PhotonNetwork.LocalPlayer.CustomProperties;
        }
      
    }

    [PunRPC]
    public void SetID()
    {
        this.playerID = this.view.ViewID.ToString();
    }
    [PunRPC]
    public void SetData(string shipName)
    {
        this.PropriedadesPlayer = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!this.PropriedadesPlayer.ContainsKey(this.playerID))
        {
            Debug.Log("set key");
            PropriedadesPlayer.Add(this.playerID,shipName);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(this.PropriedadesPlayer);
    }


    void Update()
    {
        if (view.IsMine)
        {
            
            this.view.RPC("ChangeName",RpcTarget.AllBuffered,"khai1412");
            gameObject.tag                                              = "CurrentPlayer";
            gameObject.transform.GetChild(1).tag                        = "CurrentPlayer";
            this.camera.GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;
            this.camera.GetComponent<CinemachineVirtualCamera>().LookAt = gameObject.transform;
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.position += transform.right * Mathf.Clamp01(1) * this.battleShipData.BaseSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                float rotation = 1 * this.speedRotate * 0.5f;
                transform.Rotate(Vector3.forward * rotation);
            }

            if (Input.GetKey(KeyCode.D))
            {
                float rotation = -1 * this.speedRotate * 0.5f;
                transform.Rotate(Vector3.forward * rotation);
            }

            this.healthBar.transform.position = gameObject.transform.position;
        }
       
    }

    [PunRPC]
    public void ChangeName(string name)
    {
        this.playerName.text = name;
    }
}