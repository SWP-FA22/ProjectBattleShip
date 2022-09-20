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
        Vector3 randomPosition = new Vector3(Random.Range(-175f,-75f),Random.Range(-35,35),0);
        PhotonNetwork.Instantiate(Player.name,new Vector3(Random.Range(-175f,-75f),Random.Range(-35,35),0),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
