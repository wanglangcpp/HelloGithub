using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用于显示排名的类。其中前三名特殊显示，其余用文本框显示。
    /// </summary>
    [Serializable]
    public class RankDisplay
    {
        [SerializeField]
        private UIWidget m_First = null;

        [SerializeField]
        private UIWidget m_Second = null;

        [SerializeField]
        private UIWidget m_Third = null;

        [SerializeField]
        private UILabel m_RankLabel = null;

        private UIWidget[] m_RankWidgets = null;

        public void Init()
        {
            m_RankWidgets = new UIWidget[4];
            m_RankWidgets[0] = m_RankLabel;
            m_RankWidgets[1] = m_First;
            m_RankWidgets[2] = m_Second;
            m_RankWidgets[3] = m_Third;

            for (int i = 0; i < m_RankWidgets.Length; ++i)
            {
                m_RankWidgets[i].gameObject.SetActive(false);
            }
        }

        public void SetRank(int rank)
        {
            for (int i = 1; i <= 3; ++i)
            {
                m_RankWidgets[i].gameObject.SetActive(i == rank);
            }

            m_RankLabel.text = rank.ToString();
            m_RankLabel.gameObject.SetActive(rank > 3);
        }
    }
}
