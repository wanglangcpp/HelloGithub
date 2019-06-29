using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 附近玩家数据。
    /// </summary>
    [Serializable]
    public class NearbyPlayerData
    {
        [SerializeField]
        private PlayerData m_Player = new PlayerData();

        [SerializeField]
        private int m_MainHeroType = 0;

        [SerializeField]
        private NearbyPlayerMovement m_RandomMove = null;

        public PlayerData Player { get { return m_Player; } }
        
        public int MainHeroType { get { return m_MainHeroType; } }       

        public NearbyPlayerMovement RandomMovement
        {
            set
            {
                m_RandomMove = value;
            }
            get
            {
                return m_RandomMove;
            }
        }

        public bool IsMyFriend { get { return GameEntry.Data.Friends.CheckWhetherIsMyFriend(m_Player.Id); } }

        public int Key { get { return Player.Key; } }

        public void UpdateData(PBNearbyPlayerInfo data)
        {
            m_Player.UpdateData(data.PlayerInfo);
            m_MainHeroType = data.MainHeroType;
        }
    }
}
