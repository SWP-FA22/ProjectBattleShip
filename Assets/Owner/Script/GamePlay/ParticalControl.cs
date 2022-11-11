namespace Owner.Script.GamePlay
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class ParticalControl : MonoBehaviour
    {
        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(2000));
            if (this.gameObject != null)
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}