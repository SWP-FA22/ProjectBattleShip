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
        
    }

    private void LateUpdate()
    {
        if (this.player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("CurrentPlayer");
            this.player = playerObject.transform;
        }
        else
        {
            Vector3 newPostion = this.player.position;
            newPostion.y       = transform.position.y;
            transform.position = new Vector3(this.player.position.x,this.player.position.y,-10);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
