namespace Owner.Script.GamePlay
{
    using System;
    using Photon.Pun;
    using UnityEngine;

    public class CannonControl : MonoBehaviour
    {
        public  PhotonView view;
        private void Start()
        {
            view = gameObject.GetComponent<PhotonView>();
        }

        private void Update()
        {
            if (this.view.IsMine)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward,mousePos-gameObject.transform.position);
            }
        }
    }
}