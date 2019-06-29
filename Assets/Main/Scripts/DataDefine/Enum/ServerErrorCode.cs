namespace Genesis.GameClient
{
    /// <summary>
    /// 服务器错误代码。
    /// </summary>
    public enum ServerErrorCode
    {
        Ok = 0,
        ServerException = 10000, // 服务器异常。
        DuplicateLogin = 10004, // 重复登录

        // 以下内容需和服务器同步。

        InternalError = 1000, // 内部错误。
        OperationInvalid, // 操作非法。
        AuthorityInvalid, // 权限非法。
        AuthorityBlocked, // 权限封停。
        RoomStatusError, // 房间状态错误。
        GMCommandInvalid, // GM 指令非法。
        GMCommandFailure, // GM 指令执行失败。
        AccountNameInvalid, // 帐号非法。
        PlayerNameLengthInvalid, // 玩家名称长度非法。
        PlayerNameDuplicated, // 玩家名称重复。
        PlayerNameHasBadWord, // 玩家名称含有禁用词汇。
        EnergyNotEnough, // 能量不足。
        MoneyNotEnough, // 钻石不足。
        CoinNotEnough, // 金币不足。
        MeridianTokenNotEnough, // 星图代币不足。
        ItemNotEnough, // 道具不足。
        UseHeroExpItemInvalid, // 使用英雄加经验道具非法。
        StartInstanceInvalid, // 开始副本非法。
        LeaveInstanceInvalid, // 离开副本非法。
        CleanOutInstanceInvalid, // 扫荡副本非法。
        InstanceGroupChestInvalid, // 副本组宝箱非法。
        HeroInvalid, // 英雄非法。
        HeroComposeInvalid, // 英雄合成非法。
        HeroStarLevelUpInvalid, // 英雄升星非法。
        HeroUseQualityItemInvalid, // 英雄使用品质道具非法。
        HeroQualityLevelUpInvalid, // 英雄升品质非法。
        HeroGearLevelUpInvalid, // 英雄装备升级非法。
        HeroGearQualityLevelUpInvalid, // 英雄装备升品质非法。
        SynthesizeHeroQualityItemInvalid, // 合成英雄品质道具非法。
        HeroSkillLevelUpInvalid, // 英雄技能升级非法。
        HeroSkillBadgeSlotUnlockInvalid, // 激活英雄技能徽章槽非法。
        ChatInvalid, // 聊天非法。
        ChatMessageInvalid, // 聊天内容非法。
        WorldChatLevelLimit, // 世界聊天等级不足。
        WorldChatTooFrequently, // 世界聊天频率太高。
        FriendLevelLimit, // 好友功能等级不足。
        OtherPlayerFriendListFull, // 对方好友已满。
        FriendRequestInvalid, // 好友请求非法。
        FriendReplyInvalid, // 好友回复非法。
        FriendRemoveInvalid, // 好友移除非法。
        FriendGiveEnergyInvalid, // 好友赠送体力非法。
        FriendClaimEnergyInvalid, // 好友领取体力非法。
        SearchPlayerInvalid, // 查找玩家非法。
        ChangeHeroTeamInvalid, // 修改阵容非法。
        HeroSkillBadgeInsertInvalid, // 插入技能徽章非法。
        HeroSkillBadgeRemoveInvalid, // 去除技能徽章非法。
        HeroSkillBadgeLevelUpInvalid, // 技能徽章升级非法。
        ReadMailInvalid, // 阅读邮件非法。
        DeleteMailInvalid, // 删除邮件非法。
        MeridianInvalid, // 星图非法。
        ShopInvalid = 1050, // 商城非法。
        ChanceInvalid, // 抽奖非法。
        RankInvalid, // 排行榜非法。
        MatchingInvalid, // 匹配非法。
        ExchangeEnergyInvalid, // 购买体力非法。
        ExchangeCoinInvalid, // 购买金币非法。
        ArenaLevelLimit, // 离线竞技等级不足。
        ArenaInvalid, // 离线竞技非法。
        ChangePortraitInvalid, // 更换头像非法。
        ClaimAchievementInvalid, // 领取成就奖励非法。
        ClaimDailyQuestInvalid, // 领取每日活动奖励非法。
        ClaimDailyQuestActivenessInvalid, // 领取活跃值奖励非法。
        ClaimDailyLoginInvalid, // 领取签到奖励非法。
        EnterInstanceForResourceInvalid, // 进入资源副本非法。
        LeaveInstanceForResourceInvalid, // 离开资源副本非法。
        PlatformLoginError,//SDK（Platform）登录错误
        GetGiftInvalid, // 获取礼物非法。
        GetOpenFutionInvalid, // 获取功能开发非法。
        InstanceForTowerClaimInvalid,//爬塔副本领奖无效
        InstanceForGroupBossInvalid,//boss副本攻打次数已经用完
        //room从2000开始
        NotFindRoom = 2000,//没有找到room
        NotRegister = 2001,//没有注册
    }
}
