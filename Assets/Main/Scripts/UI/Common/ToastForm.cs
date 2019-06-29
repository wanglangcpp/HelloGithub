using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ToastForm : NGUIForm
    {
        [SerializeField]
        private ToastItem m_ToastItem = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (m_ToastItem.GetAnimPlaying())
            {
                return;
            }

            var myUserData = userData as ToastDisplayData;
            if (myUserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_ToastItem.InitToast(myUserData.Message ?? string.Empty, ToastItemPlayFinished);           
        }

        private void ToastItemPlayFinished()
        {
            CloseSelf();
        }
    }
}
