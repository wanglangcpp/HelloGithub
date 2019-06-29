namespace Genesis.GameClient
{
    public static partial class Constant
    {
        public static class Buff
        {
            /// <summary>
            /// 每个角色同时可以生效的 Buff 的最大个数。
            /// </summary>
            public const int MaxBuffPerBuffTarget = 60;

            /// <summary>
            /// 免于数值伤害时，角色 HUD 文字的类型。
            /// </summary>
            public const int NumHarmFreeHudTextType = 3;

            /// <summary>
            /// 用于复活时的无敌Buff。
            /// </summary>
            public const int ReviveBuffId = 2013;

            public static class UserData
            {
                public const string OwnerSkillBadgesKey = "OwnerSkillBadges";
            }
        }
    }
}
