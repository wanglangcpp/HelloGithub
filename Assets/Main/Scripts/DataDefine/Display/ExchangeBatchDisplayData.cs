namespace Genesis.GameClient
{
    /// <summary>
    /// 批量兑换显示数据。
    /// </summary>
    public class ExchangeBatchDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 英雄碎片类型编号。
        /// </summary>
        /// <remarks>即它作为道具的编号。</remarks>
        public int HeroPieceTypeId { get; set; }

        /// <summary>
        /// 拥有数量。
        /// </summary>
        public int OwnedCount { get; set; }
    }
}
