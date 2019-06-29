using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class GuideToast : NGUIForm
    {

        [SerializeField]
        private GuideToastItem m_ToastItem = null;

        protected override void OnOpen(object userData) {
            m_ToastItem.InitToast(GameEntry.Localization.GetString("UI_TEXT_TASK_CLICK"));
        }

        public void Fire() {

            m_ToastItem.PlayAnimation(ToastItemPlayFinished);
        }

        private void ToastItemPlayFinished()
        {
            Log.Debug("CC ToastItemPlayFinished");
        }
    }
}
