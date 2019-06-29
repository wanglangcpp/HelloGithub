using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlayerInfoForm
    {
        [Serializable]
        private class PlayerInfo
        {
            [SerializeField]
            private UILabel m_PlayerName = null;

            [SerializeField]
            private UILabel m_VipLevel = null;

            [SerializeField]
            private UILabel m_Level = null;

            [SerializeField]
            private UILabel m_MightNum = null;

            [SerializeField]
            private GameObject m_Stranger = null;

            [SerializeField]
            private GameObject m_Friend = null;

            [SerializeField]
            private UILabel m_PlayerId = null;

            [SerializeField]
            private PlayerHeadView m_HeadPortrait = null;

            [SerializeField]
            private UILabel m_HerosNum = null;

            public void InitPlayerInfoPanel(bool isFriend, PBPlayerInfo playerData)
            {
                if (playerData == null)
                {
                    return;
                }
                SetAllVisible(true);
                m_Stranger.SetActive(!isFriend);
                m_Friend.SetActive(isFriend);

                if (playerData.HasName)
                {
                    m_PlayerName.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_NAME", playerData.Name);
                }

                if (playerData.HasVipLevel)
                {
                    m_VipLevel.text = playerData.VipLevel.ToString();
                }

                if (playerData.HasLevel)
                {
                    m_Level.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_LEVEL", playerData.Level);
                }

                m_MightNum.text = GameEntry.Localization.GetString("UI_TEXT_GS", playerData.Might);
                m_PlayerId.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERID", playerData.DisplayId);
                m_HeadPortrait.InitPlayerHead(playerData.PortraitType);

                if (playerData.HasPlayerHeroCount)
                {
                    m_HerosNum.text = GameEntry.Localization.GetString("UI_TEXT_HEROSNUMBER", playerData.PlayerHeroCount);
                }
            }

            public void HideAll()
            {
                SetAllVisible(false);
            }

            private void SetAllVisible(bool visible)
            {
                m_PlayerName.gameObject.SetActive(visible);
                m_VipLevel.gameObject.SetActive(visible);
                m_Level.gameObject.SetActive(visible);
                m_MightNum.gameObject.SetActive(visible);
                m_Stranger.SetActive(visible);
                m_Friend.SetActive(visible);
            }
        }
    }
}
