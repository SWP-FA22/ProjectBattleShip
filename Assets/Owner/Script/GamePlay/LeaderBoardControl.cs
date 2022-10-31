using Assets.Owner.Script.GameData;
using Assets.Owner.Script.Network.HttpRequests;
using Owner.Script.GameData;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class LeaderBoardControl : MonoBehaviour
{
    public GameObject PlayerRank;
    public GameObject Parent;
    public List<GameObject> players;
    List<GameObject> a = new();
    // Start is called before the first frame update
    async void Start()
    {
        PlayerRequest playerRequest = new PlayerRequest();
        List<PlayerData> players = await playerRequest.TopPlayer();
        
       
        for (int i = 0; i < players.Count; i++)
        a.Add(Instantiate(PlayerRank, Parent.transform));
        
        
        Debug.Log(players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            string text = (i + 1) + ". " + players[i].Name;
            if (text.Length > 12) text = text.Substring(0, 12);
            while (text.Length < 16) text = text + " ";
            text += players[i].Rank;
            Debug.Log(text);
            a[i].GetComponent<TextMeshProUGUI>().text = "";
            a[i].GetComponent<TextMeshProUGUI>().text = text;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
