using UnityEngine;
using System.Collections;
using Genesis.GameClient;
using System;

namespace Genesis.GameClient
{
    public class ActivityMeleeBattleOverForm : NGUIForm
    {
        [SerializeField]
        private float m_DurationTime = 1.5f;

        [SerializeField]
        private UILabel m_MeleeOverText = null;

        [SerializeField]
        private Animation m_Animtion = null;

        private float m_CurDuration = 0;

        private const string m_PlayerAnimName = "PlayerLevelUp";

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_MeleeOverText.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_IS_OVER");
            PlayAnim(m_Animtion, m_PlayerAnimName);
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

        private void OnClosePanel()
        {
            CloseSelf();
            GameEntry.UI.OpenUIForm(UIFormId.ActivityMeleeResultForm);
        }

        private void PlayAnim(Animation anim, string name)
        {
            anim[name].time = 0;
            anim[name].speed = 1;
            anim.Play();
        }
    }
}