namespace Owner.Script.SoundControl
{
    using System;
    using UnityEngine;

    public class MainSound : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}