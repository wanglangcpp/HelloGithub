using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 经验资源副本数据。
    /// </summary>
    [System.Serializable]
    public class InstanceForExpResourceData
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

        public void UpdateData(PBInstanceForExpResourceInfo pb)
        {
            m_PlayedCount = pb.PlayedCount;
        }
    }
}
