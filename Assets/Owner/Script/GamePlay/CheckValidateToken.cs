namespace Owner.Script.GamePlay
{
    using System;
    using UnityEngine;
    using TMPro;
    using UnityEngine.SceneManagement;
    

    public class CheckValidateToken : MonoBehaviour
    {
        LoginUtility login = new LoginUtility();
        public TMP_InputField userName;
        public TMP_InputField password;
        private async void Start()
        {
           login.LoadTokenFromFile("fgf");
           bool checkToken =await login.Verfiy();
           if(checkToken){
             LoadToLoadingScene();
           }
        }
        private void LoadToLoadingScene()
        {
            SceneManager.LoadScene("LoadingScene");
        }
        public async void ValidateLogin(){
            bool result = await login.Login(userName.text,password.text);
            if(result){
                LoadToLoadingScene();
            }
        }


    }
}