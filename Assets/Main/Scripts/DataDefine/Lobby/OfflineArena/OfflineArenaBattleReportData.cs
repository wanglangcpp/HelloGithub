using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技战报数据类。
    /// </summary>
    [Serializable]
    public class OfflineArenaBattleReportData : IGenericData<OfflineArenaBattleReportData, PBArenaReportInfo>
    {
        public int Key
        {
            // 不需要使用关键字。
            get { return 0; }
        }

        [SerializeField]
        private PlayerData m_Opponent = new PlayerData();

        public PlayerData Opponent
        {
            get
            {
                return m_Opponent;
            }
        }

        [SerializeField]
        private ReportResultType m_Result;

        public ReportResultType Result { get { return m_Result; } }

        [SerializeField]
        private int m_DeltaRank = 0;

        public int DeltaRank { get { return m_DeltaRank; } }

        [SerializeField]
        private int m_MyMight = 0;

        public int MyMight { get { return m_MyMight; } }

        private DateTime m_BattleEndTime;
        public DateTime BattleEndTime { get { return m_BattleEndTime; } }

        public void UpdateData(PBArenaReportInfo pb)
        {
            m_Result = (ReportResultType)pb.Result;
            m_DeltaRank = pb.DeltaRank;
            m_BattleEndTime = new DateTime(pb.Time, DateTimeKind.Utc);
            m_MyMight = pb.MyMight;
            m_Opponent.UpdateData(pb.ArenaPlayerInfo);
        }
    }
}
