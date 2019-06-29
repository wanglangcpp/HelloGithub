using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 锻造装备活动邀请数据类。
    /// </summary>
    [Serializable]
    public class GearFoundryInvitationData
    {
        [SerializeField]
        private PlayerData m_Inviter = new PlayerData();

        public int Key { get { return Id; } }

        public int Id { get { return m_Inviter.Id; } }

        public PlayerData Inviter { get { return m_Inviter; } }

        public int m_TeamId = -1;
        public int TeamId { get { return m_TeamId; } }

        [SerializeField]
        public float m_ExpirationTime;

        public float ExpirationTime { get { return m_ExpirationTime; } }

        public void UpdateData(PBPlayerInfo pbInviter, int teamId)
        {
            m_Inviter.UpdateData(pbInviter);
            UpdateTeamId(teamId);
        }

        public void UpdateTeamId(int teamId)
        {
            m_TeamId = teamId;
        }

        public void UpdateExpirationTime()
        {
            float secondsToKeep = 300f; // GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.GearFoundryInvitationDuration, 300f);
            m_ExpirationTime = Time.time + secondsToKeep;
        }
    }
}
