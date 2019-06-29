namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 道具使用的常量
        /// </summary>
        public static class Soul
        {
            public enum EffectId
            {
                MaxHP = 1,
                PhysicalAttack,
                MagicalAttack,
                PhysicalArmor,
                MagicalArmor,
                CriticalHitProb,
                CriticalHitRate,
                PhysicalAtkHPAbsorbRate,
                MagicAtkHPAbsorbRate,
                AntiCriticalHitProb,
                OppPhysicalDfsReduceRate,
                OppMagicDfsReduceRate,
                PhysicalAtkReflectRate,
                MagicAtkReflectRate,
                RecoverHP
            }
        }
    }
}
