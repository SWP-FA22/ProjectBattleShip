using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneControl : MonoBehaviour
{
    public void LinkToRegisterWebsite()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    public void LinkToResetPassWeb()
    {
        Application.OpenURL("http://unity3d.com/");
    }
    public void SetDeactive(){
        gameObject.SetActive(false);
    }
    public void BackToHome()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("HomeScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
