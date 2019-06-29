using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class NewGearLevelUpFrom : NGUIForm
    {
        [SerializeField]
        private Animation m_Animation = null;

        [SerializeField]
        private UILabel m_Text = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_Text.text = GameEntry.Localization.GetString("UI_TEXT_FORGING_SUCCESSFUL");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!m_Animation.isPlaying)
            {
                CloseSelf();
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }
    }
}
