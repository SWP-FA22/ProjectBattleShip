using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assets.Owner.Script.Network.HttpRequests;
using Assets.Owner.Script.Util;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using Owner.Script.ShopHandle;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkControl : MonoBehaviourPunCallbacks
{
    public FakeDataIfLoadFail FakeDataIfLoadFail = new();
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
        this.FakeDataIfLoadFail.LoadSpecialItemData();
        List<ShopRequest.SpecialItemResponse> listSpecialFromServer =  await ShopUtility.GetSPItems();
        foreach (var item in listSpecialFromServer)
        {
            var dkm = CurrentSpecialItem.Instance.SpecialData.FirstOrDefault(e => e.Value.ID == item.Resource.ID);
            if (dkm.Value is not null)
            {
                dkm.Value.Amount = item.Amount;

            }
        }
        // Load Player Data
        PlayerPrefs.DeleteAll();
        await PlayerUtility.GetMyPlayerData();
    }
}
