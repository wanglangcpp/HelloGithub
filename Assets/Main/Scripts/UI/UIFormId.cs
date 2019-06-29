namespace Genesis.GameClient
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId
    {
        Undefined = 0,

        #region Common UIs

        /// <summary>
        /// 登录。
        /// </summary>
        Login = 2,

        /// <summary>
        /// 选服。
        /// </summary>
        SelectServer = 3,

        /// <summary>
        /// 创建玩家。
        /// </summary>
        CreatePlayer = 4,

        /// <summary>
        /// 对话框。
        /// </summary>
        Dialog = 5,

        /// <summary>
        /// 提示飘窗。
        /// </summary>
        Toast = 6,

        /// <summary>
        /// 断线重连提示框。
        /// </summary>
        ReconnectionForm = 7,

        #endregion Common UIs

        #region Lobby UIs

        /// <summary>
        /// 主界面。
        /// </summary>
        Main = 100,

        /// <summary>
        /// 选择副本。
        /// </summary>
        InstanceSelectForm = 101,

        /// <summary>
        /// 副本详情。
        /// </summary>
        InstanceInfoForm = 102,

        /// <summary>
        /// 选择出战英雄。
        /// </summary>
        HeroTeamForm = 103,

        /// <summary>
        /// 英雄图鉴。
        /// </summary>
        HeroAlbumForm = 105,

        /// <summary>
        /// 翻翻棋。
        /// </summary>
        ActivityChessmanForm = 110,

        /// <summary>
        /// 翻翻棋战斗团队。
        /// </summary>
        PvpActivityChessmanForm = 111,

        /// <summary>
        /// 英雄战意。
        /// </summary>
        HeroConsiousnessForm = 112,

        /// <summary>
        /// 英雄飞升。
        /// </summary>
        HeroElevationForm = 113,

        /// <summary>
        /// 英雄战魂。
        /// </summary>
        HeroSoulForm = 114,

        /// <summary>
        /// 星图。
        /// </summary>
        MeridianForm = 115,

        /// <summary>
        /// 星图打开。
        /// </summary>
        MeridianHoroscopeForm = 116,

        /// <summary>
        /// 铭文。
        /// </summary>
        EpigraphForm = 117,

        /// <summary>
        /// 聊天。
        /// </summary>
        ChatForm = 118,

        /// <summary>
        /// 社交。
        /// </summary>
        SocialForm = 119,

        /// <summary>
        /// 好友界面。
        /// </summary>
        FriendListForm = 120,

        /// <summary>
        /// 邮件。
        /// </summary>
        EmailForm = 121,

        /// <summary>
        /// 玩家信息。
        /// </summary>
        PlayerInfoForm = 122,

        /// <summary>
        /// 铭文升级。
        /// </summary>
        EpigraphStrengthenForm = 123,

        /// <summary>
        /// 抽奖选择。
        /// </summary>
        ChanceSelectForm = 124,

        /// <summary>
        /// 抽奖详情。
        /// </summary>
        ChanceDetailForm = 125,

        /// <summary>
        /// 战魂升级。
        /// </summary>
        SoulStrengthenForm = 126,

        /// <summary>
        /// 装备锻造活动。
        /// </summary>
        ActivityFoundryForm = 127,

        /// <summary>
        /// 请求处理。
        /// </summary>
        RequestListForm = 128,

        /// <summary>
        /// 剧情对话。
        /// </summary>
        PlotDialogueForm = 129,

        /// <summary>
        /// 进入故事副本。
        /// </summary>
        InstanceStoryForm = 130,

        /// <summary>
        /// 获得奖励。
        /// </summary>
        ReceiveItemForm = 131,

        /// <summary>
        /// 副本扫荡结算。
        /// </summary>
        CleanOutResultForm = 132,

        /// 选择扫荡。
        /// </summary>
        CleanOutSelectForm = 133,

        /// <summary>
        /// 离线竞技入口。
        /// </summary>
        ActivityOfflineArenaForm = 134,

        /// <summary>
        /// 离线竞技副本准备。
        /// </summary>
        PvpOfflineArenaForm = 135,

        /// <summary>
        /// 排行榜。
        /// </summary>
        RankListForm = 136,

        /// <summary>
        /// 出售。
        /// </summary>
        SaleForm = 137,

        /// <summary>
        /// 道具。
        /// </summary>
        InventoryForm = 138,

        /// <summary>
        /// 商店。
        /// </summary>
        ShopForm = 139,

        /// <summary>
        /// 批量兑换。
        /// </summary>
        ExchangeBatchForm = 140,

        /// <summary>
        /// 英雄经验道具使用。
        /// </summary>
        HeroAlbumUseItemForm = 141,

        /// <summary>
        /// 玩家升级界面
        /// </summary>
        PlayerLevelUpForm = 142,

        /// <summary>
        /// 获得装备界面
        /// </summary>
        ReceiveGearForm = 143,

        /// <summary>
        /// 战斗力变化界面
        /// </summary>
        MightChangeForm = 144,

        /// <summary>
        /// 活动入口界面
        /// </summary>
        ActivitySelectForm = 145,

        /// <summary>
        /// 更改玩家头像界面
        /// </summary>
        ReplacePlayerPortraitForm = 146,

        /// <summary>
        /// 英雄获取界面
        /// </summary>
        ReceiveHeroForm = 147,

        /// <summary>
        /// 时空裂缝活动界面。
        /// </summary>
        ActivityCosmosForm = 148,

        /// <summary>
        /// 技能升级界面。
        /// </summary>
        SkillStrengthenForm = 149,

        /// <summary>
        /// PVP选择界面。
        /// </summary>
        PvpSelectForm = 150,

        /// <summary>
        /// PVP主界面。
        /// </summary>
        ActivitySinglePvpMainForm = 151,

        /// <summary>
        /// PVP单人竞技自动匹配界面功能。
        /// </summary>
        ActivitySinglePvpMatchForm = 152,

        /// <summary>
        /// PVP单人竞技结算界面。
        /// </summary>
        ActivitySinglePvpResultForm = 153,

        /// <summary>
        /// PVP单人竞技结算失败界面。
        /// </summary>
        ActivitySinglePvpFailureForm = 154,

        /// <summary>
        /// 玩家交互摘要。
        /// </summary>
        PlayerSummaryForm = 155,

        /// <summary>
        /// 成就副本界面。
        /// </summary>
        AchievementsForm = 156,

        /// <summary>
        /// 物品详情界面。
        /// </summary>
        GeneralItemInfoForm = 157,

        /// <summary>
        /// 属性克制界面功能实现。
        /// </summary>
        ElementsRestrictionForm = 158,

        /// <summary>
        /// 英雄使用经验药水界面。
        /// </summary>
        HeroExpUpForm = 159,

        /// <summary>
        /// 运营活动界面。
        /// </summary>
        OperationActivityForm = 160,

        /// <summary>
        /// 未激活英雄详情。
        /// </summary>
        HeroInfoForm_Unpossessed = 161,

        /// <summary>
        /// 已激活英雄详情。
        /// </summary>
        HeroInfoForm_Possessed = 162,

        /// <summary>
        /// 物品信息界面之简单版。
        /// </summary>
        GeneralItemInfoForm2 = 163,

        /// <summary>
        /// 物品信息界面之详细版。
        /// </summary>
        GeneralItemInfoForm2_WithWhereToGet = 164,

        /// <summary>
        /// 升星成功界面。
        /// </summary>
        StarUpSuccessForm = 165,

        /// <summary>
        /// 升阶成功界面。
        /// </summary>
        QualityUpSuccessForm = 166,

        /// <summary>
        /// NewGear升阶成功界面。
        /// </summary>
        NewGearQualityUpSuccessForm = 167,

        /// <summary>
        /// 技能徽章背包界面。
        /// </summary>
        SkillBadgeBagForm = 168,

        /// <summary>
        /// 好友信息界面  。
        /// </summary>
        OtherPlayerInfoForm = 169,

        /// <summary>
        /// 宝箱奖品预览界面。
        /// </summary>
        ChestRewardSubForm = 170,

        /// <summary>
        /// 购买界面。
        /// </summary>
        CostConfirmDialog = 171,

        /// <summary>
        /// 装备升级成功界面。
        /// </summary>
        NewGearLevelUpFrom = 173,

        /// <summary>
        /// 离线竞技查看战绩。
        /// </summary>
        OfflineArenaRecordForm = 174,

        /// <summary>
        /// 物品信息提示界面。
        /// </summary>
        TipsForm = 176,

        /// <summary>
        /// 战斗VS界面。
        /// </summary>
        OfflineMatchForm = 177,

        /// <summary>
        /// 抽奖十连抽奖品展示界面。
        /// </summary>
        ChanceReceiveForm = 178,

        /// <summary>
        /// 首冲界面
        /// </summary>
        ChargeFirstForm = 179,

        /// <summary>
        /// 充值界面
        /// </summary>
        ChargeForm = 180,

        /// <summary>
        /// 福利中心
        /// </summary>
        WelfareListForm = 181,

        /// <summary>
        /// 任务界面
        /// </summary>
        TasksForm = 182,

        /// <summary>
        /// 对话界面
        /// </summary>
        DialogueForm = 183,

        /// <summary>
        /// Boss挑战界面
        /// </summary>
        ChallengeBossForm = 184,

        /// <summary>
        /// 风暴之塔界面
        /// </summary>
        StormTowerForm = 185,

        #endregion Lobby UIs

        #region Instance UIs

        /// <summary>
        /// 战斗。
        /// </summary>
        BattleForm = 200,

        /// <summary>
        /// 副本结算。
        /// </summary>
        InstanceResultForm = 201,

        /// <summary>
        /// 副本失败。
        /// </summary>
        InstanceFailureForm = 202,

        /// <summary>
        /// 离线竞技副本结算。
        /// </summary>
        ArenaBattleResultForm = 203,

        /// <summary>
        /// 离线竞技副本失败。
        /// </summary>
        ArenaBattleFailureForm = 204,

        /// <summary>
        /// 剧情副本失败。
        /// </summary>
        StoryInstanceFailureForm = 205,

        /// <summary>
        /// 时空裂隙副本失败。
        /// </summary>
        CosmosCrackInstanceFailureForm = 206,

        /// <summary>
        /// 翻翻棋副本失败。
        /// </summary>
        ChessBattleInstanceFailureForm = 207,

        /// <summary>
        /// 副本横幕界面。
        /// </summary>
        InstanceMovieForm = 208,

        /// <summary>
        /// 资源副本结算界面。
        /// </summary>
        InstanceForResourceResult = 209,

        /// <summary>
        /// 大乱斗结束界面。
        /// </summary>
        ActivityMeleeBattleOverForm = 210,

        /// <summary>
        /// 大乱斗结算界面。
        /// </summary>
        ActivityMeleeResultForm = 211,
        /// <summary>
        /// 单人匹配平局界面
        /// </summary>
        ActivitySinglePvpDrawForm = 212,
        /// <summary>
        /// 功能开放
        /// </summary>
        OpenFunctionDialog = 213,
        /// <summary>
        /// 新手引导
        /// </summary>
        NoviceGuideDialog = 214,
        #endregion Instance UIs
    }
}
