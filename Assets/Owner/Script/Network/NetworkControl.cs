using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkControl : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("we are connected to "+PhotonNetwork.CloudRegion+" server!");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        
    }
}
