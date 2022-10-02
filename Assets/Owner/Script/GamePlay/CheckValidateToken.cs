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
        public GameObject     error;
        private async void Start()
        {
            try
            {
                login.LoadTokenFromFile(TOKEN_FILE);
                if (this.login.Token != null)
                {
                    bool checkToken = await login.Verfiy();
                    if (checkToken)
                    {
                        LoadToLoadingScene();
                        
                        
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("agdgfjh");
            }
            
            
        }

        private void LoadToLoadingScene()
        {
            SceneManager.LoadScene("LoadingScene");
        }
        
        public async void ValidateLogin()
        {
            LoadToLoadingScene();
            if (this.userName.text == "" || this.password.text == "")
            {
                this.error.SetActive(true);
            }
            else
            {
                bool result = await login.Login(userName.text, password.text);
                if (result)
                {
                    login.SaveTokenToFile(TOKEN_FILE);
                    LoadToLoadingScene();
                }
                else
                {
                    this.error.SetActive(true);
                }
            }
            
        }
    }
}