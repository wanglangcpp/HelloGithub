using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    public abstract class BaseSkillBadgeData
    {
        private int m_BadgeId;

        /// <summary>
        /// 徽章编号。
        /// </summary>
        public int BadgeId
        {
            get { return m_BadgeId; }
            protected set { m_BadgeId = value; }
        }

        public BaseSkillBadgeData()
        {
            m_BadgeId = -1;
        }
    }
}