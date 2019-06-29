using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChessBattleHeroStatus : IGenericData<ChessBattleHeroStatus, PBLobbyHeroStatus>
    {
        [SerializeField]
        private int m_Type;

        [SerializeField]
        private int m_CurHP;

        public int Key { get { return m_Type; } }
        public int Type { get { return m_CurHP; } }
        public int CurHP { get { return m_CurHP; } }

        public void UpdateData(PBLobbyHeroStatus heroStatus)
        {
            m_Type = heroStatus.Type;
            m_CurHP = heroStatus.CurHP;
        }
    }
}
