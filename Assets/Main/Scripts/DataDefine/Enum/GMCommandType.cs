namespace Genesis.GameClient
{
    public enum GMCommandType
    {
        Undefined = 0,
        AddHero,
        PlayerLevelUp,
        PlayerVipLevelUp,
        HeroLevelUp,
        AddEnergy,
        AddMoney,
        AddCoin,
        AddMeridianToken,
        AddSpiritToken,
        AddArenaToken,
        AddDragonStripeToken,
        AddPvpToken,
        /// <summary>
        /// 活跃度
        /// </summary>
        AddActivenessToken,
        AddItem,
        SendSystemWorldChat,
        SendMail,
        SetAllInstanceStars,
        AddTask,
    }
}
