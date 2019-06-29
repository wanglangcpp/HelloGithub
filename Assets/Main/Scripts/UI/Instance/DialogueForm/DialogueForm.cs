using UnityEngine;
using System.Collections;
using GameFramework;
using System;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    /// <summary>
    /// 对话
    /// </summary>
    public partial class DialogueForm : NGUIForm
    {
        [Serializable]
        private struct Item
        {
            public UISprite ItemIcon;
            public UILabel ItemCount;
        }
        [SerializeField]
        private Talker m_LeftTalker = null;
        [SerializeField]
        private Talker m_RightTalker = null;
        [SerializeField]
        private UILabel m_DialogueContent = null;
        [SerializeField]
        private GameObject m_RewareObj = null;
        [SerializeField]
        private UIGrid m_RewardsGrid = null;
        [SerializeField]
        private Item[] m_Rewards = null;
        [SerializeField]
        private TypewriterEffect m_WriterEffect = null;

        [SerializeField]
        private float m_NextPlotTime = 5.0f;
        [SerializeField]
        private float m_NextWordTime = 0.1f;

        private float m_CurTime = 0.0f;
        private bool m_TalkStart = false;
        private bool m_IsLineFeed = false;
        private IDataTable<DRTaskTalk> TalkTable = null;
        private DRTaskTalk m_TalkData = null;
        private DRTask m_Task = null;
        string talkContent;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var myUserData = userData as DialogueFormDisplayData;

            if (myUserData == null)
            {
                Log.Error("PlotDialogDisplayData is invalid.");
                return;
            }
            TalkTable = GameEntry.DataTable.GetDataTable<DRTaskTalk>();
            m_Task = myUserData.Task;
            RefreshRewards();
            StartPlot(myUserData.DialogId);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (talkContent.Length > m_DialogueContent.text.Length)
            {
                m_TalkStart = false;
                m_IsLineFeed = true;
            }
            else
            {
                m_TalkStart = true;
                m_IsLineFeed = false;
            }
            if (m_TalkStart)
            {
                m_CurTime += realElapseSeconds;
                if (m_CurTime >= m_NextPlotTime)
                {
                    StartPlot(m_TalkData.NextTalkId);
                }
            }
            else if (m_IsLineFeed)
            {
                realElapseSeconds = 0.0f;
            }

#if UNITY_EDITOR
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                OnClickSkipBt();
            }
#endif
        }

        private void StartPlot(int plotId)
        {
            if (plotId == 0)
            {
                m_TalkStart = false;
                m_RewareObj.SetActive(true);
                return;
            }
            m_IsLineFeed = false;
            m_TalkStart = true;
            m_TalkData = TalkTable.GetDataRow(plotId);
            if (m_TalkData == null)
            {
                Log.Error("Cannot find Plot Dialogue '{0}'.", plotId);
                return;
            }

            m_CurTime = 0.0f;
            HideAllTalker();
            var curTalker = m_TalkData.IsMeTalk ? m_RightTalker : m_LeftTalker;
            curTalker.SetVisible(true);
            curTalker.SetNameValue(m_TalkData.NpcName, m_TalkData.IsMeTalk);
            curTalker.SetNameIcon(m_TalkData.HeadIcon, m_TalkData.IsMeTalk);
            talkContent = GameEntry.Localization.GetString(m_TalkData.DialogueContent);
            m_DialogueContent.text = talkContent;
            m_WriterEffect.enabled = true;
            m_WriterEffect.ResetToBeginning();
        }

        private void QuickTalk()
        {
            m_WriterEffect.enabled = false;
            m_DialogueContent.text = GameEntry.Localization.GetString(m_TalkData.DialogueContent);
        }

        private void RefreshRewards()
        {
            for (int i = 0; i < m_Rewards.Length; i++)
            {
                if (i < m_Task.Rewards.Count)
                {
                    m_Rewards[i].ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(m_Task.Rewards[i].IconId));
                    if (m_Task.Rewards[i].Count > 1)
                    {
                        m_Rewards[i].ItemCount.text = m_Task.Rewards[i].Count.ToString();
                    }
                    else
                    {
                        m_Rewards[i].ItemCount.text = string.Empty;
                    }
                }
                else
                {
                    m_Rewards[i].ItemIcon.gameObject.SetActive(false);
                }
            }
            m_RewareObj.SetActive(false);
        }

        private void HideAllTalker()
        {
            m_LeftTalker.SetVisible(false);
            m_RightTalker.SetVisible(false);
        }

        public void OnClickSkipBt()
        {
            if (m_IsLineFeed)
            {
                QuickTalk();
                return;
            }
            StartPlot(m_TalkData.NextTalkId);
        }

        public void OnClickClaimBtn()
        {
            int npcId = TalkTable.GetDataRow(m_Task.Conditions[1]).NpcId;
            GameEntry.TaskComponent.FinishTask(m_Task.Id, m_Task.Conditions[1]);
            CloseSelf();
        }
    }
}

