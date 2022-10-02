using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Owner.Script.Util;
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
        Debug.Log("we are connected to " + PhotonNetwork.CloudRegion + " server!");
        PhotonNetwork.JoinLobby();
    }
    public override async void OnJoinedLobby()
    {
        await LoadResource();
        SceneManager.LoadScene("HomeScene");
    }

    public async Task LoadResource()
    {
        await new ShopUtility().GetAllItems();
    }
}
