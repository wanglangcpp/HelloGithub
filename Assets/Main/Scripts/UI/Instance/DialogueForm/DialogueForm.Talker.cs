using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    public partial class DialogueForm
    {
        [Serializable]
        private class Talker
        {
            [SerializeField]
            private GameObject m_Self = null;

            [SerializeField]
            private UILabel m_NameLabel = null;

            [SerializeField]
            private GameObject m_HeroSlot = null;

            [SerializeField]
            private UISprite m_NameIcon = null;

            [SerializeField]
            private GameObject m_PlayerSlot = null;

            [SerializeField]
            private UISprite m_PlayerIcon = null;

            public void SetVisible(bool isVisible)
            {
                m_Self.SetActive(isVisible);
            }

            public void SetNameValue(string str, bool isPlayer)
            {
                if (isPlayer)
                {
                    m_NameLabel.text = GameEntry.Data.Player.Name;
                }
                else
                {
                    m_NameLabel.text = GameEntry.Localization.GetString(str);
                }

            }

            public void SetNameIcon(int iconId, bool isPlayer)
            {
                m_PlayerSlot.SetActive(isPlayer);
                m_HeroSlot.SetActive(!isPlayer);
                if (iconId == -1)
                {
                    return;
                }
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
