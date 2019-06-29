using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 锻造装备活动进度的数据类。
    /// </summary>
    [Serializable]
    public class GearFoundryProgressData
    {
        [SerializeField]
        private int m_CurrentLevel = 0;

        /// <summary>
        /// 当前等级。
        /// </summary>
        public int CurrentLevel
        {
            get
            {
                return m_CurrentLevel;
            }
        }

        [SerializeField]
        private int m_CurrentProgress = 0;

        /// <summary>
        /// 当前等级下的进度。
        /// </summary>
        public int CurrentProgress
        {
            get
            {
                return m_CurrentProgress;
            }
        }

        public void UpdateData(PBGearFoundryProgressInfo pb)
        {
            m_CurrentLevel = pb.CurrentLevel;
            m_CurrentProgress = pb.CurrentProgress;
        }
    }
}
