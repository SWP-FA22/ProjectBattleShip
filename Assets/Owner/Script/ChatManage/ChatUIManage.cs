namespace Owner.Script.ChatManage
{
    using UnityEngine;
    using Zenject;

    public class ChatUIManage : MonoBehaviour
    {
        private          bool       isShow = true;
        [Inject] private SignalBus  signalBus;
        public           GameObject ChatField;
        public void ControlChat()
        {
            if (this.isShow)
            {
                this.ChatField.SetActive(false);
                this.isShow = false;
            }
            else
            {
                this.ChatField.SetActive(true);
                this.isShow = true;
            }
            
        }
    }
}