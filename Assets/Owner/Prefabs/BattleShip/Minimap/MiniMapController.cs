using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("CurrentPlayer");
        this.player = playerObject.transform;
    }

    private void LateUpdate()
    {
        Vector3 newPostion = this.player.position;
        newPostion.y       = transform.position.y;
        transform.position = newPostion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
