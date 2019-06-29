namespace Genesis.GameClient
{
    /// <summary>
    /// 事件类型编号。
    /// </summary>
    public enum EventId
    {
        GameEventStart = UnityGameFramework.Runtime.EventId.GameEventStart,

        /// <summary>
        /// 预加载资源被加载成功。
        /// </summary>
        LoadPreloadResourceSuccess,

        /// <summary>
        /// 预加载资源被加载失败。
        /// </summary>
        LoadPreloadResourceFailure,

        /// <summary>
        /// 时间轴组被加载成功。
        /// </summary>
        LoadTimeLineGroupSuccess,

        /// <summary>
        /// 时间轴组被加载失败。
        /// </summary>
        LoadTimeLineGroupFailure,

        /// <summary>
        /// 行为树加载成功。
        /// </summary>
        LoadBehaviorSuccess,

        /// <summary>
        /// 行为树加载失败。
        /// </summary>
        LoadBehaviorFailure,

        /// <summary>
        /// 即将开始切换场景。
        /// </summary>
        WillChangeScene,

        /// <summary>
        /// 登录验证服务器完成。
        /// </summary>
        LoginServer,

        /// <summary>
        /// 创建玩家。
        /// </summary>
        CreatePlayer,

        /// <summary>
        /// 登录游戏服务器的某个准备项完成。
        /// </summary>
        SignInPrepare,

        /// <summary>
        /// 切换场景开始。
        /// </summary>
        ChangeSceneStart,

        /// <summary>
        /// 切换场景完成。
        /// </summary>
        ChangeSceneComplete,

        /// <summary>
        /// 副本中角色死亡。
        /// </summary>
        CharacterDead,

        /// <summary>
        /// 副本中角色数值改变。
        /// </summary>
        CharacterPropertyChange,

        /// <summary>
        /// 副本中建筑死亡。
        /// </summary>
        BuildingDead,

        /// <summary>
        /// 副本中当前玩家准备完毕。
        /// </summary>
        InstanceMePrepared,

        /// <summary>
        /// 副本中对手准备完毕。
        /// </summary>
        InstanceOppPrepared,

        /// <summary>
        /// 副本准备好可以开始。
        /// </summary>
        InstanceReadyToStart,

        /// <summary>
        /// 副本中切换英雄开始。
        /// </summary>
        SwitchHeroStart,

        /// <summary>
        /// 副本中切换英雄结束。
        /// </summary>
        SwitchHeroComplete,

        /// <summary>
        /// 副本中英雄复活。
        /// </summary>
        ReviveHeroes,

        /// <summary>
        /// 副本中进入替换技能。
        /// </summary>
        EnterAltSkill,

        /// <summary>
        /// 副本中离开替换技能。
        /// </summary>
        LeaveAltSkill,

        /// <summary>
        /// 副本喊话开始。
        /// </summary>
        InstancePropagandaBegin,

        /// <summary>
        /// 副本喊话结束。
        /// </summary>
        InstancePropagandaEnd,

        /// <summary>
        /// 副本中霸体条被破。
        /// </summary>
        SteadyBreaked,

        /// <summary>
        /// 副本中霸体条恢复。
        /// </summary>
        SteadyRecovered,

        /// <summary>
        /// 副本中 Boss 出现。
        /// </summary>
        ShowBoss,

        /// <summary>
        /// 收到离开副本回包。
        /// </summary>
        LeaveInstanceResponse,

        /// <summary>
        /// 翻翻棋 -- 刷新成功。
        /// </summary>
        ChessBoardRefreshSuccess,

        /// <summary>
        /// 翻翻棋 -- 刷新失败。
        /// </summary>
        ChessBoardRefreshFailure,

        /// <summary>
        /// 翻翻棋 -- 打开棋子成功。
        /// </summary>
        ChessBoardOpenChessFieldSuccess,

        /// <summary>
        /// 翻翻棋 -- 打开棋子失败。
        /// </summary>
        ChessBoardOpenChessFieldFailure,

        /// <summary>
        /// 翻翻棋 -- 快速打开成功。
        /// </summary>
        ChessBoardQuickOpenSuccess,

        /// <summary>
        /// 翻翻棋 -- 快速打开失败。
        /// </summary>
        ChessBoardQuickOpenFailure,

        /// <summary>
        /// 翻翻棋 -- 使用炸弹成功。
        /// </summary>
        ChessBoardBombSuccess,

        /// <summary>
        /// 翻翻棋 -- 使用炸弹失败。
        /// </summary>
        ChessBoardBombFailure,

        /// <summary>
        /// 装备被选择。
        /// </summary>
        GearSelected,

        /// <summary>
        /// 装备被多选。
        /// </summary>
        GearMultiSelected,

        /// <summary>
        /// 装备合成完成。
        /// </summary>
        GearComposeComplete,

        /// <summary>
        /// 道具被选择。
        /// </summary>
        ItemSelected,

        /// <summary>
        /// 战魂被选择。
        /// </summary>
        SoulSelected,

        /// <summary>
        /// 副本中允许显示玩家英雄向导标志的状态发生改变。
        /// </summary>
        CanShowGuideIndicatorChanged,

        /// <summary>
        /// 星图中星星被打开。
        /// </summary>
        OpenMeridianStar,

        /// <summary>
        /// 获取聊天数据。
        /// </summary>
        GetChat,

        /// <summary>
        /// 发送聊天数据。
        /// </summary>
        SendChat,

        /// <summary>
        /// 搜索玩家成功。
        /// </summary>
        SearchPlayersSuccess,

        /// <summary>
        /// 获取推荐玩家列表成功。
        /// </summary>
        GetRecommendedPlayersSuccess,

        /// <summary>
        /// 好友请求已发送。
        /// </summary>
        FriendRequestSent,

        /// <summary>
        /// 能量赠予了好友。
        /// </summary>
        EnergyGivenToFriend,

        /// <summary>
        /// 被好友赠予了能量。
        /// </summary>
        EnergyGivenFromFriend,

        /// <summary>
        /// 收取了好友赠予的能量。
        /// </summary>
        EnergyReceivedFromFriend,

        /// <summary>
        /// 服务器 UTC 时间进入新的一天。
        /// </summary>
        ServerUtcTimeNewDay,

        /// <summary>
        /// 待处理好友请求数据变化。
        /// </summary>
        PendingFriendRequestsDataChanged,

        /// <summary>
        /// 添加了好友。
        /// </summary>
        FriendAdded,

        /// <summary>
        /// 拒绝了好友请求。
        /// </summary>
        FriendRequestRefused,

        /// <summary>
        /// 删除了好友。
        /// </summary>
        FriendDeleted,

        /// <summary>
        /// 获取玩家详情。
        /// </summary>
        GetPlayerInfo,

        /// <summary>
        /// 选择铭文。
        /// </summary>
        SelectEpigraph,

        /// <summary>
        /// 升级铭文。
        /// </summary>
        UpgradeEpigraph,

        /// <summary>
        /// 升级战魂。
        /// </summary>
        UpgradeHeroSoul,

        /// <summary>
        /// 选择了故事副本的对应英雄图标。
        /// </summary>
        InstanceStoryClickHeroItem,

        /// <summary>
        /// 扫荡了副本。
        /// </summary>
        CleanOutInstance,

        /// <summary>
        /// 物品出售完成。
        /// </summary>
        SellGoods,

        /// <summary>
        /// 更换英雄身上的装备。
        /// </summary>
        ChangeGear,

        /// <summary>
        /// 穿戴战魂成功。
        /// </summary>
        DressSoulSuccess,

        /// <summary>
        /// 实体死亡保留时间已到。
        /// </summary>
        DeadKeepTimeReached,

        /// <summary>
        /// 自动战斗状态改变。
        /// </summary>
        AutoFightStateChanged,

        /// <summary>
        /// 拥有者要求使用技能。
        /// </summary>
        OwnerAskToPerformSkill,

        /// <summary>
        /// 合成英雄成功。
        /// </summary>
        ComposeHeroComplete,

        /// <summary>
        /// 装备锻造活动 -- 创建团队。
        /// </summary>
        GearFoundryTeamCreated,

        /// <summary>
        /// 装备锻造活动 -- 加入团队。
        /// </summary>
        GearFoundryTeamJoined,

        /// <summary>
        /// 装备锻造活动 -- 匹配团队失败。
        /// </summary>
        GearFoundryTeamMatchingFailed,

        /// <summary>
        /// 装备锻造活动 -- 发送邀请。
        /// </summary>
        GearFoundryInvitationSent,

        /// <summary>
        /// 装备锻造活动 -- 收到邀请。
        /// </summary>
        GearFoundryInvitationReceived,

        /// <summary>
        /// 装备锻造活动 -- 回复邀请。
        /// </summary>
        GearFoundryInvitationResponded,

        /// <summary>
        /// 装备锻造活动 -- 团队人员变化。
        /// </summary>
        GearFoundryTeamPlayersChanged,

        /// <summary>
        /// 装备锻造活动 -- 退出队伍。
        /// </summary>
        GearFoundryLeftTeam,

        /// <summary>
        /// 装备锻造活动 -- 被踢出队伍。
        /// </summary>
        GearFoundryKickedFromTeam,

        /// <summary>
        /// 装备锻造活动 -- 完成锻造操作。
        /// </summary>
        GearFoundryPerformed,

        /// <summary>
        /// 装备锻造活动 -- 领取奖励。
        /// </summary>
        GearFoundryRewardClaimed,

        /// <summary>
        /// 装备锻造活动 -- 重置。
        /// </summary>
        GearFoundryReset,

        /// <summary>
        /// 离线竞技 -- 数据改变。
        /// </summary>
        OfflineArenaDataChanged,

        /// <summary>
        /// 离线竞技 -- 刷新列表数据成功。
        /// </summary>
        OfflineArenaPlayerListChanged,

        /// <summary>
        /// 离线竞技 -- 获取战报成功。
        /// </summary>
        OfflineArenaReportListChanged,

        /// <summary>
        /// 离线竞技 -- 活跃度奖励领取成功。
        /// </summary>
        OfflineArenaLivenessRewardClaimed,

        /// <summary>
        /// 离线竞技 -- 刷新对手数据成功。
        /// </summary>
        OfflineArenaOpponentDataChanged,

        /// <summary>
        /// 离线竞技 -- 取得结算数据。
        /// </summary>
        OfflineArenaBattleResultDataObtained,

        /// <summary>
        /// 离线竞技 -- 获取排行榜。
        /// </summary>
        OfflineArenaRankListObtained,

        /// <summary>
        /// 时空裂缝 -- 数据变化。
        /// </summary>
        CosmosCrackDataChanged,

        /// <summary>
        /// 手势 -- 拖拽开始。
        /// </summary>
        DragStart,

        /// <summary>
        /// 手势 -- 拖拽轮询。
        /// </summary>
        Drag,

        /// <summary>
        /// 手势 -- 拖拽结束。
        /// </summary>
        DragEnd,

        /// <summary>
        /// 手势 -- 刮扫开始。
        /// </summary>
        SwipeStart,

        /// <summary>
        /// 手势 -- 刮扫轮询。
        /// </summary>
        Swipe,

        /// <summary>
        /// 手势 -- 刮扫结束。
        /// </summary>
        SwipeEnd,

        /// <summary>
        /// 玩家数据改变。
        /// </summary>
        PlayerDataChanged,

        /// <summary>
        /// 大厅英雄数据改变。
        /// </summary>
        LobbyHeroDataChanged,

        /// <summary>
        /// 房间数据改变。
        /// </summary>
        RoomDataChanged,

        /// <summary>
        /// 英雄队伍数据改变。
        /// </summary>
        HeroTeamDataChanged,

        /// <summary>
        /// 翻翻棋副本专用的英雄队伍数据改变。
        /// </summary>
        HeroTeamDataChangedChessBattle,

        /// <summary>
        /// 道具数据改变。
        /// </summary>
        ItemDataChanged,
        /// <summary>
        /// 副本进度数据改变。
        /// </summary>
        InstanceProgressDataChanged,

        /// <summary>
        /// 翻翻棋 -- 敌方数据获取成功。
        /// </summary>
        ChessBoardGetEnemyDataSuccess,

        /// <summary>
        /// 翻翻棋 -- 敌方数据获取失败。
        /// </summary>
        ChessBoardGetEnemyDataFailure,

        /// <summary>
        /// 星图数据改变。
        /// </summary>
        MeridianDataChanged,


        /// <summary>
        /// 邮件数据改变。
        /// </summary>
        MailDataChanged,

        /// <summary>
        /// 好友数据改变。
        /// </summary>
        FriendDataChanged,

        /// <summary>
        /// 附近玩家数据改变。
        /// </summary>
        NearbyPlayerDataChanged,

        /// <summary>
        /// 抽奖数据改变。
        /// </summary>
        ChanceDataChanged,

        /// <summary>
        /// 副本参数发生改变。
        /// </summary>
        InstanceParamChanged,

        /// <summary>
        /// 装备升级。
        /// </summary>
        GearUpgraded,

        /// <summary>
        /// 装备强化。
        /// </summary>
        GearStrengthened,

        /// <summary>
        /// 装备合成。
        /// </summary>
        GearCompose,

        /// <summary>
        /// 网络定制错误。
        /// </summary>
        NetworkCustomError,

        /// <summary>
        /// 修改玩家头像。
        /// </summary>
        ChangePlayerPortrait,

        /// <summary>
        /// 场景传送事件。
        /// </summary>
        TriggerPortal,

        /// <summary>
        /// 获得了玩家体力相关数据。
        /// </summary>
        GotPlayerEnergy,

        /// <summary>
        /// 获得了玩家名字验证结果。
        /// </summary>
        CheckNameComplete,

        /// <summary>
        /// 提醒器更新。
        /// </summary>
        ReminderUpdated,

        /// <summary>
        /// 伤害记录到统计信息。
        /// </summary>
        StatDamageRecorded,

        /// <summary>
        /// 开始 PVP 匹配。
        /// </summary>
        StartPvpMatchingEventArgs,

        /// <summary>
        /// 停止 PVP 匹配。
        /// </summary>
        StopPvpMatchingEventArgs,

        /// <summary>
        /// PVP 匹配成功。
        /// </summary>
        PvpMatchingSuccessEventArgs,

        /// <summary>
        /// 房间准备完毕。
        /// </summary>
        RoomReady,

        /// <summary>
        /// 玩家英雄移动更新。
        /// </summary>
        MyHeroMovingUpdate,

        /// <summary>
        /// 收到其他玩家英雄移动更新。
        /// </summary>
        OtherHeroMovingUpdate,

        /// <summary>
        /// 玩家英雄释放技能开始。
        /// </summary>
        MyHeroPerformSkillStart,

        /// <summary>
        /// 玩家英雄释放技能结束。
        /// </summary>
        MyHeroPerformSkillEnd,

        /// <summary>
        /// 房间连接关闭。
        /// </summary>
        RoomClosed,

        /// <summary>
        /// 收到房间对战结果推送。
        /// </summary>
        RoomBattleResultPushed,

        /// <summary>
        /// PVP 英雄队伍数据变化。
        /// </summary>
        PvpArenaHeroTeamDataChanged,

        /// <summary>
        /// 网络同步实体技能快进。
        /// </summary>
        EntitySkillFFFromNetwork,

        /// <summary>
        /// 更新技能冲刺。
        /// </summary>
        UpdateSkillRushing,

        /// <summary>
        /// 网络同步实体技能冲刺。
        /// </summary>
        EntitySkillRushingFromNetwork,

        /// <summary>
        /// 时间轴中给自身加 Buff。
        /// </summary>
        SelfAddBuffInTimeLine,

        /// <summary>
        /// 武器显示完成。
        /// </summary>
        ShowWeaponsComplete,

        /// <summary>
        /// 副本开始时间更新。
        /// </summary>
        InstanceStartTimeChanged,

        /// <summary>
        /// 房间内实体同步。
        /// </summary>
        RoomEntityForceSync,

        /// <summary>
        /// 手势 -- 简单点击。
        /// </summary>
        SimpleTap,

        /// <summary>
        /// 手势 -- 触摸抬起。
        /// </summary>
        TouchUp,

        /// <summary>
        /// PVP 基本信息获取。
        /// </summary>
        SinglePvpInfoChanged,

        /// <summary>
        /// 成就系统完成并领取奖励。
        /// </summary>
        ClaimAchievementReward,

        /// <summary>
        /// 获取成就系统进度。
        /// </summary>
        PushAchievementProgress,

        /// <summary>
        /// 每日任务完成并领取奖励。
        /// </summary>
        ClaimDailyQuestReward,

        /// <summary>
        /// 获取每日任务进度。
        /// </summary>
        UpdateDailyQuest,

        /// <summary>
        /// 每日任务主动刷新。
        /// </summary>
        DailyQuestsRefreshed,

        /// <summary>
        /// 连续点击技更新输入。
        /// </summary>
        SkillContinualTapUpdateInput,

        /// <summary>
        /// 时间缩放任务结束。
        /// </summary>
        TimeScaleTaskFinish,

        /// <summary>
        /// 请求连接大厅服务器。
        /// </summary>
        RequestConnectLobbyServer,

        /// <summary>
        /// 回应连接大厅服务器。
        /// </summary>
        ResponseConnectLobbyServer,

        /// <summary>
        /// 请求登入大厅服务器。
        /// </summary>
        RequestSignInLobbyServer,

        /// <summary>
        /// 回应登入大厅服务器。
        /// </summary>
        ResponseSignInLobbyServer,

        /// <summary>
        /// 加载并实例化界面实例成功。
        /// </summary>
        LoadAndInstantiateUIInstanceSuccess,

        /// <summary>
        /// 加载并实例化界面实例失败。
        /// </summary>
        LoadAndInstantiateUIInstanceFailure,

        /// <summary>
        /// 收到运营活动事件。
        /// </summary>
        OperationActivityResponse,

        /// <summary>
        /// 获取Pvp排行榜。
        /// </summary>
        GetSinglePvpRank,

        /// <summary>
        ///
        /// </summary>
        GetWorldPvpRank,

        /// <summary>
        /// 加载 Lua 脚本成功。
        /// </summary>
        LoadLuaScriptSuccess,

        /// <summary>
        /// 加载 Lua 脚本失败。
        /// </summary>
        LoadLuaScriptFailure,

        /// <summary>
        /// Npc死亡掉落金币。
        /// </summary>
        NpcDeadDropIcons,

        /// <summary>
        /// 木桶等物品掉落事件
        /// </summary>
        BuildingDeadDropIcons,

        /// <summary>
        /// 断线重连，获取Room状态。
        /// </summary>
        GetRoomStatus,

        /// <summary>
        /// 发送连接Room请求。
        /// </summary>
        ConnectRoom,

        /// <summary>
        /// Online霸体条状态变化。
        /// </summary>
        PushEntitySteadyChanged,

        /// <summary>
        /// 特定服务器错误。
        /// </summary>
        ServerError,

        /// <summary>
        /// 摄像机动画开始播放。
        /// </summary>
        CameraAnimStartToPlay,

        /// <summary>
        /// 摄像机动画加载失败。
        /// </summary>
        CameraAnimLoadFailure,

        /// <summary>
        /// 摄像机动画取消。
        /// </summary>
        CameraAnimCancel,

        /// <summary>
        /// 摄像机动画停止。
        /// </summary>
        CameraAnimStopped,

        /// <summary>
        /// 摄像机动画即将开始。
        /// </summary>
        CameraAnimAboutToStart,

        /// <summary>
        /// 摄像机动画即将结束。
        /// </summary>
        CameraAnimAboutToStop,

        /// <summary>
        /// 英雄升品道具数据变化。
        /// </summary>
        HeroQualityItemDataChange,

        /// <summary>
        /// 使用英雄加经验道具。
        /// </summary>
        UseHeroExpItem,

        /// <summary>
        /// 英雄升阶。
        /// </summary>
        IncreaseHeroQualityLevel,

        /// <summary>
        /// 装备升阶。
        /// </summary>
        NewGearQualityLevelUp,

        /// <summary>
        /// 装备强化。
        /// </summary>
        NewGearStrengthen,

        /// <summary>
        /// 英雄升星。
        /// </summary>
        HerostarLevelUp,

        /// <summary>
        /// 英雄技能数据变化。
        /// </summary>
        HeroSkillDataChanged,

        /// <summary>
        /// 传送状态通知到 <see cref="BattleForm"/>。
        /// </summary>
        PortagingAnimation,

        /// <summary>
        /// 传送过程播放特效。
        /// </summary>
        PortagingEffect,

        /// <summary>
        ///当技能徽章数据改变。
        /// </summary>
        OnSkillBadgeDataChanged,

        /// <summary>
        /// 获取玩家在线详情。
        /// </summary>
        GetPlayerOnlineStatus,

        /// <summary>
        /// 当触发蓄力技能。
        /// </summary>
        PerformChargeSkill,

        /// <summary>
        ///当技能徽章背包数据改变。
        /// </summary>
        OnSkillBadgeBagDataChanged,

        /// <summary>
        ///
        /// </summary>
        AddBlackList,

        /// <summary>
        ///
        /// </summary>
        RefreshNearbyPlayers,

        /// <summary>
        ///
        /// </summary>
        ShopDataChanged,

        /// <summary>
        ///
        /// </summary>
        PurchaseInShop,

        /// <summary>
        /// 副本打开宝箱领取奖励。
        /// </summary>
        OpenInstanceGroupChest,

        /// <summary>
        ///获取排行榜
        /// </summary>
        GetRankListData,

        /// <summary>
        /// 领取邮件的附件。
        /// </summary>
        PickMailAttachment,

        /// <summary>
        /// 离开资源副本。
        /// </summary>
        LeaveInstanceForResourceResponse,

        /// <summary>
        /// 资源副本数据改变。
        /// </summary>
        InstancesForResourceDataChanged,

        /// <summary>
        /// 签到成功。
        /// </summary>
        DailyLogined,

        /// <summary>
        /// 签到跨天。
        /// </summary>
        DailyLoginAcrossDay,

        /// <summary>
        /// 成就数据更新。
        /// </summary>
        UpdateAchievement,

        /// <summary>
        ///
        /// </summary>
        ClaimActivenessChest,

        /// <summary>
        ///需要显示Hud的Bool值改变。
        /// </summary>
        ShouldShowHudValueChanged,

        /// <summary>
        /// 复活。
        /// </summary>
        Revive,

        /// <summary>
        ///大乱斗队伍积分改变时。
        /// </summary>
        OnMimicMeleeChanged,

        /// <summary>
        /// 模拟乱斗中可被攻击实体显示。
        /// </summary>
        TargetableObjectShowInMimicMelee,

        /// <summary>
        /// 模拟乱斗中可作为目标的实体隐藏。
        /// </summary>
        TargetableObjectHideInMimicMelee,
        /// <summary>
        /// 请求1v1匹配的结果
        /// </summary>
        RequestSingleMatch,
        /// <summary>
        /// 请求1v1匹配成功了开始游戏
        /// </summary>
        SingleMatchSuccess,
        /// <summary>
        /// 向room服务器注册当前玩家
        /// </summary>
        RegistPlayerToRoom,
        /// <summary>
        /// room向客户端推送1v1游戏开始，进入倒计时5秒
        /// </summary>
        PvpStart,
        /// <summary>
        /// 客户端向服务器请求当前战斗的状态的返回
        /// </summary>
        GetRoomBattleStatus,
        /// <summary>
        /// 客户端重新获得运行焦点，即从后台切回前台
        /// </summary>
        ApplicationFocus,
        /// <summary>
        /// 1v1中另外一个玩家掉线或者重新连接
        /// </summary>
        SinglePvpOtherPlayerDisconnect,

        /// <summary>
        /// 刷新充值商店
        /// </summary>
        RefreshMonthCard,

        /// <summary>
        /// 获取购买过的商品状态（礼包和钻石）
        /// </summary>
        GetItemStatus,

        /// <summary>
        /// 礼包领取成功
        /// </summary>
        GetGiftSuccess,

        /// <summary>
        /// 礼包领取失败
        /// </summary>
        GetGiftFailed,
        /// <summary>
        /// 获取排名和积分信息
        /// </summary>
        GetRankData,

        /// <summary>
        /// 收到购买的物品
        /// </summary>
        ReceiveBuyItem,

        /// <summary>
        /// 任务列表刷新
        /// </summary>
        TaskListChanged,

        /// <summary>
        /// 收到并展示物品
        /// </summary>
        ReceiveAndShowItems,

        /// <summary>
        /// 任务奖励领取成功
        /// </summary>
        ClaimTaskRewardSuccess,

        /// <summary>
        /// 风暴之塔数据刷新
        /// </summary>
        StromTowerDataChange,

        /// <summary>
        /// 挑战副本数据刷新
        /// </summary>
        ChallengeBossDataChange,

        /// <summary>
        /// 加载Title成功
        /// </summary>
        LoadTitleSuccess,
		
        /// <summary>
        /// 玩家升级动画后回调
        /// </summary>
        PlayerLevelUpAnimationCallBack,
		
        /// <summary>
        /// 功能开放动画播放结束
        /// </summary>
        OpenFunctionAnimationEnd,
		
		 /// <summary>
        /// 在线奖励数据改变
        /// </summary>
        OnlineRewardsDataChange,

        /// <summary>
        /// 七日登录数据改变
        /// </summary>
        SevenDayLoginDataChange,
		
		 /// <summary>
        /// 领取签到宝箱成功
        /// </summary>
        ClaimSignInBoxSuccess,

        /// <summary>
        /// 补签成功
        /// </summary>
        RetroactiveSuccess,

        /// <summary>
        /// 获得系统消息
        /// </summary>
        GetSystemMsg,
    }
}
