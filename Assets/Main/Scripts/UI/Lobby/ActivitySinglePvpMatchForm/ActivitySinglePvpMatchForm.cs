using GameFramework;
using GameFramework.Event;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivitySinglePvpMatchForm : BasePvpPrepareForm
    {
        /// <summary>
        /// 敌方的积分
        /// </summary>
        [SerializeField]
        private UILabel m_PlayerScore = null;
        /// <summary>
        /// 玩家积分
        /// </summary>
        [SerializeField]
        private UILabel m_SelfScore = null;
        /// <summary>
        /// 匹配用时
        /// </summary>
        [SerializeField]
        private UILabel m_MatchTime = null;
        /// <summary>
        /// 预计匹配时间
        /// </summary>
        [SerializeField]
        private UILabel m_AutoMatchTime = null;
        /// <summary>
        /// 匹配成功提示文字
        /// </summary>
        [SerializeField]
        private UILabel m_MatchSuccuss = null;
        /// <summary>
        /// 匹配成功计时器图标
        /// </summary>
        [SerializeField]
        private UISprite m_CompleteTimeIcon = null;
        /// <summary>
        /// 匹配成功倒计时
        /// </summary>
        [SerializeField]
        private UILabel m_CompleteTimeLabel = null;
        /// <summary>
        /// 取消按钮
        /// </summary>
        [SerializeField]
        private UIButton m_CancelMatchButton = null;
        /// <summary>
        /// 敌方队长图标
        /// </summary>
        [SerializeField]
        private GameObject m_OppLeaderSign = null;
        /// <summary>
        /// 敌方所在服务器
        /// </summary>
        [SerializeField]
        private UILabel m_OppServerName = null;
        /// <summary>
        /// 玩家所在服务器
        /// </summary>
        [SerializeField]
        private UILabel m_SelfServerName = null;
        /// <summary>
        /// 敌方头像图标
        /// </summary>
        [SerializeField]
        private GameObject m_OppIcon = null;
        /// <summary>
        /// 正在匹配背景
        /// </summary>
        [SerializeField]
        private GameObject m_SearchingTimeBg = null;
        /// <summary>
        /// 敌方阵容
        /// </summary>
        [SerializeField]
        private GameObject m_PVPDecalsRight = null;
        /// <summary>
        /// 我方阵容
        /// </summary>
        [SerializeField]
        private Transform m_SelfTeam = null;
        [SerializeField]
        private Transform m_SelfPoint = null;
        /// <summary>
        /// 敌方阵容
        /// </summary>
        [SerializeField]
        private Transform m_PlayerTeam = null;
        [SerializeField]
        private Transform m_PlayPoint = null;

        [SerializeField]
        private HeroView[] m_HeroViews = null;

        [SerializeField]
        private HeroView[] m_OppHeroViews = null;

        private float m_WaitMatchTime = 0.0f;

        private bool m_MatchSuccess = false;

        private bool m_EnterInstanceSuccess = false;

        private float m_MatchCutdownTime = 0.0f;

        private const float MatchCutdownTime = 5.0f;
        /// <summary>
        /// 当前比赛的id，暂时使用roomID
        /// </summary>
        private int m_InstanceId = 0;

        private const float AutoMatchTime = 30.0f;

        private const float m_Size = 485.0f;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //GameEntry.Event.Subscribe(EventId.PvpMatchingSuccessEventArgs, MatchPvpSuccess);
            GameEntry.Event.Subscribe(EventId.SingleMatchSuccess, MatchPvpSuccess);
            GameEntry.Event.Subscribe(EventId.GetRankData, OnGetRankScore);
            OnInitData();
        }

        protected override void OnClose(object userData)
        {
            //GameEntry.Event.Unsubscribe(EventId.PvpMatchingSuccessEventArgs, MatchPvpSuccess);
            GameEntry.Event.Unsubscribe(EventId.SingleMatchSuccess, MatchPvpSuccess);
            GameEntry.Event.Unsubscribe(EventId.GetRankData, OnGetRankScore);
            base.OnClose(userData);
        }
        /// <summary>
        /// 匹配成功后的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchPvpSuccess(object sender, GameEventArgs e)
        {
            m_SearchingTimeBg.SetActive(false);
            m_PVPDecalsRight.SetActive(true);
            m_OppIcon.SetActive(true);
            SingleMatchSuccessArgs msg = e as SingleMatchSuccessArgs;
            m_InstanceId = msg.RoomId;
            m_CancelMatchButton.gameObject.SetActive(false);
            m_MatchSuccess = true;
            m_MatchCutdownTime = MatchCutdownTime;
            m_AutoMatchTime.text = string.Empty;

            m_MatchSuccuss.gameObject.SetActive(true);
            //m_MatchSuccuss.text = GameEntry.Localization.GetString("UI_TEXT_MATCHING_COMPLETE");
            m_CompleteTimeIcon.gameObject.SetActive(true);

            m_MatchTime.text = string.Empty;
            //m_OppLeaderSign.SetActive(true);
            m_Opponent.Name.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", GameEntry.Data.PvpArenaOpponent.Player.Name, GameEntry.Data.PvpArenaOpponent.Player.Level);
            m_Opponent.Might.text = GameEntry.Data.PvpArenaOpponent.Player.TeamMight.ToString();
            m_PlayerScore.text = GameEntry.Localization.GetString("UI_TEXT_PVP_INTEGRAL", GameEntry.Data.PvpArenaOpponent.oppScore);
            m_OppPlayerId = GameEntry.Data.PvpArenaOpponent.Player.Id;
            m_Opponent.Portrait.gameObject.SetActive(true);
            m_Opponent.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(GameEntry.Data.PvpArenaOpponent.Player.PortraitType));
            //m_OppServerName.text = GameEntry.Localization.GetString("UI_TEXT_SERVER_NAME_WITH_BRACKET", GameEntry.Data.ServerNames.GetServerData(GameEntry.Data.PvpArenaOpponent.ServerId).Name);
            m_OppServerName.text = GameEntry.Localization.GetString("UI_TEXT_SERVER_NAME_WITH_BRACKET", GameEntry.Data.PvpArenaOpponent.ServerName);

            int count = GameEntry.Data.PvpArenaOpponent.HeroDatas.Count;
            for (int i = 0; i < m_OppHeroes.Length; i++)
            {
                m_OppHeroes[i].CurHP.enabled = false;
                var portrait = m_OppHeroes[i].PortraitTexture;
                var name = m_OppHeroes[i].HeroName;
                var element = m_OppHeroes[i].Element;
                var level = m_OppHeroes[i].HeroLevel;
                portrait.LoadAsync(Constant.HeroDefaultTextureId);
                name.gameObject.SetActive(i < count);
                element.gameObject.SetActive(i < count);
                level.gameObject.SetActive(false);
                if (i < count)
                {
                    portrait.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(GameEntry.Data.PvpArenaOpponent.HeroDatas[i].Type));
                    name.text = GameEntry.Data.PvpArenaOpponent.HeroDatas[i].Name;
                    element.spriteName = UIUtility.GetElementSpriteName(GameEntry.Data.PvpArenaOpponent.HeroDatas[i].ElementId);
                    //level.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", GameEntry.Data.PvpArenaOpponent.HeroDatas[i].Level);

                    m_OppHeroViews[i].InitHeroView(GameEntry.Data.PvpArenaOpponent.HeroDatas[i].Type,null, GameEntry.Data.PvpArenaOpponent.HeroDatas[i]);
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_EnterInstanceSuccess)
            {
                return;
            }
            if (m_MatchSuccess)
            {
                m_MatchCutdownTime -= realElapseSeconds;
                m_CompleteTimeLabel.text = Timer(m_MatchCutdownTime);
                //m_StartCutdownTime.text = GameEntry.Localization.GetString("UI_TEXT_PVP_COUNTDOWN", time);
                if (m_MatchCutdownTime <= 0)
                {
                    m_EnterInstanceSuccess = true;
                    if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain))
                    {
                        Log.Warning("Current procedure is not main!");
                        return;
                    }
                    GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.SinglePvp, m_InstanceId, false));
                }
            }
            else
            {
                m_WaitMatchTime += realElapseSeconds;
                m_MatchTime.text = Timer(m_WaitMatchTime);
            }
            float currentSize = m_SelfTeam.GetComponent<UISprite>().width;
            UpdataSize(currentSize);
        }
        /// <summary>
        /// UI自适应
        /// </summary>
        /// <param name="sizeX"></param>
        private void UpdataSize(float sizeX)
        {
            float X = sizeX / m_Size;
            X = Mathf.Clamp(X, 0.4f, 1f);
            m_SelfTeam.localScale = new Vector3(X, 1f, 1f);
            m_PlayerTeam.localScale = new Vector3(X, 1f, 1f);
            //int sizeSelf = m_SelfPoint.GetComponent<UISprite>().width;
            //int sizePlayer = m_PlayPoint.GetComponent<UISprite>().width;
            //sizeSelf = Mathf.Clamp(sizeSelf, 150, 380);
            //sizePlayer = Mathf.Clamp(sizePlayer, 150, 380);
            //m_SelfPoint.GetComponent<UISprite>().width = sizeSelf;
            //m_PlayPoint.GetComponent<UISprite>().width = sizePlayer;
        }

        /// <summary>
        /// 转换时间格式
        /// </summary>
        /// <param name="curtime"></param>
        private string Timer(float curtime)
        {
            int time = (int)curtime;
            StringBuilder m_Min = null; ;
            if (time >= 60)
            {
                int min = time / 60;

                if (min < 10)
                {
                    m_Min = new StringBuilder("0").Append(min.ToString()).Append(":");
                }
                else
                {
                    m_Min = new StringBuilder(min.ToString()).Append(":");
                }
                if (time % 60 < 10)
                {
                    m_Min.Append("0");
                }
                return m_Min.Append((time % 60).ToString()).ToString();
            }
            else
            {
                if (time < 10)
                {
                    m_Min = new StringBuilder("00:0");
                }
                else
                {
                    m_Min = new StringBuilder("00:");
                }
                return m_Min.Append(time.ToString()).ToString();
            }
        }

        private void OnInitData()
        {
            m_PVPDecalsRight.SetActive(false);
            m_OppIcon.SetActive(false);
            m_OppLeaderSign.SetActive(false);
            for (int i = 0; i < m_OppHeroes.Length; i++)
            {
                m_OppHeroes[i].PortraitTexture.LoadAsync(Constant.HeroDefaultTextureId);
                m_OppHeroes[i].HeroName.gameObject.SetActive(false);
                m_OppHeroes[i].Element.gameObject.SetActive(false);
                m_OppHeroes[i].HeroLevel.gameObject.SetActive(false);
            }

            //匹配预计时间
            m_AutoMatchTime.text = GameEntry.Localization.GetString("UI_TEXT_PVP_AUTOMATCH_TIME", AutoMatchTime);
            m_AutoMatchTime.gameObject.SetActive(false);

            m_OppServerName.text = string.Empty;
            m_SelfServerName.text = GameEntry.Localization.GetString("UI_TEXT_SERVER_NAME_WITH_BRACKET", GameEntry.Data.Account.ServerData.Name);
            m_EnterInstanceSuccess = false;
            m_MatchSuccess = false;
            m_CancelMatchButton.isEnabled = true;

            m_WaitMatchTime = 0.0f;

            m_MatchSuccuss.gameObject.SetActive(false);

            //m_StartCutdownTime.text = string.Empty;
            m_MatchTime.text = ((int)m_WaitMatchTime).ToString();//计时

            m_Opponent.Name.text = string.Empty;
            m_Opponent.Might.text = string.Empty;
            m_PlayerScore.text = GameEntry.Localization.GetString("UI_TEXT_POINT_WITH_QUESTION_MASK");
            m_OppPlayerId = 0;
            m_Opponent.Portrait.gameObject.SetActive(false);
            m_SelfScore.text = GameEntry.Localization.GetString("UI_TEXT_PVP_INTEGRAL", GameEntry.Data.SingleMatchData.MyScore);

            int count = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType.Count;

            for (int i = 0; i < m_MyHeroes.Length; i++)
            {
                m_MyHeroes[i].CurHP.enabled = false;

                m_MyHeroes[i].PortraitTexture.LoadAsync(Constant.HeroDefaultTextureId);
                m_MyHeroes[i].HeroName.gameObject.SetActive(i < count);
                m_MyHeroes[i].Element.gameObject.SetActive(i < count);
                m_MyHeroes[i].HeroLevel.gameObject.SetActive(false);

                bool isActive = false;
                if (i < count)
                {
                    if (!(GameEntry.Data.LobbyHeros.GetData(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType[i]) == null))
                    {
                        isActive = true;
                    }
                }

                if (isActive)
                {
                    var heroTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default);
                    m_MyHeroes[i].PortraitTexture.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(heroTeam.HeroType[i]));
                    m_MyHeroes[i].HeroName.text = GameEntry.Data.LobbyHeros.GetData(heroTeam.HeroType[i]).Name;
                    m_MyHeroes[i].Element.spriteName = UIUtility.GetElementSpriteName(GameEntry.Data.LobbyHeros.GetData(heroTeam.HeroType[i]).ElementId);
                    m_MyHeroes[i].HeroLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", GameEntry.Data.LobbyHeros.GetData(heroTeam.HeroType[i]).Level);

                    m_HeroViews[i].InitHeroView(GameEntry.Data.LobbyHeros.GetData(heroTeam.HeroType[i]).Type);
                }
                else
                {
                    m_MyHeroes[i].PortraitTexture.LoadAsync(Constant.HeroDefaultTextureId);
                    m_MyHeroes[i].HeroName.gameObject.SetActive(isActive);
                    m_MyHeroes[i].Element.gameObject.SetActive(isActive);
                    //m_MyHeroes[i].HeroLevel.gameObject.SetActive(isActive);
                }
            }
            GameEntry.LobbyLogic.StartPvpMatching(PvpType.Single);
        }

        public void OnClickCancel()
        {
            GameEntry.LobbyLogic.StopPvpMatching();
            CloseSelf();
        }

        public override void OnClickChangeHeroButton()
        {

        }

        protected override void RefreshOppData()
        {

        }

        protected override void EnterBattle()
        {

        }
        void OnGetRankScore(object sender, GameEventArgs args)
        {
            m_SelfScore.text = GameEntry.Localization.GetString("UI_TEXT_PVP_INTEGRAL", GameEntry.Data.SingleMatchData.MyScore);
        }
    }
}
