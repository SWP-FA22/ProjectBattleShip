namespace Owner.Script.GamePlay
{
    using System;
    using UnityEngine;
    using TMPro;
    using UnityEngine.SceneManagement;


    public class CheckValidateToken : MonoBehaviour
    {
        public TMP_InputField userName;
        public TMP_InputField password;
        public GameObject     error;
        
        private async void Start()
        {
            try
            {
                LoginUtility.LoadTokenFromFile();
                if (LoginUtility.GLOBAL_TOKEN?.Length > 0)
                {
                    bool checkToken = await LoginUtility.Verfiy();
                    if (checkToken)
                    {
                        LoadToLoadingScene();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
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
                bool result = await LoginUtility.Login(userName.text, password.text);
                if (result)
                {
                    LoginUtility.SaveTokenToFile();
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