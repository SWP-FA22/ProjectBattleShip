using Assets.Owner.Script.GameData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingControl : MonoBehaviour
{
    public GameObject                         CurrentRankingText;
    public GameObject                         Parent;
    public List<GameObject>                   players;
    public Dictionary<GameObject, GameObject> listPlayerRank;
    List<GameObject>                          a =new();
    int                                       temp;

    private int count = 1;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newRank = Instantiate(CurrentRankingText, Parent.transform);
        a.Add(newRank) ;
        this.temp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        players = ListCurrentPlayers.Instance.listPlayer;
        while (this.temp < players.Count)
        {
            a.Add(Instantiate(CurrentRankingText, Parent.transform));
            this.temp++;
        }

        while (this.temp > this.players.Count)
        {
            Destroy(a[0].gameObject);
            this.a.Remove(a[0]);
            this.temp--;
        }

        for (int i = 0; i < this.count; i++)
        {
            if (players[i] != null)
            {
                string text                   = (i + 1) + ". " + players[i].GetComponent<PlayerControl>().playerName.text;
                if (text.Length > 12) text    = text.Substring(0, 12);
                while (text.Length < 16) text = text + " ";
                text += players[i].GetComponent<PlayerControl>().score;
            
                a[i].GetComponent<TextMeshProUGUI>().text = "";
                a[i].GetComponent<TextMeshProUGUI>().text = text;
            }
            else
            {
                Destroy(a[i].gameObject);
                this.a.Remove(a[i]);
            }
        }

        if (this.players.Count != this.count)
        {
            this.count = this.players.Count;
        }

    }
}
