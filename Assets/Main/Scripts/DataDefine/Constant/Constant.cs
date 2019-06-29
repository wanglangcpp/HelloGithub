namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 每个英雄技能组个数（含连续普攻技能、常规技能和闪避技能，不含切换技能和怒气技能）。
        /// </summary>
        public const int SkillGroupCount = 5;

        /// <summary>
        /// 每个英雄技能组个数（含所有技能，包括未开放使用的技能位置）。
        /// </summary>
        public const int TotalSkillGroupCount = 10;

        /// <summary>
        /// 切换技能索引。
        /// </summary>
        public const int SwitchSkillIndex = 9;

        /// <summary>
        /// 闪避技能索引。
        /// </summary>
        public const int DodgeSkillIndex = 4;

        /// <summary>
        /// 战场英雄最大个数。
        /// </summary>
        public const int MaxBattleHeroCount = 3;

        /// <summary>
        /// 副本目标个数。
        /// </summary>
        public const int InstanceRequestCount = 3;

        /// <summary>
        /// 副本掉落种类个数
        /// </summary>
        public const int InstanceDropTypeMaxCount = 9;

        /// <summary>
        /// 怒气技能最大点数。
        /// </summary>
        public const int AngerSkillMaxPoint = 1000;

        /// <summary>
        /// 随机掉落配置个数。
        /// </summary>
        public const int DropItemCount = 50;

        /// <summary>
        /// 大厅场景编号。
        /// </summary>
        public const int LobbySceneId = 1;

        /// <summary>
        /// 受击点个数。
        /// </summary>
        public const int HitPointCount = 3;

        /// <summary>
        /// 英雄星级数。
        /// </summary>
        public const int HeroStarLevelCount = 5;

        /// <summary>
        /// 英雄飞升所需装备最大数量。
        /// </summary>
        public const int HeroElevationNeedGearMaxCount = 4;

        /// <summary>
        /// 英雄飞升最大等级。
        /// </summary>
        public const int MaxElevationLevel = 15;

        /// <summary>
        /// 英雄装备槽个数。
        /// </summary>
        public const int HeroMaxGearSlot = 6;

        /// <summary>
        /// 英雄战魂槽个数。
        /// </summary>
        public const int HeroMaxSoulSlot = 6;

        /// <summary>
        /// 最大强化等级。
        /// </summary>
        public const int MaxGearStrengthenLevel = 5;

        /// <summary>
        /// 默认动画过渡时间。
        /// </summary>
        public const float DefaultAnimCrossFadeDuration = 0.3f;

        /// <summary>
        /// 简体中文代码页 (GB2312)。
        /// </summary>
        public const int SimplifiedChineseCodePage = 936;

        /// <summary>
        /// 副本喊话队列容量
        /// </summary>
        public const int InstancePropagandaQueueCapacity = 20;

        /// <summary>
        /// 玩家体力上限值。
        /// </summary>
        public const int PlayerMaxEnergy = 120;

        /// <summary>
        /// 装备合成需要材料数。
        /// </summary>
        public const int GearComposeMaterialCount = 5;

        /// <summary>
        /// 装备合成需要材料数。
        /// </summary>
        public const int GearQualityCount = 5;

        /// <summary>
        /// buff最多身上挂多少个特效
        /// </summary>
        public const int MaxCharacterBuffEffectCount = 3;

        /// <summary>
        /// 铭文最多能升几级
        /// </summary>
        public const int MaxEpigraphLevel = 5;

        /// <summary>
        /// 抽奖卡牌最大数量。
        /// </summary>
        public const int MaxChancedCardCount = 10;

        /// <summary>
        /// 主界面聊天显示多少秒内的聊天内容
        /// </summary>
        public const int BroadcastChatMsgTime = 2;

        /// <summary>
        /// 技能效果描述数量。
        /// </summary>
        public const int SkillEffectDescriptionCount = 5;

        /// <summary>
        /// 血条保留时间。
        /// </summary>
        public const float HPBarKeepTime = 3f;

        /// <summary>
        /// 血条半透时间。
        /// </summary>
        public const float HPBarAlphaTime = 0.3f;

        /// <summary>
        /// 副本中 AI 行为个数上限。
        /// </summary>
        public const int AIBehaviorMaxCountInInstance = 4;

        /// <summary>
        /// 副本中寻径点个数上限。
        /// </summary>
        public const int GuidePointMaxCountInInstance = 30;

        /// <summary>
        /// 副本中宝箱个数上限。
        /// </summary>
        public const int ChestsMaxCountInInstance = 10;

        /// <summary>
        /// 默认的 PVPAI 类型副本编号。
        /// </summary>
        public const int DefaultPlayerVSPlayerAIInstanceId = 1;

        /// <summary>
        /// 切场景小纸条数量。
        /// </summary>
        public const int LoadingTipsCount = 100;

        /// <summary>
        /// 每种星图可以领取个数。
        /// </summary>
        public const int MaxMeridianAstrolabe = 30;

        /// <summary>
        /// 星图总数。
        /// </summary>
        public const int MeridianTotality = 360;

        /// <summary>
        /// 一套武器的最大武器数量。
        /// </summary>
        public const int MaxWeaponCountInSuite = 10;

        /// <summary>
        /// 飞升所需要消耗的材料数量。
        /// </summary>
        public const int GearElevationMaterialCount = 4;

        /// <summary>
        /// 乱斗小地图队友1的ID。
        /// </summary>
        public const int MimicMeleeMiniMapOneNpcId = 7030101;

        /// <summary>
        /// 乱斗小地图队友2的ID。
        /// </summary>
        public const int MimicMeleeMiniMapTwoNpcId = 7030201;

        /// <summary>
        /// 集成 Bugly 的应用标识。创建者 QQ 号：3341885091。
        /// </summary>
        public const string BuglyAppId =
#if UNITY_IOS
            "900012024"
#elif UNITY_ANDROID
            "900011333"
#else
            ""
#endif
            ;

        /// <summary>
        /// 一个服务器选择区所包含的最大服务器个数。
        /// </summary>
        public const int OneServerAreaContainMaxServerCount = 10;

        /// <summary>
        /// 角色创建时可选头像数。
        /// </summary>
        public const int FreePortraitCount = 10;

        /// <summary>
        /// 时间轴用户数据：快进关键字。
        /// </summary>
        public const string TimeLineFastForwardTillKey = "FastForwardTill";

        /// <summary>
        /// 时间轴用户数据：给 NPC 加 Buff 关键字。
        /// </summary>
        public const string TimeLineAddBuffToNpcsKey = "AddBuffToNpcs";

        /// <summary>
        /// 时间轴用户数据：技能等级关键字。
        /// </summary>
        public const string TimeLineSkillLevelKey = "SkillLevel";

        /// <summary>
        /// 时间轴用户数据：技能索引关键字。
        /// </summary>
        public const string TimeLineSkillIndexKey = "SkillIndex";

        /// <summary>
        /// 时间轴用户数据：所有者技能等级关键字。
        /// </summary>
        public const string TimeLineOwnerSkillLevelKey = "OwnerSkillLevel";

        /// <summary>
        /// 时间轴用户数据：所有者技能索引关键字。
        /// </summary>
        public const string TimeLineOwnerSkillIndexKey = "OwnerSkillIndex";

        /// <summary>
        /// 成就参数个数。
        /// </summary>
        public const int AchievementParamCount = 5;

        /// <summary>
        /// 成就奖励个数。
        /// </summary>
        public const int AchievementRewardCount = 5;

        /// <summary>
        /// 每日任务参数个数。
        /// </summary>
        public const int DailyQuestParamCount = 5;

        /// <summary>
        /// 每日任务奖励个数。
        /// </summary>
        public const int DailyQuestRewardCount = 5;

        /// <summary>
        /// 默认英雄技能等级。
        /// </summary>
        public readonly static int[] DefaultHeroSkillLevels = new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 1 };

        /// <summary>
        /// 英雄技能是否可升级。
        /// </summary>
        public readonly static bool[] HeroSkillCanLevelUp = new bool[] { true, true, true, true, false, true, true, true, true, true };

        /// <summary>
        /// 世界聊天最小发送等级。
        /// </summary>
        public const int WorldChatSendMinLevel = 10;

        /// <summary>
        /// 系统消息最大存储数据。
        /// </summary>
        public const int SystemChatMaxBroadcastNum = 10;

        /// <summary>
        /// PVP 默认英雄图片纹理编号。
        /// </summary>
        public const int HeroDefaultTextureId = 10000;

        /// <summary>
        /// PVP 每日最大奖励次数。
        /// </summary>
        public const int PvpMaxRewardNum = 5;

        /// <summary>
        /// 英雄升品道具槽个数。
        /// </summary>
        public const int HeroQualityLevelItemSlotCount = 6;

        /// <summary>
        /// Item默认空白时的空底。
        /// </summary>
        public const int EmptyItemIconId = 426;

        /// <summary>
        /// 角色展示时互动动作个数。
        /// </summary>
        public const int CharacterForShowInteractionCount = 3;

        /// <summary>
        /// 重启专用摄像机的名称。
        /// </summary>
        public const string ShuttingDownCameraName = "Shutting Down Camera";
    }
}
