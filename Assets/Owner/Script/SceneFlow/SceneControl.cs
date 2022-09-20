using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class SceneControl : MonoBehaviour
{
    private const string TOKEN_FILE = ".login-data";
    public void LinkToRegisterWebsite()
    {
        Application.OpenURL("http://103.185.184.47:8080/HttpServer/register");
    }

    public void LinkToResetPassWeb()
    {
        Application.OpenURL("http://103.185.184.47:8080/HttpServer/forgot");
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

    public void SignOut()
    {
        File.WriteAllText(TOKEN_FILE, "");
        SceneManager.LoadScene("LoginScene");
    }
    
    
}
