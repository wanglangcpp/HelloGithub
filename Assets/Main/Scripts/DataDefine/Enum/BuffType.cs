namespace Genesis.GameClient
{
    /// <summary>
    /// Buff 的种类。
    /// </summary>
    public enum BuffType
    {
        /// <summary>
        /// 未定义。
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// 霸体（无视状态伤害）。
        /// </summary>
        StateHarmFree = 1,

        /// <summary>
        /// 无敌（无视任何伤害）。
        /// </summary>
        StateAndNumHarmFree = 2,

        /// <summary>
        /// 周期性对该 Buff 的目标形成 Impact。
        /// </summary>
        PeriodicalImpact = 3,

        /// <summary>
        /// 假死。
        /// </summary>
        FakeDeath = 4,

        /// <summary>
        /// 显示或隐藏武器。
        /// </summary>
        ShowOrHideWeapons = 5,

        /// <summary>
        /// 高级霸体（无视状态伤害）。
        /// </summary>
        StateHarmFreeAdvanced = 6,

        /// <summary>
        /// 屏蔽伤害中的位移部分。
        /// </summary>
        DisplacementHarmFree = 7,

        /// <summary>
        /// 增加霸体条值。
        /// </summary>
        AddSteadyValueByPct = 8,

        /// <summary>
        /// 不受浮空状态改为普通僵直状态。
        /// </summary>
        ImmuneFloatImpact = 9,

        /// <summary>
        /// 改变最大血量。
        /// </summary>
        ChangeMaxHP = 5001,

        /// <summary>
        /// 改变移动速度。
        /// </summary>
        ChangeSpeed = 5002,

        /// <summary>
        /// 改变物理攻击力。
        /// </summary>
        ChangePhysicalAttack = 5003,

        /// <summary>
        /// 改变物理防御力。
        /// </summary>
        ChangePhysicalDefense = 5004,

        /// <summary>
        /// 改变法术攻击力。
        /// </summary>
        ChangeMagicAttack = 5005,

        /// <summary>
        /// 改变法术防御力。
        /// </summary>
        ChangeMagicDefense = 5006,

        /// <summary>
        /// 策划保留起始值。在此值和 <see cref="Genesis.GameClient.BuffType.DesignerReserveEnd"/> 之间的值请勿随意使用。
        /// </summary>
        DesignerReserveBegin = 6000,

        /// <summary>
        /// 光属性减益。
        /// </summary>
        LightDebuff = 6000,

        /// <summary>
        /// 策划保留终止值。
        /// </summary>
        DesignerReserveEnd = 6099,
    }
}
