using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 金币资源副本数据。
    /// </summary>
    [System.Serializable]
    public class InstanceForCoinResourceData
    {
        [SerializeField]
        private int m_PlayedCount = 0;

        public int PlayedCount
        {
            get
            {
                return m_PlayedCount;
            }
        }

        public void UpdateData(PBInstanceForCoinResourceInfo pb)
        {
            m_PlayedCount = pb.PlayedCount;
        }
    }
}
