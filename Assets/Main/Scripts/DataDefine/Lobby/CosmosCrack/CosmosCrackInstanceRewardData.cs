using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝副本奖励数据。
    /// </summary>

    [Serializable]
    public class CosmosCrackInstanceRewardData
    {
        [SerializeField]
        private int m_GeneralItemId = 0;

        /// <summary>
        /// 物品编号。
        /// </summary>
        public int GeneralItemId { get { return m_GeneralItemId; } }

        [SerializeField]
        private int m_GeneralItemCount = 0;

        /// <summary>
        /// 物品数量。
        /// </summary>
        public int GeneralItemCount { get { return m_GeneralItemCount; } }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="pb">通信协议数据。</param>
        public void UpdateData(PBItemInfo pb)
        {
            m_GeneralItemId = pb.Type;
            m_GeneralItemCount = pb.Count;
        }
    }
}
