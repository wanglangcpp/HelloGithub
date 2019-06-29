using GameFramework;
using GameFramework.Fsm;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技战斗副本结算界面基类。
    /// </summary>
    public abstract partial class BaseArenaBattleResultForm : NGUIForm
    {
        [SerializeField]
        protected Transform[] m_SubPanels = null;

        [SerializeField]
        protected Transform m_RankSubPanel = null;

        [SerializeField]
        protected float m_RankSubPanelDuration = 3f;

        [SerializeField]
        protected Transform m_RewardSubPanel = null;

        [SerializeField]
        protected UILabel m_ArenaCoinObtained = null;

        [SerializeField]
        protected UILabel m_ArenaTokenObtained = null;

        [SerializeField]
        protected GeneralItemView[] m_ItemsObtained = null;

        [SerializeField]
        protected Player m_Me = null;

        [SerializeField]
        protected Player m_Opponent = null;

        protected OfflineArenaBattleResultDataObtainedEventArgs m_UserData = null;
        protected IFsm<BaseArenaBattleResultForm> m_Fsm = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_UserData = ParseUserData(userData);
            if (m_UserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            if (m_UserData.MyNewRank == m_UserData.MyOldRank)
            {
                m_Fsm = GameEntry.Fsm.CreateFsm(this, new InitState(), new RewardState());
            }
            else
            {
                m_Fsm = GameEntry.Fsm.CreateFsm(this, new InitState(), new RankState(), new RewardState());
            }

            m_Fsm.Start<InitState>();
        }

        protected override void OnClose(object userData)
        {
            m_UserData = null;
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        // Called by NGUI via reflection.
        public void OnClickNextButton()
        {
            GameEntry.SceneLogic.GoBackToLobby();
        }

        // Called by NGUI via reflection.
        public void OnClickWholeScreenButton()
        {
            (m_Fsm.CurrentState as StateBase).OnClickWholeScreenButton(m_Fsm);
        }

        protected void Reset()
        {
            for (int i = 0; i < m_SubPanels.Length; ++i)
            {
                m_SubPanels[i].gameObject.SetActive(false);
            }
        }

        protected abstract bool OnRankStateUpdate(ref bool myAnimationHasStarted, ref bool oppAnimationHasStarted, ref bool oppAnimationHasStopped, ref float oppAnimationStopTime, IFsm<BaseArenaBattleResultForm> fsm);

        protected abstract OfflineArenaBattleResultDataObtainedEventArgs ParseUserData(object userData);

        private bool HasGotReward
        {
            get
            {
                return m_UserData.ArenaTokenObtained > 0;
            }
        }

        [Serializable]
        protected class Player
        {
            public PlayerPortrait Portrait = null;
            public Transform Self = null;
            public UILabel Rank = null;
            public Animation Animation = null;
        }
    }
}
