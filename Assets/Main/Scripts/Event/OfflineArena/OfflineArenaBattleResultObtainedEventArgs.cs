using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技中取得副本结算数据的事件。
    /// </summary>
    public class OfflineArenaBattleResultDataObtainedEventArgs : GameEventArgs
    {
        public OfflineArenaBattleResultDataObtainedEventArgs(int arenaCoinObtained, int arenaTokenObtained, int myOldRank, int myNewRank, List<ItemData> arenaItemsObtained)
        {
            ArenaCoinObtained = arenaCoinObtained;
            ArenaTokenObtained = arenaTokenObtained;
            MyOldRank = myOldRank;
            MyNewRank = myNewRank;
            ArenaItemsObtained = arenaItemsObtained;
        }

        public override int Id
        {
            get { return (int)EventId.OfflineArenaBattleResultDataObtained; }
        }

        public int ArenaCoinObtained { get; private set; }

        public int ArenaTokenObtained { get; private set; }

        public int MyOldRank { get; private set; }

        public int MyNewRank { get; private set; }

        public List<ItemData> ArenaItemsObtained { get; private set; }
    }
}
