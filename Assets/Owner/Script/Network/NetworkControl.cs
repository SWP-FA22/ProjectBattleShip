using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Owner.Script.Util;
using Owner.Script.ShopHandle;
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
        // Delete shop data files then refetch from server
        File.Delete(ShopUtility.FILE_PATH_ITEMS_DATA);
        await ShopUtility.GetAllItems();

        File.Delete(ShopUtility.FILE_PATH_SHIPS_DATA);
        await ShopUtility.GetAllShips();

        // Load Player Data
        PlayerPrefs.DeleteAll();
        await PlayerUtility.GetMyPlayerData();
    }
}
