using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class ServerAreaTypeItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_BtnText = null;

        [SerializeField]
        private UILabel m_BtnDisText = null;

        private GameFrameworkAction m_OnTabAction;
        public void Refresh(GameFrameworkAction onTabAction)
        {
            m_BtnText.text = GameEntry.Localization.GetString("UI_SIGN_RECOMMENDED_AREA");
            m_BtnDisText.text = m_BtnText.text;
            m_OnTabAction = onTabAction;
        }

        public void Refresh(int serverAreaCount, GameFrameworkAction onTabAction)
        {
            m_OnTabAction = onTabAction;
            m_BtnText.text = GameEntry.Localization.GetString("UI_SIGN_SELECT_A_LARGE_AREA", serverAreaCount * Constant.OneServerAreaContainMaxServerCount - 9, serverAreaCount * Constant.OneServerAreaContainMaxServerCount);
            m_BtnDisText.text = m_BtnText.text;
        }

        public void OnTab()
        {
            m_OnTabAction();
        }
    }
}