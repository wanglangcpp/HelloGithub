using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OfflineArenaRecordItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Win = null;

        [SerializeField]
        private GameObject m_Lose = null;

        [SerializeField]
        private UILabel m_TimeLabel = null;

        [SerializeField]
        private UILabel m_RankLabel = null;

        [SerializeField]
        private PlayerPortrait m_PlayerPortrait = null;

        [SerializeField]
        private GameObject m_UpArrow = null;

        [SerializeField]
        private GameObject m_DownArrow = null;

        [SerializeField]
        private GameObject m_AttackObj = null;

        [SerializeField]
        private GameObject m_DeffenseObj = null;

        private const int OneDayTotalHours = 24;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void RefreshData(OfflineArenaBattleReportData data)
        {
            m_AttackObj.SetActive(data.Result == ReportResultType.AttackLose || data.Result == ReportResultType.AttackWin);
            m_DeffenseObj.SetActive(!(data.Result == ReportResultType.AttackLose || data.Result == ReportResultType.AttackWin));
            m_Win.SetActive(data.Result == ReportResultType.AttackWin || data.Result == ReportResultType.DefenseWin);
            m_Lose.SetActive(!(data.Result == ReportResultType.AttackWin || data.Result == ReportResultType.DefenseWin));
            TimeSpan timeSpan = GameEntry.Time.LobbyServerUtcTime - data.BattleEndTime;
            if (timeSpan.TotalHours < 1)
            {
                m_TimeLabel.text = GameEntry.Localization.GetString("UI_TEXT_FIGHTING_TIME_MINUTE", ((int)timeSpan.TotalMinutes).ToString());
            }
            else if (timeSpan.TotalDays < 1)
            {
                m_TimeLabel.text = GameEntry.Localization.GetString("UI_TEXT_FIGHTING_TIME_HOUR", ((int)timeSpan.TotalHours).ToString());
            }
            else
            {
                m_TimeLabel.text = GameEntry.Localization.GetString("UI_TEXT_FIGHTING_TIME_HOUR", OneDayTotalHours.ToString());
            }
            m_UpArrow.SetActive(data.DeltaRank < 0);
            m_DownArrow.SetActive(data.DeltaRank > 0);
            if (data.DeltaRank > 0)
            {
                m_RankLabel.text = GameEntry.Localization.GetString("UI_TEXT_NUMBER_RAD", data.DeltaRank.ToString());
            }
            else if (data.DeltaRank == 0)
            {

                m_RankLabel.text = GameEntry.Localization.GetString("UI_TEXT_CONSTANT");
            }
            else
            {
                m_RankLabel.text = GameEntry.Localization.GetString("UI_TEXT_NUMBER_GOLD", Mathf.Abs(data.DeltaRank).ToString());
            }

            m_PlayerPortrait.SetPortrait(data.Opponent);
        }
    }
}
