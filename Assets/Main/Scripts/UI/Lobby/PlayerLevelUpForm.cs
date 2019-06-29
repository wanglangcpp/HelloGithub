using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class PlayerLevelUpForm : NGUIForm
    {
        [SerializeField]
        private float m_DurationTime = 1.5f;

        [SerializeField]
        private Animation m_PlayerAnim = null;

        [SerializeField]
        private UILabel m_LavelTxt = null;

        private float m_CurDuration = 0;

        private const string m_PlayerAnimName = "PlayerLevelUp";

        private GameFrameworkAction m_ReturnFunction = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ReturnFunction = null;
            InitScenario(userData);
            PlayAnim(m_PlayerAnim, m_PlayerAnimName);
            m_LavelTxt.text = GameEntry.Data.Player.Level.ToString();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_CurDuration += elapseSeconds;
            if (m_CurDuration > m_DurationTime)
            {
                OnClosePanel();
            }
        }

        private void InitScenario(object userData)
        {
            m_ReturnFunction = (userData as PlayerLevelUpDisplayData).PlayerLevelUpReturn;
        }

        public void OnClosePanel()
        {
            if (m_ReturnFunction != null)
            {
                m_ReturnFunction();
            }
            GameEntry.Data.Player.IsLevelUp = false;
            CloseSelf();
        }

        private void PlayAnim(Animation anim, string name)
        {
            anim[name].time = 0;
            anim[name].speed = 1;
            anim.Play();
        }
    }
}
