namespace Genesis.GameClient
{
    /// <summary>
    /// VIP 特权类型。
    /// </summary>
    public enum VipPrivilegeType
    {
        /// <summary>
        /// 兑换体力。
        /// </summary>
        ExchangeEnergy = 0,

        /// <summary>
        /// 兑换金币。
        /// </summary>
        ExchangeCoin,

        /// <summary>
        /// 购买额外离线竞技次数。
        /// </summary>
        BuyAdditionalArena,

        /// <summary>
        /// VIP 特权类型数量，若要增加 VIP 特权类型，需加在此枚举之前！
        /// </summary>
        VipPrivilegeTypeCount
    }
}
