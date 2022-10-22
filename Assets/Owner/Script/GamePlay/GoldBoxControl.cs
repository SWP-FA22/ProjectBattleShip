namespace Owner.Script.GamePlay
{
    using System;
    using ExitGames.Client.Photon;
    using Owner.Script.Signals;
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    public class GoldBoxControl : MonoBehaviour
    {
        private const byte       EVENT_CODE   = 1;
        public        float      healthAmount = 1.6f;
        public        GameObject healthBar;
        private       Vector3    localScale;
        public        PhotonView view;
        public        float      baseHP = 1.6f;
        public        string     playerID;
        private       GameObject player;
        [Inject]
        private SignalBus signalBus;
        ExitGames.Client.Photon.Hashtable PropriedadesPlayer = new ExitGames.Client.Photon.Hashtable();
        public GameObject                 mainPlayer;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("island"))
            {
                Vector3 randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
                gameObject.transform.position = randomPosition;
            }
            if (col.CompareTag("Bullet"))
            {
                float dam = col.GetComponent<Bullet>().damage;
                this.view.RPC("LoseHealth", RpcTarget.AllBuffered,dam);
                this.playerID = col.gameObject.GetComponent<Bullet>().playerID;

            }
        }
        private void Start()
        {
            this.localScale = this.healthBar.transform.localScale;
            this.mainPlayer = GameObject.Find("GameController");
            
        }
        private void Update()
        {
            this.localScale.x                   = this.baseHP;
            this.healthBar.transform.localScale = this.localScale;
            if(gameObject.GetComponent<GoldBoxControl>().baseHP<=0){
                this.player = GameObject.FindWithTag("CurrentPlayer");
                int score = this.player.GetComponent<PlayerControl>().score;
                score += 10;
                string shipName="ship3";
                if (PhotonNetwork.LocalPlayer.CustomProperties[this.playerID] != null)
                {
                    shipName = PhotonNetwork.LocalPlayer.CustomProperties[this.playerID].ToString();
                }
                PhotonNetwork.LocalPlayer.CustomProperties[playerID] = shipName;
                this.PropriedadesPlayer[playerID]                    = shipName;
                PhotonNetwork.LocalPlayer.SetCustomProperties(this.PropriedadesPlayer);
                this.view.RPC("DestroyGoldBox", RpcTarget.AllBuffered,this.playerID);
                
                
                
            }
        }

        [PunRPC]
        public void LoseHealth(float lose)
        {
            if (gameObject.GetComponent<GoldBoxControl>() != null)
            {
                
                if (gameObject.GetComponent<GoldBoxControl>().healthAmount > 0)
                {
                    gameObject.GetComponent<GoldBoxControl>().baseHP       -= (lose*this.healthAmount/this.baseHP);
                    gameObject.GetComponent<GoldBoxControl>().healthAmount -= lose;
                }
                
            }
            
        }
        [PunRPC]
        public void DestroyGoldBox(string playerID)
        {
            
            if (this.player!=null&&player.GetComponent<PlayerControl>().playerID == playerID)
            {
                this.PropriedadesPlayer = PhotonNetwork.LocalPlayer.CustomProperties;
                int score = this.player.GetComponent<PlayerControl>().score;
                Debug.Log("get shoot");
                score                                                                       += 10;
                GameObject.Find("GameController").GetComponent<GameManage>().score          =  score;
                GameObject.Find("GameController").GetComponent<GameManage>().scoreText.text =  "SCORE: "+score.ToString();
                
                this.healthAmount = 1.6f;
                this.baseHP       = 1.6f;
                Vector3 randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
                gameObject.transform.position = randomPosition;
            }
            
        }
        
    }
}