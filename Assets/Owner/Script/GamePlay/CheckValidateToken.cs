namespace Owner.Script.GamePlay
{
    using System;
    using UnityEngine;
    using TMPro;
    using UnityEngine.SceneManagement;


    public class CheckValidateToken : MonoBehaviour
    {
        LoginUtility login = new LoginUtility();

        private const string TOKEN_FILE = ".login-data";

        public TMP_InputField userName;
        public TMP_InputField password;

        private async void Start()
        {
            login.LoadTokenFromFile(TOKEN_FILE);
            bool checkToken = await login.Verfiy();
            if (checkToken)
            {
                LoadToLoadingScene();
            }
        }

        private void LoadToLoadingScene()
        {
            SceneManager.LoadScene("LoadingScene");
        }
        
        public async void ValidateLogin()
        {
            bool result = await login.Login(userName.text, password.text);
            if (result)
            {
                login.SaveTokenToFile(TOKEN_FILE);
                LoadToLoadingScene();
            }
        }
    }
}