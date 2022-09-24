using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
public class PlayerControl : MonoBehaviour
{
    public PhotonView view;
    public GameObject camera;
    public float speed;

    public GameObject canon;
    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
        this.camera = GameObject.Find("CM vcam1");
    }

    void Update()
    {
        if (view.IsMine)
        {
            this.camera.GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;
            this.camera.GetComponent<CinemachineVirtualCamera>().LookAt = gameObject.transform;
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.position += transform.right * Mathf.Clamp01(1) * this.speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                float rotation = 1 * this.speed * 0.1f;
                transform.Rotate(Vector3.forward * rotation);
            }
            if (Input.GetKey(KeyCode.D))
            {
                float rotation = -1 * this.speed * 0.1f;
                transform.Rotate(Vector3.forward * rotation);
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            canon.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - canon.transform.position);

        }
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


}
