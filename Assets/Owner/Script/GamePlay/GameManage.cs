using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManage : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-7.5f,7.5f),Random.Range(-4,4));
        PhotonNetwork.Instantiate(Player.name,randomPosition,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
