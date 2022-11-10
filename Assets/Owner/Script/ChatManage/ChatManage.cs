namespace Owner.Script.ChatManage
{
    using System;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using Photon.Chat;
    using Photon.Chat.Demo;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;

    public class ChatManage : MonoBehaviour, IChatClientListener
    {
        private ChatClient chatClient;
        private bool       isConnected;
        
        public TMP_InputField        ChatField;
        public TextMeshProUGUI       message;
        public List<TextMeshProUGUI> listMessage = new();
        public GameObject            parent;
        public TextMeshProUGUI       chatDisplay;

        [SerializeField] private string userID;

        private void Start()
        {
            this.chatClient            = new ChatClient(this);
            chatClient.ChatRegion      = "asia";
            this.chatClient.AuthValues = new Photon.Chat.AuthenticationValues(this.userID);
            ChatAppSettings chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
            this.chatClient.ConnectUsingSettings(chatAppSettings);
            //this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues("khai1412"));
            Debug.Log("connecting");
            
            
        }

        private void Update()
        {
            this.chatClient.Service();
            
        }
        public void DebugReturn(DebugLevel level, string message)
        {
            Debug.Log("run here");
        }
        public void OnDisconnected()
        {
            Debug.Log("close");
        }
        public void OnConnected()
        {
            this.isConnected = true;
            Debug.Log("connected");
            this.chatClient.Subscribe(new string[] { "RegionChannel" });
        }

        public void OnChatStateChange(ChatState state)
        {
            this.isConnected = true;
        }


        public void SendPublicChat()
        {
            this.chatClient.PublishMessage("RegionChannel", this.ChatField.text);
            this.ChatField.text = "";
        }
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            string message = "";
            for (int i = 0; i < senders.Length; i++)
            {
                message               =  string.Format("{0}: {1}", senders[i], messages[i]);
                this.chatDisplay.text += "\n" + message;
            }
        }
        public  void OnPrivateMessage(string sender, object message, string channelName)      { throw new NotImplementedException(); }
        public void OnSubscribed(string[] channels, bool[] results)
        {
            Debug.Log("subcribed");
        }
        public  void OnUnsubscribed(string[] channels)                                        { throw new NotImplementedException(); }
        public  void OnStatusUpdate(string user, int status, bool gotMessage, object message) { throw new NotImplementedException(); }
        public  void OnUserSubscribed(string channel, string user)                            { throw new NotImplementedException(); }
        public  void OnUserUnsubscribed(string channel, string user)                          { throw new NotImplementedException(); }
    }
}