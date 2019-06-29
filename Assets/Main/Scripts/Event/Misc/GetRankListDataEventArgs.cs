using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class GetRankListDataEventArgs : GameEventArgs
    {
        public GetRankListDataEventArgs(LCRefreshRankList refreshRankList)
        {
            RefreshRankList = refreshRankList;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetRankListData;
            }
        }

        public LCRefreshRankList RefreshRankList { get; set; }
    }
}
