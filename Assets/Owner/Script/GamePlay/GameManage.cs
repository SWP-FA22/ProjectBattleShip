using System.Collections;
using System.Collections.Generic;
using Owner.Script.GameData;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AddressableAssets;
using Zenject;
public class GameManage : MonoBehaviour
{
    
    public  GameObject                        Player;
    public  GameObject                        GoldBox;
    private ExitGames.Client.Photon.Hashtable playerPoperties = new ExitGames.Client.Photon.Hashtable();

    public HandleLocalData HandleLocalData;
    // Start is called before the first frame update
    void Start()
    {
        this.HandleLocalData = new();
        Vector3    randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
        GameObject a              =PhotonNetwork.Instantiate(this.Player.name,new Vector3(Random.Range(-175f,-75f),Random.Range(-35,35),0),Quaternion.identity);
        this.playerPoperties[PhotonNetwork.LocalPlayer.ActorNumber] = "ship3";
        PhotonNetwork.LocalPlayer.CustomProperties                  = this.playerPoperties;
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            this.GenerateGoldBox();
        }

    }

    public void GenerateGoldBox()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject a              =PhotonNetwork.Instantiate(this.GoldBox.name,new Vector3(Random.Range(-175f,-75f),Random.Range(-35,35),0),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
