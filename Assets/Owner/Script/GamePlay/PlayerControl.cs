using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using Owner.Script.GamePlay;
using Owner.Script.ShopHandle;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AddressableAssets;
using Assets.Owner.Script.GameData;
using Unity.VisualScripting;

public class PlayerControl : MonoBehaviour
{
    private const byte            EVENT_CODE = 1;
    public        string          playerID;
    public        PhotonView      view;
    public        GameObject      camera;
    public        float           speed;
    public        float           speedRotate;
    public        PhotonView      viewOfCannon;
    public        GameObject      cannon;
    public        GameObject      healthBar;
    public        TextMeshPro     playerName;
    public        HandleLocalData HandleLocalData;
    public        int             score;
    public        BattleShipData  battleShipData;
    public        string          shipname;
    public        LoadDataItem    LoadDataItem;
    
    public TMP_InputField        ChatField;
    public TextMeshProUGUI       message;
    public List<TextMeshProUGUI> listMessage = new();
    public GameObject            parent;


    public GameObject                 gameManage;
    ExitGames.Client.Photon.Hashtable PropriedadesPlayer = new ExitGames.Client.Photon.Hashtable();

    private ListItemData listItemData;

    private GamePlayData gamePlayData;

    private void OnEnable()
    {
        
    }
    
    public void SendMessage()
    {
        this.view.RPC("CreateNewMessage", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void CreateNewMessage()
    {
        TextMeshProUGUI message = Instantiate(this.message, this.parent.transform);
        message.text = this.ChatField.text;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= this.GetScoreEvent;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        this.speed                       = 0;
        this.gameManage                  = GameObject.Find("GameController");
        this.HandleLocalData             = new HandleLocalData();
        this.LoadDataItem                = new LoadDataItem();
        this.gamePlayData                = new GamePlayData { ShipName = "ship5", Score = 0 };
        this.listItemData                = this.LoadDataItem.LoadData();
        ListCurrentPlayers.Instance.listPlayer.Add(gameObject);
        if (this.listItemData == null)
        {
            this.LoadDataItem.SetupData();
            this.listItemData = this.LoadDataItem.LoadData();
        }
        view                 = gameObject.GetComponent<PhotonView>();
        if (this.view.IsMine)
        {
            Debug.Log("Send message to change");
            this.ChangeModel();
            CurrentPlayerData.Instance.Score = 0;
        }
        
        this.camera                                             =  GameObject.Find("CM vcam1");
        foreach (var customPropertiesKey in PhotonNetwork.LocalPlayer.CustomProperties.Values)
        {
            Debug.Log("custom key"+customPropertiesKey);
        }
        //gamePlayData = (GamePlayData)PhotonNetwork.LocalPlayer.CustomProperties[this.playerID];
        string       shipName     = PhotonNetwork.LocalPlayer.CustomProperties[this.playerID].ToString().Split("|")[0];
        if(shipName==""){
            shipName="ship5";
        }
        
        Addressables.LoadAssetAsync<Sprite>(shipName).Completed += (player) => { this.gameObject.transform.GetComponent<SpriteRenderer>().sprite = player.Result; };
        this.ChangeStaff();
        
        this.cannon.GetComponent<CannonControl>().playerID =  this.playerID;
        

        PhotonNetwork.NetworkingClient.EventReceived += this.GetScoreEvent;

    }

    public void GetScoreEvent(EventData obj)
    {
        Debug.Log(obj.Code);
        if (obj.Code == 253)
        {
            Debug.Log("event id"+obj.CustomData);
            
            if (this.playerID == obj.CustomData)
            {
                this.score += 10;
            }
        }
        
    }

   
    public void ChangeStaff()
    {
        //change by ship
        this.battleShipData = HandleLocalData.LoadData<BattleShipData>("ShipStaff");
        if (this.battleShipData == null)
        {
            this.battleShipData = new BattleShipData { ID = 1, Name = "ship3", Description = "aaaaaa", BaseAttack = 0.5f, BaseHP = 2.0f, BaseSpeed = 5f, BaseRota = 5f, Price = 10, Addressable = "ship1", IsOwner = true, IsEquipped = false };
            this.HandleLocalData.SaveData("ShipStaff",this.battleShipData);
            // Debug.LogError("Lost Ship!");
        }
        this.speed                        += battleShipData.BaseSpeed;
        this.speedRotate                  += this.battleShipData.BaseRota;

        //change by item
        PlayerData playerData = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
        foreach (var item in this.listItemData.item)
        {
            if (item.ID == playerData.CannonID || item.ID == playerData.EngineID || item.ID == playerData.SailID)
            {
                
                this.speed                        += item.BonusSpeed;
                this.speedRotate                  += item.BonusRota;
            }
        }
        //change buy special item
        foreach (var item in CurrentSpecialItem.Instance.SpecialData)
        {
            this.speed                       += item.Value.BonusSpeed*item.Value.CurrentUse;
        }

        if (this.view.IsMine)
        {
            CurrentPlayerData.Instance.Speed = this.speed;
            CurrentPlayerData.Instance.Rotate  =this.speedRotate;
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
        this.score = score;
        //this.gameManage.GetComponent<GameManage>().score = this.score;
        int index = ListCurrentPlayers.Instance.listPlayer.FindIndex(x => x.GetComponent<PlayerControl>().playerID == this.playerID);
        while (index>0 && ListCurrentPlayers.Instance.listPlayer[index].GetComponent<PlayerControl>().score> ListCurrentPlayers.Instance.listPlayer[index-1].GetComponent<PlayerControl>().score)
        {
            GameObject temp = ListCurrentPlayers.Instance.listPlayer[index];
            ListCurrentPlayers.Instance.listPlayer[index] = ListCurrentPlayers.Instance.listPlayer[index - 1];
            ListCurrentPlayers.Instance.listPlayer[index - 1] = temp;
            index--;
        }
        //ListCurrentPlayers.Instance.listPlayer[index].GetComponent<PlayerControl>().score = this.score;
    }
    private void OnDestroy()
    {
        ListCurrentPlayers.Instance.listPlayer.Remove(this.gameObject);
        if (this.view.IsMine)
        {
            CurrentPlayerData.Instance.ATK          = 0;
            CurrentPlayerData.Instance.Speed        = 0;
            CurrentPlayerData.Instance.Rotate       = 0;
            CurrentPlayerData.Instance.BaseHP       = 0;
            CurrentPlayerData.Instance.SpecialItems = new List<string>();
        }
        
        foreach (var item in CurrentSpecialItem.Instance.SpecialData)
        {
            item.Value.CurrentUse = 0;
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
            this.shipname              = shipName;
            this.gamePlayData.ShipName = this.shipname;
            this.view.RPC("SetData", RpcTarget.AllBuffered,this.shipname);
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
            
            int tempScore = GameObject.Find("GameController").GetComponent<GameManage>().score;
            this.view.RPC("UpdateScoreServer",RpcTarget.AllBuffered,tempScore);
            this.healthBar.transform.position = gameObject.transform.position;
        }
       
    }

    [PunRPC]
    public void ChangeName(string name)
    {
        this.playerName.text = name;
    }
}