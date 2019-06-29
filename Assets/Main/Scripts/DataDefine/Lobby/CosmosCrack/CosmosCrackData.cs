using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝活动数据。
    /// </summary>
    [Serializable]
    public class CosmosCrackData
    {
        [SerializeField]
        private int m_UsedRoundCount = 0;

        /// <summary>
        /// 已经使用的轮次。
        /// </summary>
        public int UsedRoundCount { get { return m_UsedRoundCount; } }

        [SerializeField]
        private List<CosmosCrackInstanceData> m_InstanceDatas = new List<CosmosCrackInstanceData>();

        /// <summary>
        /// 获取副本数据。
        /// </summary>
        /// <returns>副本数据。</returns>
        public IList<CosmosCrackInstanceData> GetInstanceDatas()
        {
            return m_InstanceDatas.ToArray();
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="pb">通信协议数据。</param>
        public void UpdateData(LCGetCosmosCrackInfo pb)
        {
            m_UsedRoundCount = pb.UsedRoundCount;

            for (int i = 0; i < pb.InstanceInfos.Count; ++i)
            {
                var pbInstance = pb.InstanceInfos[i];

                if (i < m_InstanceDatas.Count)
                {
                    m_InstanceDatas[i].UpdateData(pbInstance);
                }
                else
                {
                    var instanceData = new CosmosCrackInstanceData();
                    instanceData.UpdateData(pbInstance);
                    m_InstanceDatas.Add(instanceData);
                }
            }
        }
    }
}
