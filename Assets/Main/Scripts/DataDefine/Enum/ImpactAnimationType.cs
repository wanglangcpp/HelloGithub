namespace Genesis.GameClient
{
    public enum ImpactAnimationType
    {
        None = -1,

        /// <summary>
        /// 地面普通打击。
        /// </summary>
        HitAnimation = 0,

        /// <summary>
        /// 地面强力打击。
        /// </summary>
        HitStrongAnimation = 1,

        /// <summary>
        /// 保持站立姿态。
        /// </summary>
        StandingAnimation = 2,

        /// <summary>
        /// 旋转动画。
        /// </summary>
        RotateAnimation = 3,

        /// <summary>
        /// 空中普通打击。
        /// </summary>
        HitAirAnimation = 10,

        /// <summary>
        /// 空中强力打击重击弹地。
        /// </summary>
        HitAirAnimationStrong = 11,

        /// <summary>
        /// 浮空第一段。
        /// </summary>
        FloatAnimation = 20,

        /// <summary>
        /// 浮空第二段。
        /// </summary>
        FallingAnimation = 21,

        /// <summary>
        /// 击飞第一段。
        /// </summary>
        BlownAwayAnimation = 30,

        /// <summary>
        /// 击飞第二段。
        /// </summary>
        BlownAwayFallingAnimtion = 31,
    }
}
