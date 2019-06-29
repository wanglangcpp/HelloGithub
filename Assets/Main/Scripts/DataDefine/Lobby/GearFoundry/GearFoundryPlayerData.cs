using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 锻造装备活动的玩家数据类。
    /// </summary>
    [Serializable]
    public class GearFoundryPlayerData : IGenericData<GearFoundryPlayerData, PBGearFoundryPlayerInfo>
    {
        [SerializeField]
        private PlayerData m_Player = new PlayerData();

        [SerializeField]
        private int m_FoundryCount = 0;

        public int Key { get { return Id; } }

        public int Id { get { return m_Player.Id; } }

        public string Name { get { return m_Player.Name; } }

        /// <summary>
        /// 该玩家当日在队伍内锻造装备的次数。
        /// </summary>
        public int FoundryCount { get { return m_FoundryCount; } }

        public PlayerData Player { get { return m_Player; } }

        public void UpdateData(PBGearFoundryPlayerInfo pb)
        {
            UpdateData(pb.Player);
            UpdateData(pb.FoundryCount);
        }

        public void UpdateData(PBPlayerInfo pbRawPlayer)
        {
            m_Player.UpdateData(pbRawPlayer);
        }

        public void UpdateData(int foundryCount)
        {
            m_FoundryCount = foundryCount;
        }
    }
}
