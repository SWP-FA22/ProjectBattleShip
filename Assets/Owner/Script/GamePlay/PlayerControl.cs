using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerControl : MonoBehaviour
{
    public PhotonView view;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(view.IsMine){
            if(Input.GetKey(KeyCode.W)){
                gameObject.transform.position+= Vector3.up*speed*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.S)){
                gameObject.transform.position+= Vector3.down*speed*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.A)){
                gameObject.transform.position+= Vector3.left*speed*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.D)){
                gameObject.transform.position+= Vector3.right*speed*Time.deltaTime;
            }
            
        }
    }
}
