using System.Collections;
using System.Collections.Generic;
using Owner.Script.GameData;
using Owner.Script.GameData.HandleData;
using Owner.Script.Signals;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject;
public class GameManage : MonoBehaviour
{

    public  GameObject                        Player;
    public  GameObject                        GoldBox;
    private ExitGames.Client.Photon.Hashtable playerPoperties = new ExitGames.Client.Photon.Hashtable();
    public  FakeDataIfLoadFail                FakeDataIfLoadFail;
    public  HandleLocalData                   HandleLocalData;
    [Inject]
    private DiContainer diContainer;
    [Inject]
    private SignalBus signalBus;
    public TextMeshProUGUI  scoreText;
    public PhotonView       view;
    public List<GameObject> battleShip;
    public List<GameObject> listGoldBox;
    public int              score;
    public int              check = 0;
    public GameObject       player;
    
    public TMP_InputField        ChatField;
    public TextMeshProUGUI       message;
    public List<TextMeshProUGUI> listMessage = new();
    public GameObject            parent;


    void Start()
    {
        this.battleShip = new();
        this.HandleLocalData = new();
        PlayerData playerData     = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
        if (playerData == null)
        {
            this.FakeDataIfLoadFail = new FakeDataIfLoadFail();
            playerData              = this.FakeDataIfLoadFail.LoadPlayerData();
        }
        //Vector3    randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
        this.player = PhotonNetwork.Instantiate(this.Player.name, new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0), Quaternion.identity);
        this.diContainer.InjectGameObject(player);
        this.battleShip.Add(player);
          
        
        this.playerPoperties[PhotonNetwork.LocalPlayer.ActorNumber] = playerData.Extra?.Ship.Addressable;

        PhotonNetwork.LocalPlayer.CustomProperties = this.playerPoperties;
            
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject a = PhotonNetwork.Instantiate(this.GoldBox.name, new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0), Quaternion.identity);
                this.diContainer.InjectGameObject(a);
                this.listGoldBox.Add(a);
            }
        }

    }

    public void SendMessage()
    {
        this.view.RPC("CreateNewMessage", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void CreateNewMessage()
    {
        TextMeshProUGUI message = Instantiate(this.message, this.parent.transform);
        message.text = this.ChatField.text;
    }

   
    public void UpdateScore(object obj)
    {
        Debug.Log("cap nhap diem");
        this.score += 10;
        int currentScore = this.score;
        this.scoreText.text = "SCORE: " + this.score.ToString();
        foreach (var item in this.battleShip)
        {
            item.transform.GetChild(0).GetComponent<PlayerControl>().score = this.score;
        }
        this.view.RPC("UpdateScoreServer", RpcTarget.AllBuffered, this.score);

    }

    [PunRPC]
    public void UpdateScoreServer(int score)
    {
        this.player.transform.GetChild(0).GetComponent<PlayerControl>().score = score;
    }
    
    

    

    // Update is called once per frame
    void Update()
    {
        if (this.ChatField.isFocused)
        {
            //player cannot move or do anything
        }
    }
}
