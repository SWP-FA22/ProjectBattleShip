using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    public GameObject ErrorMessage;
    public TMP_InputField RoomName;
    public TMP_InputField JoinRoomName;

   
    public void CreateNewRoom()
    {
        
        if (RoomName.text != "")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(RoomName.text, roomOptions);
        }
        else
        {
            ErrorMessage.SetActive(true);
        }
    }

    public void JoinCurrentRoom()
    {
        if (JoinRoomName.text != "")
        {
            PhotonNetwork.JoinRoom(JoinRoomName.text);
            
        }
        else
        {
            ErrorMessage.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ErrorMessage.SetActive(true);
        Debug.Log("fail");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }


    public void JoinRandomRoom()
    {
        if (PhotonNetwork.CountOfRooms == 0)
        {
            ErrorMessage.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }

    }
}
