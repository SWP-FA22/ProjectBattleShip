using System.Collections.Generic;
using Assets.Owner.Script.Network.HttpRequests;
using Owner.Script.GameData;
using TMPro;
using UnityEngine;

public class LeaderBoardControl : MonoBehaviour
{
    public GameObject       PlayerRank;
    public GameObject       Parent;
    public List<GameObject> players;

    List<GameObject> a = new();
    // Start is called before the first frame update
    async void Start()
    {
        PlayerRequest    playerRequest = new PlayerRequest();
        List<PlayerData> players       = await playerRequest.TopPlayer();


        for (int i = 0; i < players.Count; i++)
            a.Add(Instantiate(PlayerRank, Parent.transform));


        Debug.Log(players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            string text = (i + 1) + ". " + players[i].Name;
            a[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
            a[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].Rank.ToString();
        }
    }

    // Update is called once per frame
    void Update() { }
}