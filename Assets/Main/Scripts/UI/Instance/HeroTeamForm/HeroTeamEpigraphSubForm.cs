using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroTeamEpigraphSubForm : NGUISubForm
    {
        [SerializeField]
        private HeroTeamEpigraphItem[] m_EpigraphSlots = null;

        [SerializeField]
        private HeroTeamEpigraphAttributeItem[] m_AttributeItems = null;

        [SerializeField]
        private Animation[] m_Animations = null;

        [SerializeField]
        private GameObject m_AttributePanel = null;

        [SerializeField]
        private EpigraphItemInfoPanel m_EpigraphInfoPanel = null;

        [SerializeField]
        private Animation m_SwitchAnimation = null;

        private int m_CurSlotIndex = -1;

        private Action m_UnfoldReturn = null;

        public float GetSwitchAnimTime()
        {
            return m_SwitchAnimation.clip.length;
        }

        public void RefreshEpigraphData(Action unfoldReturn, int newEpigraphId = -1)
        {
            m_UnfoldReturn = unfoldReturn;
            m_AttributePanel.SetActive(true);
            m_EpigraphInfoPanel.gameObject.SetActive(false);

            m_CurSlotIndex = -1;
            for (int i = 0; i < m_Animations.Length; i++)
            {
                m_Animations[i].clip.wrapMode = WrapMode.PingPong;
                m_Animations[i].Play();
            }

            int count = GameEntry.Data.EpigraphSlots.Data.Count;
            m_CurSlotIndex = count - 1;
            int nextSlotlevel = 1; // GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.EpigraphRequiredLevel, count.ToString()));

            for (int i = 0; i < m_EpigraphSlots.Length; i++)
            {
                if (i <= m_CurSlotIndex)
                {
                    m_EpigraphSlots[i].SetUndress(GameEntry.Data.EpigraphSlots.Data[i].Id != 0, GameEntry.Data.EpigraphSlots.Data[i], i, OnClickEpigraphSlot);
                    m_EpigraphSlots[i].SetSelect(false);
                    m_EpigraphSlots[i].Id = GameEntry.Data.EpigraphSlots.Data[i].Id;
                    if (GameEntry.Data.EpigraphSlots.Data[i].Id != 0)
                    {
                        m_AttributeItems[i].gameObject.SetActive(true);
                        m_AttributeItems[i].Init(GameEntry.Data.EpigraphSlots.Data[i], GameEntry.Data.EpigraphSlots.Data[i].Id == newEpigraphId);
                    }
                    else
                    {
                        m_AttributeItems[i].gameObject.SetActive(false);
                    }
                }
                else if (i == m_CurSlotIndex + 1 && (nextSlotlevel <= GameEntry.Data.Player.Level))
                {
                    m_EpigraphSlots[i].SetCanUnLock();
                    m_AttributeItems[i].gameObject.SetActive(false);
                }
                else
                {
                    int unLocklevel = 1; // GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.EpigraphRequiredLevel, i.ToString()));
                    m_EpigraphSlots[i].SetLock(unLocklevel);
                    m_AttributeItems[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnClickEpigraphSlot(int index)
        {
            for (int i = 0; i <= m_CurSlotIndex; i++)
            {
                m_EpigraphSlots[i].SetSelect(index == i);
            }
            m_AttributePanel.SetActive(false);
            m_EpigraphInfoPanel.gameObject.SetActive(true);
            m_EpigraphInfoPanel.Init(index, m_EpigraphSlots[index].Id);
        }

        public void OnClickCloseBtn()
        {
            if (m_UnfoldReturn != null)
            {
                m_UnfoldReturn();
            }
        }

        public void OnClickBackToAttribute()
        {
            if (m_AttributePanel.activeSelf)
            {
                return;
            }
            for (int i = 0; i <= m_CurSlotIndex; i++)
            {
                m_EpigraphSlots[i].SetSelect(false);
            }
            m_AttributePanel.SetActive(true);
            m_EpigraphInfoPanel.gameObject.SetActive(false);
        }
    }
}
