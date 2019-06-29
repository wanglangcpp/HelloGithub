using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 打开棋子。
    /// </summary>
    public class ChessBoardOpenChessFieldSuccessEventArgs : GameEventArgs
    {

        public ChessBoardOpenChessFieldSuccessEventArgs(LCOpenChessField packet = null)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardOpenChessFieldSuccess;
            }
        }

        public IList<int> ChangedChessFieldIndices { get; private set; }

        /// <summary>
        /// 网络回包。
        /// </summary>
        public LCOpenChessField Packet { get; private set; }

        /// <summary>
        /// 获取或设置获得物品数据。
        /// </summary>
        public ReceivedGeneralItemsViewData ReceiveGoodsData { get; set; }
    }
}
