using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
public class PlayerControl : MonoBehaviour
{
    public PhotonView view;
    public GameObject camera;
    public float      speed;
    public PhotonView viewOfCannon;
    public GameObject cannon;
    public GameObject healthBar;

    
    // Start is called before the first frame update
    void Start()
    {
        view            = gameObject.GetComponent<PhotonView>();
        this.camera     = GameObject.Find("CM vcam1");
        
        //this.viewOfCannon=this.cannon.GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if(view.IsMine)
        {
            gameObject.tag                                              = "CurrentPlayer";
            gameObject.transform.GetChild(1).tag            = "CurrentPlayer";
            this.camera.GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;
            this.camera.GetComponent<CinemachineVirtualCamera>().LookAt = gameObject.transform;
            if(Input.GetKey(KeyCode.W)){
                gameObject.transform.position += transform.right * Mathf.Clamp01(1) * this.speed*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.A)){
                float rotation = 1 * this.speed*0.5f;
                transform.Rotate(Vector3.forward*rotation);
            }
            if(Input.GetKey(KeyCode.D)){
                float rotation = -1 * this.speed*0.5f;
                transform.Rotate(Vector3.forward*rotation);
            }
            this.healthBar.transform.position = gameObject.transform.position;
            

        }
    }
   
}
