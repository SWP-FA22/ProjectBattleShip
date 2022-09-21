namespace Owner.Script.GamePlay
{
    using System;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PlayerBoxCollider : MonoBehaviour
    {
        public  float      healthAmount = 1;
        public  GameObject healthBar;
        private Vector3    localScale;
        private void Start()
        {
            this.localScale = this.healthBar.transform.localScale;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("an dan r");
            this.healthAmount -= 0.1f;
            if (col.transform.tag == "Bullet")
            {
                this.localScale.x                   = this.healthAmount;
                this.healthBar.transform.localScale = this.localScale;
            }
        }
    }
}