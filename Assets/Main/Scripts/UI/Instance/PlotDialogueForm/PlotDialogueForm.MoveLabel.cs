using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlotDialogueForm
    {
        [Serializable]
        private class MoveLabel
        {
            [SerializeField]
            private UILabel m_Content = null;

            [SerializeField]
            private int m_MaxLineCount = 2;

            private float m_OriginalY = 0.0f;
            private int m_EveryMoveDistance = 0;
            private int m_CurMoveTimes = 1;
            private int m_MaxMoveTimes = 1;

            public string ContentValue
            {
                get
                {
                    return m_Content.text;
                }
                set
                {
                    m_Content.text = value;
                }
            }

            public int MaxMoveTimes
            {
                get
                {
                    if (m_MaxMoveTimes == 0 || m_MaxMoveTimes == -1)
                    {
                        m_MaxMoveTimes = m_Content.height / EveryMoveDistance;
                    }

                    return m_MaxMoveTimes;
                }
            }

            public int EveryMoveDistance
            {
                get
                {
                    if (m_EveryMoveDistance == 0)
                    {
                        m_OriginalY = m_Content.transform.localPosition.y;
                        m_EveryMoveDistance = (m_Content.fontSize + m_Content.spacingY) * (m_MaxLineCount > 0 ? m_MaxLineCount : 1);
                    }

                    return m_EveryMoveDistance;
                }
            }

            public bool HasShowEndLine()
            {
                if (MaxMoveTimes > m_CurMoveTimes)
                {
                    return false;
                }
                return true;
            }

            public void ShowNextLine()
            {
                if (m_MaxMoveTimes == 0)
                {
                    m_MaxMoveTimes = m_Content.height / EveryMoveDistance - 1;
                    if (m_MaxMoveTimes < 0)
                    {
                        m_MaxMoveTimes = 1;
                    }
                    m_CurMoveTimes = 1;
                }

                m_CurMoveTimes++;
                m_Content.transform.localPosition = new Vector3(m_Content.transform.localPosition.x, m_OriginalY + m_CurMoveTimes * EveryMoveDistance, m_Content.transform.localPosition.z);
            }

            public void ResumeLabel()
            {
                m_CurMoveTimes = 1;
                m_MaxMoveTimes = 1;
                if (EveryMoveDistance != 0.0f)
                {
                    m_Content.transform.localPosition = new Vector3(m_Content.transform.localPosition.x, m_OriginalY, m_Content.transform.localPosition.z);
                }
            }
        }
    }
}
