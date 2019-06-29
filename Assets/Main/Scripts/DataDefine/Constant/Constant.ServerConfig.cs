namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 服务器配置。
        /// </summary>
        public static class ServerConfig
        {
            // 注意：本类中所有的常量应两两不同！
            public static class Friend
            {
                /// <summary>
                /// 最大好友个数。
                /// </summary>
                public const string MaxFriendCount = "Friend.MaxFriendCount";

                /// <summary>
                /// 好友功能的开启等级。
                /// </summary>
                public const string UnlockLevel = "Friend.UnlockLevel";

                /// <summary>
                /// 好友每天赠送体力次数。
                /// </summary>
                public const string DailyEnergyGiveTimes = "Friend.DailyEnergyGiveTimes";

                /// <summary>
                /// 好友体力领取次数上限。
                /// </summary>
                public const string DailyEnergyClaimTimes = "Friend.DailyEnergyClaimTimes";

                /// <summary>
                /// 好友赠送体力的刷新时间
                /// </summary>
                public const string RefreshUtcTime = "Friend.RefreshUtcTime";
            }

            public static class Chat
            {
                /// <summary>
                /// 聊天世界频道开启等级。
                /// </summary>
                public const string UnlockWorldChannelLevel = "Chat.UnlockWorldChannelLevel";

                /// <summary>
                /// 聊天发送文字上限。
                /// </summary>
                public const string MaxMessageLength = "Chat.MaxMessageLength";

                /// <summary>
                /// 发送世界频道聊天间隔。
                /// </summary>
                public const string SendWorldMessageInterval = "Chat.SendWorldMessageInterval";

                /// <summary>
                /// 私聊保留玩家数量上限。
                /// </summary>
                public const string PrivateSavePlayerCount = "Chat.PrivateSavePlayerCount";

                /// <summary>
                /// 私聊每玩家保留信息数量上限。
                /// </summary>
                public const string PrivateSaveMessageCountPerPlayer = "Chat.PrivateSaveMessageCountPerPlayer";

                /// <summary>
                /// 聊天屏蔽人数上限。
                /// </summary>
                public const string MaxBlackListCount = "Chat.MaxBlackListCount";
            }

            public static class Mail
            {
                /// <summary>
                /// 邮件的最大显示数量
                /// </summary>
                public const string MaxCount = "Mail.MaxCount";
            }

            public static class Player
            {
                /// <summary>
                /// 主城玩家英雄位置同步时间
                /// </summary>
                public const string ReportLobbyPositionInterval = "Player.ReportLobbyPositionInterval";
            }

            public static class System
            {
                /// <summary>
                /// 是否忽略警告类日志。
                /// </summary>
                public const string IgnoreWarningLog = "System.IgnoreWarningLog";
            }

            public static class Account
            {
                /// <summary>
                /// 玩家名称最大长度。
                /// </summary>
                public const string PlayerNameMaxLength = "Account.MaxPlayerNameLength";
            }

            public static class Instance
            {
                /// <summary>
                /// 打一次副本需要消耗的体力。
                /// </summary>
                public const string CostEnergy = "Instance.CostEnergy";
            }

            public static class Meridian
            {
                /// <summary>
                /// 星图开启等级。
                /// </summary>
                public const string UnlockLevel = "Meridian.UnlockLevel";
            }

            public static class DailyQuest
            {
                /// <summary>
                /// 任务开启等级
                /// </summary>
                public const string UnlockLevel = "DailyQuest.UnlockLevel";
            }
        }
    }
}
