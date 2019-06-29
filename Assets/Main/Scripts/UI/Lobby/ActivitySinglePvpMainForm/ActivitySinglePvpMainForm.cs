using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivitySinglePvpMainForm : NGUIForm
    {
        /// <summary>
        /// 赛季
        /// </summary>
        [SerializeField]
        private UILabel m_DescriptionLabel = null;

        [SerializeField]
        private UILabel m_PvpRank = null;

        [SerializeField]
        private UILabel m_RankLable = null;

        [SerializeField]
        private UILabel m_PvpInteral = null;

        [SerializeField]
        private UILabel m_InteralLable = null;

        /// <summary>
        /// 开放时间
        /// </summary>
        [SerializeField]
        private UILabel m_PvpPointLabel = null;

        /// <summary>
        /// 挑战次数
        /// </summary>
        [SerializeField]
        private UILabel m_RemainChallengeCountLabel = null;

        /// <summary>
        /// 段位图标
        /// </summary>
        [SerializeField]
        private UITexture m_TitleIcon = null;

        /// <summary>
        /// 段位名字
        /// </summary>
        [SerializeField]
        private UILabel m_TitleName = null;

        //[SerializeField]
        //private GameObject[] m_RankList = null;

        [SerializeField]
        private float m_ChangeDuration = 1f;

        /// <summary>
        /// 消息提示
        /// </summary>
        [SerializeField]
        private UILabel m_NotOpenActivityNote = null;

        /// <summary>
        /// 开始按键
        /// </summary>
        [SerializeField]
        private UIButton m_MatchButton = null;

        private bool m_StartMatch = false;

        private const int WeekDayNum = 7;

        DRPvpRank[] PvpRankData = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            PvpRankData = GameEntry.DataTable.GetDataTable<DRPvpRank>().GetAllDataRows();
            Reset();
            GameEntry.Event.Subscribe(EventId.SinglePvpInfoChanged, GetSingleInfoSuccess);
            //获取服务器的匹配的排名数据，暂时无
            //GameEntry.LobbyLogic.GetPvpInfo();
            //获取竞技场布阵信息
            //GameEntry.LobbyLogic.RefreshOfflineArena(false);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(EventId.SinglePvpInfoChanged, GetSingleInfoSuccess);
            base.OnClose(userData);
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_StartMatch = false;
        }

        private void GetSingleInfoSuccess(object sender, GameEventArgs e)
        {
            m_StartMatch = false;
            OnRefreshData();
        }

        private void OnRefreshData()
        {
            //显示可挑战的次数
            // m_RemainChallengeCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_NUMBER", GameEntry.Data.PvpArena.ChallengeCount);

            //显示当前得分和排名，shouldPlayAnim是否显示积分变动的动画
            //int rank = 0;
            //int score = 0;

            //bool shouldPlayAnim = false;
            //if (GameEntry.Data.HasTempData(Constant.TempData.SinglePvpShouldPlayRankScoreAnims))
            //{
            //    shouldPlayAnim = GameEntry.Data.GetAndRemoveTempData<bool>(Constant.TempData.SinglePvpShouldPlayRankScoreAnims);
            //}

            //if (shouldPlayAnim && (GameEntry.Data.PvpArena.Rank != GameEntry.Data.PvpArena.LastRank || GameEntry.Data.PvpArena.Score != GameEntry.Data.PvpArena.LastScore))
            //{
            //    rank = GameEntry.Data.PvpArena.LastRank;
            //    score = GameEntry.Data.PvpArena.LastScore;
            //}
            //else
            //{
            //    rank = GameEntry.Data.PvpArena.Rank;
            //    score = GameEntry.Data.PvpArena.Score;
            //}

            //m_RankLabel.text = rank.ToString();
            //m_PvpPointLabel.text = GameEntry.Localization.GetString("UI_TEXT_PVP_1V1_POINT", score);

            //for (int i = 1; i < m_RankList.Length; i++)
            //{
            //    m_RankList[i].SetActive(GameEntry.Data.PvpArena.Rank == i);
            //}

            //m_RankList[0].SetActive(GameEntry.Data.PvpArena.Rank > 3);

            //m_RankList[m_RankList.Length - 1].SetActive(rank <= 0);

            //if (shouldPlayAnim)
            //{
            //    if (GameEntry.Data.PvpArena.Rank > 3 && rank > 3)
            //    {
            //        TweenNumber.Begin(m_RankLabel.gameObject, m_ChangeDuration, GameEntry.Data.PvpArena.Rank);
            //    }
            //    else
            //    {
            //        m_RankLabel.text = GameEntry.Data.PvpArena.Rank.ToString();
            //    }

            //    TweenNumber.Begin(m_PvpPointLabel.gameObject, m_ChangeDuration, GameEntry.Data.PvpArena.Score);
            //}
            //else
            //{
            //    m_RankLabel.text = GameEntry.Data.PvpArena.Rank.ToString();
            //    m_PvpPointLabel.text = GameEntry.Localization.GetString("UI_TEXT_PVP_1V1_POINT", GameEntry.Data.PvpArena.Score);
            //}

            //根据PvpArena.Score来显示当前pvp的等级
            //int finalScore = GameEntry.Data.PvpArena.Score;
            //var dataTable = GameEntry.DataTable.GetDataTable<DRPvpTitle>();
            //var rows = dataTable.GetAllDataRows();
            //DRPvpTitle drPvpTitle = null;
            //for (int i = 0; i < rows.Length; i++)
            //{
            //    if (rows[i].TitleMaxScore >= finalScore && finalScore >= rows[i].TitleMinScore)
            //    {
            //        drPvpTitle = rows[i];
            //    }
            //}
            //if (drPvpTitle == null)
            //{
            //    Log.Error("ActivitySinglePvpMainForm Can't get DRPvpTitle,Score is '{0}'.", finalScore);
            //    return;
            //}

            //m_TitleIcon.LoadAsync(drPvpTitle.TitleTextureId);
            //m_TitleName.text = GameEntry.Localization.GetString(drPvpTitle.TitleName);

            //{0}第{1}赛季（{2}-{3}）
            //DateTime now = GameEntry.Time.LobbyServerTime;
            //int week = now.DayOfWeek == DayOfWeek.Sunday ? WeekDayNum : (int)now.DayOfWeek;
            //DateTime monday = now.AddDays(1 - week);
            //DateTime sunday = now.AddDays(WeekDayNum - 1);
            //m_DescriptionLabel.gameObject.SetActive(true);
            //m_DescriptionLabel.text = GameEntry.Localization.GetString("UI_TEXT_PVP_SEASON_NAME_TEXT", now.Year.ToString(), GameEntry.Data.PvpArena.Season, UIUtility.GetTimeSpanStringYmd(monday), UIUtility.GetTimeSpanStringYmd(sunday));
        }

        public void OnClickAutoMatch()
        {
            if (m_StartMatch)
            {
                return;
            }

            m_StartMatch = true;
            GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpMatchForm);
        }

        public void OnClickTeamBtn()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.PvpArenaBattle, });
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.Lobby });
        }

        public void OnClickRankBtn()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.PvpArenaBattle, });
            GameEntry.UI.OpenUIForm(UIFormId.RankListForm, new RankListDisplayData { Scenario = RankListType.PvpMain });
        }

        public void OnClickShopBtn()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.PvpArenaBattle, });
            GameEntry.UI.OpenUIForm(UIFormId.ShopForm, new ShopDisplayData { Scenario = ShopScenario.OfflineArena });
        }
        private void Reset()
        {
            //for (int i = 0; i < m_RankList.Length; i++)
            //{
            //    m_RankList[i].SetActive(false);
            //}
            //m_DescriptionLabel.gameObject.SetActive(false);
            //m_PvpRank.text = GameEntry.Localization.GetString("UI_TEXT_PVP_RANK");//, UnityEngine.Random.Range(1, 100));
            int m_Rank = GameEntry.Data.SingleMatchData.Rank;

            if (m_Rank == 0)
            {
                m_RankLable.text = GameEntry.Localization.GetString("UI_TEXT_PVP_NOT_ON_LIST");
            }
            else
            {
                m_RankLable.text = GameEntry.Data.SingleMatchData.Rank.ToString();
            }
            //m_PvpInteral.text = GameEntry.Localization.GetString("UI_TEXT_PVP_SCORE");//, UnityEngine.Random.Range(10, 1000));
            m_InteralLable.text = GameEntry.Data.SingleMatchData.MyScore.ToString();
            m_PvpPointLabel.text = GameEntry.Localization.GetString("UI_TEXT_PVP_STARTTIME", "20:00-12:00");
            m_TitleName.text = GameEntry.Localization.GetString(GetPvpGrading(GameEntry.Data.SingleMatchData.MyScore, true));
            m_RemainChallengeCountLabel.text = string.Empty;

            bool isOpen = IsOpen();
            m_MatchButton.isEnabled = isOpen;
            if (isOpen)
            {
                m_NotOpenActivityNote.text = "";
            }
            else
            {
                TimeSpan startTime = TimeSpan.Parse("4:00:00"/*GameEntry.ServerConfig.GetString(Constant.ServerConfig.PvpStartTimeEveryday, "4:00:00")*/);
                TimeSpan endTime = TimeSpan.Parse("16:00:00"/*GameEntry.ServerConfig.GetString(Constant.ServerConfig.PvpEndTimeEveryday, "16:00:00")*/);
                DateTime now = GameEntry.Time.LobbyServerUtcTime;
                DateTime todayStartTime = now.Date.Add(startTime);
                DateTime todayEndTime = now.Date.Add(endTime);
                todayStartTime = todayStartTime.ToLocalTime();
                todayEndTime = todayEndTime.ToLocalTime();
                m_NotOpenActivityNote.text = GameEntry.Localization.GetString("UI_TEXT_ACTIVITY_OPEN_TIME_QUANTUM",
                    GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", todayStartTime.Hour, todayStartTime.Minute, todayStartTime.Second),
                    GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", todayEndTime.Hour, todayEndTime.Minute, todayEndTime.Second));
            }
        }
        /// <summary>
        /// 获取积分对应的信息
        /// </summary>
        /// <param name="integral">积分</param>
        /// <param name="isname">需要的内容（true-段位名称/false-段位图标）</param>
        /// <returns></returns>
        public string GetPvpGrading(int integral, bool isname)
        {
            for (int i = 0; i < PvpRankData.Length; i++)
            {
                if (integral >= PvpRankData[i].MinIntegral && integral <= PvpRankData[i].MaxIntegral)
                {
                    if (isname)
                    {
                        return PvpRankData[i].GradingName;
                    }
                    else
                    {
                        return PvpRankData[i].GradingIcon;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 当前是否打开匹配状态
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return true;
            TimeSpan startTime = TimeSpan.Parse("4:00:00"/*GameEntry.ServerConfig.GetString(Constant.ServerConfig.PvpStartTimeEveryday, "4:00:00")*/);
            TimeSpan endTime = TimeSpan.Parse("16:00:00"/*GameEntry.ServerConfig.GetString(Constant.ServerConfig.PvpEndTimeEveryday, "16:00:00")*/);
            DateTime now = GameEntry.Time.LobbyServerUtcTime;
            DateTime todayStartTime = now.Date.Add(startTime);
            DateTime todayEndTime = now.Date.Add(endTime);
            if (now < todayStartTime || now > todayEndTime)
            {
                return false;
            }
            if (GameEntry.Data.Player.Level < 30/*GameEntry.ServerConfig.GetInt(Constant.ServerConfig.PvpRequirePlayerLevel, 30)*/)
            {
                return false;
            }
            return true;
        }

    }
}
