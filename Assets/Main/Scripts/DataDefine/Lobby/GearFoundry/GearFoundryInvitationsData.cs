using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 锻造装备活动邀请列表类。
    /// </summary>
    [Serializable]
    public class GearFoundryInvitationsData
    {
        private List<GearFoundryInvitationData> m_Data = new List<GearFoundryInvitationData>();

        public List<GearFoundryInvitationData> Data { get { return m_Data; } }

        public void OnUpdate()
        {
            float currentTime = Time.time;
            for (int i = 0; i < m_Data.Count; ++i)
            {
                if (currentTime > m_Data[i].ExpirationTime)
                {
                    m_Data.RemoveAt(i);
                    --i;
                }
            }
        }

        public GearFoundryInvitationData GetData(int key)
        {
            for (int i = 0; i < m_Data.Count; ++i)
            {
                if (m_Data[i].Key == key)
                {
                    return m_Data[i];
                }
            }
            return null;
        }

//         public bool AddOrUpdateData(LCInvitedForGearFoundry invitationPacket)
//         {
//             var key = invitationPacket.InviterPlayer.Id;
//             var item = GetData(key);
//             if (item == null)
//             {
//                 item = new GearFoundryInvitationData();
//                 item.UpdateData(invitationPacket.InviterPlayer, invitationPacket.TeamId);
//                 item.UpdateExpirationTime();
//                 m_Data.Add(item);
//                 return true;
//             }
// 
//             item.UpdateTeamId(invitationPacket.TeamId);
//             item.UpdateExpirationTime();
//             return false;
//         }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public void RemoveData(int playerId, int teamId)
        {
            int index = -1;
            for (int i = 0; i < m_Data.Count; ++i)
            {
                if (m_Data[i].Inviter.Id == playerId && m_Data[i].TeamId == teamId)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                m_Data.RemoveAt(index);
            }
        }
    }
}
