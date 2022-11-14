namespace Owner.Script.ChatManage
{
    using DG.Tweening;
    using UnityEngine;
    using Zenject;

    public class ChatUIManage : MonoBehaviour
    {
        private          bool       isShow = true;
        [Inject] private SignalBus  signalBus;
        public           GameObject ChatField;
        public           int        addPosition;
        public void ControlChat()
        {
            Vector3 position = this.ChatField.transform.position;
            if (this.isShow)
            {
                this.ChatField.transform.DOMove(new Vector3(position.x-this.addPosition, position.y, position.z), 0.5f).SetEase(Ease.InOutSine);
                this.isShow = false;
            }
            else
            {
                this.ChatField.transform.DOMove(new Vector3(position.x+this.addPosition, position.y, position.z), 0.5f).SetEase(Ease.InOutSine);
                this.isShow = true;
            }
            
        }
    }
}