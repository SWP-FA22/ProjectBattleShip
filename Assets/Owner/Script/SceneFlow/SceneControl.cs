using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void LoadToLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void LinkToRegisterWebsite()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    public void LinkToResetPassWeb()
    {
        Application.OpenURL("http://unity3d.com/");
    }
    public void setDeactive(){
        gameObject.SetActive(false);
    }
    
    
}
