using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChessBattleMeData
    {
        [SerializeField]
        private HeroTeamData m_HeroTeam = new HeroTeamData();

        public HeroTeamData HeroTeam { get { return m_HeroTeam; } }

        [SerializeField]
        private ChessBattleHeroesStatus m_HeroesStatus = new ChessBattleHeroesStatus();

        public ChessBattleHeroesStatus HeroesStatus { get { return m_HeroesStatus; } }

        [SerializeField]
        private int m_Anger = 0;

        public int Anger { get { return m_Anger; } }

        public bool AnyHeroIsAlive
        {
            get
            {
                var lobbyHeroesData = GameEntry.Data.LobbyHeros.Data;

                for (int i = 0; i < lobbyHeroesData.Count; ++i)
                {
                    if (HeroIsAlive(lobbyHeroesData[i].Type))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool AnyHeroInTeamIsAlive
        {
            get
            {
                for (int i = 0; i < m_HeroTeam.HeroType.Count; ++i)
                {
                    if (HeroIsAlive(m_HeroTeam.HeroType[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool AnyHeroInTeamIsDead
        {
            get
            {
                for (int i = 0; i < m_HeroTeam.HeroType.Count; ++i)
                {
                    if (!HeroIsAlive(m_HeroTeam.HeroType[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool CaptainIsAlive
        {
            get
            {
                return HeroIsAlive(m_HeroTeam.HeroType[0]);
            }
        }

        public bool HeroIsAlive(int heroTypeId)
        {
            // For performance, this method doesn't request the player have this hero.
            var heroStatus = HeroesStatus.GetData(heroTypeId);
            return heroStatus == null || heroStatus.CurHP > 0;
        }

        public void UpdateData(LCGetChessBoard packet)
        {
            m_Anger = packet.Anger;

            var heroTeamInfo = new PBHeroTeamInfo();
            heroTeamInfo.HeroType.AddRange(packet.HeroTeam);
            UpdateHeroTeam(heroTeamInfo);
            HeroesStatus.ClearAndAddData(packet.HeroStatus);
        }

        public void UpdateData(PBHeroTeamInfo heroTeamInfo)
        {
            UpdateHeroTeam(heroTeamInfo);
        }

        public void UpdateData(IList<PBLobbyHeroStatus> heroesStatus)
        {
            for (int i = 0; i < heroesStatus.Count; ++i)
            {
                var heroTypeId = heroesStatus[i].Type;
                var current = HeroesStatus.GetData(heroTypeId);
                if (current == null)
                {
                    HeroesStatus.AddData(heroesStatus[i]);
                }
                else
                {
                    current.UpdateData(heroesStatus[i]);
                }
            }
        }

        private void UpdateHeroTeam(PBHeroTeamInfo heroTeamInfo)
        {
            if (heroTeamInfo.HeroType.Count <= 0)
            {
                heroTeamInfo.HeroType.AddRange(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType);
            }

            HeroTeam.UpdateData(heroTeamInfo);
        }
    }
}
