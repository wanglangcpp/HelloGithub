using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlotDialogueForm
    {
        [Serializable]
        private class SideItem
        {
            [SerializeField]
            private GameObject m_Self = null;

            [SerializeField]
            private UILabel m_NameLabel = null;

            [SerializeField]
            private UISprite m_NameIcon = null;

            [SerializeField]
            private GameObject m_HeroSlot = null;

            [SerializeField]
            private GameObject m_PlayerSlot = null;

            [SerializeField]
            private UISprite m_PlayerIcon = null;

            public void SetVisible(bool isVisible)
            {
                m_Self.SetActive(isVisible);
            }

            public void SetNameValue(string str)
            {
                m_NameLabel.text = GameEntry.Localization.GetString(str);
            }
            public void SetPlayerName(string str)
            {
                m_NameLabel.text = str;
            }
            public void SetNameIcon(int iconId, bool isPlayer)
            {
                m_PlayerSlot.SetActive(isPlayer);
                m_HeroSlot.SetActive(!isPlayer);
                if (isPlayer)
                {
                    m_PlayerIcon.LoadAsync(iconId);
                }
                else
                {
                    m_NameIcon.LoadAsync(iconId);
                }
            }
        }
    }
}
