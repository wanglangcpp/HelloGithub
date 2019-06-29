using GameFramework.DataTable;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlayerInfoForm
    {
        [Serializable]
        private class SelfInfo
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
            private UILabel m_HerosNum = null;

            [SerializeField]
            private UILabel m_HerosLimit = null;

            [SerializeField]
            private UILabel m_GearLevelLimit = null;

            [SerializeField]
            private UISlider m_ExperienceBg = null;

            [SerializeField]
            private UILabel m_PlayerId = null;

            [SerializeField]
            private PlayerHeadView m_HeadPortrait = null;

            [SerializeField]
            private UILabel m_NowExp = null;

            [SerializeField]
            private UILabel m_PowerRecoverTime = null;

            public void InitSelfInfoPanel()
            {
                var playerData = GameEntry.Data.Player;
                m_PlayerName.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_NAME", playerData.Name);
                m_VipLevel.text = playerData.VipLevel.ToString();

                m_MightNum.text = GameEntry.Localization.GetString("UI_TEXT_GS", GameEntry.Data.Player.TeamMight);
                m_HerosNum.text = GameEntry.Localization.GetString("UI_TEXT_HEROSNUMBER", GameEntry.Data.LobbyHeros.Data.Count);
                m_HerosLimit.text = GameEntry.Localization.GetString("UI_TEXT_HEROLEVELLIMIT", playerData.Level);
                m_GearLevelLimit.text = GameEntry.Localization.GetString("UI_TEXT_GEARLEVELLIMIT", playerData.Level);
                m_PlayerId.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERID", playerData.DisplayId);

                m_HeadPortrait.InitPlayerHead(playerData.PortraitType);
                IDataTable<DRPlayer> dt = GameEntry.DataTable.GetDataTable<DRPlayer>();

                DRPlayer dr = dt.GetDataRow(playerData.Level);
                if (dr == null)
                {
                    return;
                }

                m_ExperienceBg.gameObject.SetActive(true);
                m_ExperienceBg.value = playerData.ExpRatio;
                m_Level.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_LEVEL", playerData.Level);
                m_NowExp.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", playerData.Exp, dr.LevelUpExp);

                UpdateEnergyRecoveryTime(GameEntry.PlayerEnergy.EnergyRecoveryLeftTime);
            }

            public void UpdateEnergyRecoveryTime(TimeSpan leftTime)
            {
                m_PowerRecoverTime.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERINFO_PHYSICAL_STRENGTH", leftTime.Minutes, leftTime.Seconds);
            }
        }
    }
}
