using System.Collections;
using System.Collections.Generic;
using Owner.Script.GameData;
using Owner.Script.Signals;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AddressableAssets;
using Zenject;
public class GameManage : MonoBehaviour
{

    public GameObject Player;
    public GameObject GoldBox;
    private ExitGames.Client.Photon.Hashtable playerPoperties = new ExitGames.Client.Photon.Hashtable();

    public HandleLocalData HandleLocalData;
    [Inject]
    private DiContainer diContainer;
    [Inject]
    private SignalBus signalBus;
    public TextMeshProUGUI scoreText;
    public PhotonView view;
    public List<GameObject> battleShip;
    public List<GameObject> listGoldBox;
    public int score;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        this.battleShip = new();
        this.HandleLocalData = new();
        PlayerData playerData     = this.HandleLocalData.LoadData<PlayerData>("PlayerData");
        //Vector3    randomPosition = new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0);
        GameObject player = PhotonNetwork.Instantiate(this.Player.name, new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0), Quaternion.identity);
        this.diContainer.InjectGameObject(player);
        this.battleShip.Add(player);
        this.player = player;
        
        this.playerPoperties[PhotonNetwork.LocalPlayer.ActorNumber] = playerData.Extra?.Ship.Addressable;

        PhotonNetwork.LocalPlayer.CustomProperties = this.playerPoperties;
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            this.view.RPC("GenerateGoldBox", RpcTarget.AllBuffered);
        }
        //this.signalBus.Subscribe<AddScoreSignal>(x=>UpdateScore());
        //Observer.Instance.AddObserver("UpdateScore",this.UpdateScore);

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

    [PunRPC]
    public void GenerateGoldBox()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject a = PhotonNetwork.Instantiate(this.GoldBox.name, new Vector3(Random.Range(-175f, -75f), Random.Range(-35, 35), 0), Quaternion.identity);
            this.diContainer.InjectGameObject(a);
            this.listGoldBox.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
