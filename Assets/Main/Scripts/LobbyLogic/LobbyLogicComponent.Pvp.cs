using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        [SerializeField]
        private Pvp m_Pvp = new Pvp();

        public PvpType CurrentPvpType
        {
            get
            {
                return m_Pvp.CurrentType;
            }
        }

        public void StartPvpMatching(PvpType pvpType)
        {
            m_Pvp.StartMatching(pvpType);
        }

        public void StopPvpMatching()
        {
            m_Pvp.StopMatching();
        }

        public void GetPvpInfo()
        {
            m_Pvp.GetInfo();
        }

        public void GetSinglePvpRank()
        {
            m_Pvp.GetSinglePvpRank();
        }

        public void GetWorldSinglePvpRank()
        {
            m_Pvp.GetWorldSinglePvpRank();
        }

        [Serializable]
        private class Pvp
        {
            [SerializeField]
            private PvpType m_CurrentPvpType = PvpType.Undefined;

            public PvpType CurrentType
            {
                get
                {
                    return m_CurrentPvpType;
                }
            }

            public void StartMatching(PvpType pvpType)
            {
                if (GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    return;
                }

                m_CurrentPvpType = pvpType;
                switch (pvpType)
                {
                    case PvpType.Single:
                        //GameEntry.Network.Send(new CLStartSinglePvpMatching());
                        CLRequestSingleMatch request = new CLRequestSingleMatch();
                        request.Type = 1;
                        GameEntry.Network.Send(request);
                        break;
                    default:
                        Log.Error("Can not match this PVP type '{0}'.", pvpType.ToString());
                        break;
                }
            }

            public void StopMatching()
            {
                if (GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    return;
                }

                if (m_CurrentPvpType == PvpType.Undefined)
                {
                    return;
                }

                m_CurrentPvpType = PvpType.Undefined;
                //CLStopSinglePvpMatching request = new CLStopSinglePvpMatching();
                CLRequestSingleMatch request = new CLRequestSingleMatch();
                request.Type = 2;
                GameEntry.Network.Send(request);
            }

            public void GetInfo()
            {
                //if (!GameEntry.OfflineMode.OfflineModeEnabled)
                //{
                //    CLGetSinglePvpInfo msg = new CLGetSinglePvpInfo();
                //    GameEntry.Network.Send(msg);
                //    return;
                //}

                //var pvpArenaData = GameEntry.Data.PvpArena;
                //var response = new LCGetSinglePvpInfo
                //{
                //    ChallengeCount = 5,
                //    Rank = pvpArenaData.Rank == 0 ? 1 : pvpArenaData.Rank == 1 ? 3 : pvpArenaData.Rank == 3 ? 4 : pvpArenaData.Rank == 4 ? 100 : 1,
                //    Score = pvpArenaData.Score == 100 ? 1000 : 100,
                //};

                //LCGetSinglePvpInfoHandler.Handle(this, response);
            }

            public void GetSinglePvpRank()
            {
                if (GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    //var response = new LCGetSinglePvpRank();

                    //int itemsPerPage = 20;
                    //for (int i = 0; i < itemsPerPage; ++i)
                    //{
                    //    response.PlayerInfo.Add(new PBPlayerInfo
                    //    {
                    //        Id = itemsPerPage + i + 1 + 10000,
                    //        Name = "Fake Player",
                    //        Level = 100,
                    //    });

                    //    response.Score.Add(10000 - i * 10);
                    //}
                    //response.MyScore = 100;
                    //response.MyRank = 10;
                    //GameEntry.Event.Fire(this, new GetSinglePvpRankEventArgs(response));
                    return;
                }

                CLGetSinglePvpRank msg = new CLGetSinglePvpRank();
                GameEntry.Network.Send(msg);
            }

            public void GetWorldSinglePvpRank()
            {
                //if (GameEntry.OfflineMode.OfflineModeEnabled)
                //{
                //    var response = new LCGetWorldSinglePvpRank();

                //    int itemsPerPage = 20;
                //    for (int i = 0; i < itemsPerPage; ++i)
                //    {
                //        response.PlayerInfo.Add(new PBPlayerInfo
                //        {
                //            Id = itemsPerPage + i + 1 + 10000,
                //            Name = "Fake Player",
                //            Level = 100,
                //        });

                //        response.Score.Add(10000 - i * 10);
                //        response.ServerId.Add(1);
                //    }
                //    response.MyScore = 100;
                //    response.MyRank = 10;
                //    GameEntry.Event.Fire(this, new GetWorldPvpRankEventArgs(response));
                //    return;
                //}

                //CLGetWorldSinglePvpRank msg = new CLGetWorldSinglePvpRank();
                //GameEntry.Network.Send(msg);
            }
        }
    }
}
