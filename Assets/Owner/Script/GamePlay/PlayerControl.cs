using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Owner.Script.GameData;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AddressableAssets;

public class PlayerControl : MonoBehaviour
{
    public string                     playerID;
    public PhotonView                 view;
    public GameObject                 camera;
    public float                      speed;
    public PhotonView                 viewOfCannon;
    public GameObject                 cannon;
    public GameObject                 healthBar;
    public TextMeshPro                playerName;
    public HandleLocalData            HandleLocalData;

    public BattleShipData battleShipData;
    public string                     shipname;
    ExitGames.Client.Photon.Hashtable PropriedadesPlayer = new ExitGames.Client.Photon.Hashtable();
    
    // Start is called before the first frame update
    void Start()
    {

        this.HandleLocalData = new HandleLocalData();
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

        this.battleShipData = HandleLocalData.LoadData<BattleShipData>("ShipStaff");
        this.speed = battleShipData.BaseSpeed;
    }

    
    
    public void ChangeModel()
    {
        this.playerID = this.view.ViewID.ToString();
        if (this.view.IsMine)
        {
            this.HandleLocalData = new HandleLocalData();
            PlayerData data     = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
            string     shipName = data.ShipName;
            Debug.Log("manage" + shipName);
            this.shipname           = shipName;
            this.view.RPC("SetData", RpcTarget.AllBuffered,shipName);
        }
        else
        {
            this.PropriedadesPlayer = PhotonNetwork.LocalPlayer.CustomProperties;
        }
       


        //this.playerID                                                                     =  PhotonNetwork.LocalPlayer.CustomProperties[PhotonNetwork.LocalPlayer.ActorNumber].ToString();

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
                float rotation = 1 * this.battleShipData.BaseRota * 0.5f;
                transform.Rotate(Vector3.forward * rotation);
            }

            if (Input.GetKey(KeyCode.D))
            {
                float rotation = -1 * this.battleShipData.BaseRota * 0.5f;
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