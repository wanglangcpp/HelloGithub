using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备属性显示器。
    /// </summary>
    /// <typeparam name="T">单条属性脚本类型。</typeparam>
    public class GearAttributeDisplayer<T> where T : class
    {
        public event GameFrameworkFunc<int, T> GetItemDelegate;

        public event GameFrameworkAction<T, string, string> SetNameAndCurrentValueDelegate;

        public event GameFrameworkAction<T, string, string, string> SetNameAndValuesDelegate;

        private readonly GearData m_GearData;

        private GearAttributeNewValueMask m_NewValueMask;

        private delegate ValueStrs GetValueStrings(GearData data, int newLevel, int newStrenghthenLevel);

        private static readonly Dictionary<string, GetValueStrings> s_ValueStringsGetters = new Dictionary<string, GetValueStrings>
        {
            { Constant.AttributeName.PhysicalAttackKey, GetPhysicalAttack },
            { Constant.AttributeName.MagicAttackKey, GetMagicAttack },
            { Constant.AttributeName.PhysicalDefenseKey, GetPhysicalDefense },
            { Constant.AttributeName.MagicDefenseKey, GetMagicDefense },
            { Constant.AttributeName.MaxHPKey, GetMaxHP },
            { Constant.AttributeName.PhysicalAtkHPAbsorbRateKey, GetPhysicalAtkHPAbsorbRate },
            { Constant.AttributeName.PhysicalAtkReflectRateKey, GetPhysicalAtkReflectRate },
            { Constant.AttributeName.OppPhysicalDfsReduceRateKey, GetOppPhysicalDfsReduceRate },
            { Constant.AttributeName.MagicAtkHPAbsorbRateKey, GetMagicAtkHPAbsorbRate },
            { Constant.AttributeName.MagicAtkReflectRateKey, GetMagicAtkReflectRate },
            { Constant.AttributeName.OppMagicDfsReduceRateKey, GetOppMagicDfsReduceRate },
            { Constant.AttributeName.DamageReductionRateKey, GetDamageReductionRate },
            { Constant.AttributeName.AdditionalDamageKey, GetAdditionalDamage },
            { Constant.AttributeName.CriticalHitRateKey, GetCriticalHitRate },
            { Constant.AttributeName.CriticalHitProbKey, GetCriticalHitProb },
            { Constant.AttributeName.AntiCriticalHitProbKey, GetAntiCriticalHitProb },
            { Constant.AttributeName.ReducedSkillCoolDownRateKey, GetReducedSkillCoolDownRate },
            { Constant.AttributeName.ReducedHeroSwitchCDRateKey, GetReducedHeroSwitchCDRate },
        };

        public GearAttributeDisplayer(GearData data, GearAttributeNewValueMask expectedValueMask)
        {
            m_GearData = data;
            m_NewValueMask = expectedValueMask;
        }

        public int Run()
        {
            int index = 0;
            index = ShowAttribute(Constant.AttributeName.MaxHPKey, index);
            index = ShowAttribute(Constant.AttributeName.PhysicalAttackKey, index);
            index = ShowAttribute(Constant.AttributeName.MagicAttackKey, index);
            index = ShowAttribute(Constant.AttributeName.PhysicalDefenseKey, index);
            index = ShowAttribute(Constant.AttributeName.MagicDefenseKey, index);
            index = ShowAttribute(Constant.AttributeName.CriticalHitProbKey, index);
            index = ShowAttribute(Constant.AttributeName.CriticalHitRateKey, index);
            index = ShowAttribute(Constant.AttributeName.OppPhysicalDfsReduceRateKey, index);
            index = ShowAttribute(Constant.AttributeName.OppMagicDfsReduceRateKey, index);
            index = ShowAttribute(Constant.AttributeName.PhysicalAtkReflectRateKey, index);
            index = ShowAttribute(Constant.AttributeName.MagicAtkReflectRateKey, index);
            index = ShowAttribute(Constant.AttributeName.PhysicalAtkHPAbsorbRateKey, index);
            index = ShowAttribute(Constant.AttributeName.MagicAtkHPAbsorbRateKey, index);
            index = ShowAttribute(Constant.AttributeName.AntiCriticalHitProbKey, index);
            index = ShowAttribute(Constant.AttributeName.ReducedSkillCoolDownRateKey, index);
            index = ShowAttribute(Constant.AttributeName.DamageReductionRateKey, index);
            index = ShowAttribute(Constant.AttributeName.AdditionalDamageKey, index);
            index = ShowAttribute(Constant.AttributeName.ReducedHeroSwitchCDRateKey, index);
            return index;
        }

        private int ShowAttribute(string key, int index)
        {
            int newLevel = (m_NewValueMask & GearAttributeNewValueMask.LevelPlusOne) > 0 ? (m_GearData.Level + 1) : m_GearData.Level;
            int newStrenghthenLevel = (m_NewValueMask & GearAttributeNewValueMask.StrengthLevelPlusOne) > 0 ? (m_GearData.StrengthenLevel + 1) : m_GearData.StrengthenLevel;

            GetValueStrings getValueStrings;
            if (!s_ValueStringsGetters.TryGetValue(key, out getValueStrings))
            {
                return index;
            }

            ValueStrs valStrs = getValueStrings(m_GearData, newLevel, newStrenghthenLevel);
            if (string.IsNullOrEmpty(valStrs.CurValStr))
            {
                return index;
            }

            var itemScript = GetItemDelegate(index);

            if (SetNameAndCurrentValueDelegate != null)
            {
                SetNameAndCurrentValueDelegate(itemScript, GameEntry.Localization.GetString(key), valStrs.CurValStr);
            }
            else if (SetNameAndValuesDelegate != null)
            {
                SetNameAndValuesDelegate(itemScript, GameEntry.Localization.GetString(key), valStrs.CurValStr, valStrs.NewValStr);
            }
            else
            {
                Log.Warning("Delegate cannot find to set label texts.");
            }

            return index + 1;
        }

        private static ValueStrs GetPhysicalAttack(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.PhysicalAttack > 0)
            {
                ret.CurValStr = data.PhysicalAttack.ToString();
                ret.NewValStr = data.GetPhysicalAttack(newLevel, newStrenghthenLevel).ToString();
            }

            return ret;
        }

        private static ValueStrs GetMagicAttack(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.MagicAttack > 0)
            {
                ret.CurValStr = data.MagicAttack.ToString();
                ret.NewValStr = data.GetMagicAttack(newLevel, newStrenghthenLevel).ToString();
            }

            return ret;
        }

        private static ValueStrs GetPhysicalDefense(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.PhysicalDefense > 0)
            {
                ret.CurValStr = data.PhysicalDefense.ToString();
                ret.NewValStr = data.GetPhysicalDefense(newLevel, newStrenghthenLevel).ToString();
            }

            return ret;
        }

        private static ValueStrs GetMagicDefense(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.MagicDefense > 0)
            {
                ret.CurValStr = data.MagicDefense.ToString();
                ret.NewValStr = data.GetMagicDefense(newLevel, newStrenghthenLevel).ToString();
            }

            return ret;
        }

        private static ValueStrs GetMaxHP(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.MaxHP > 0)
            {
                ret.CurValStr = data.MaxHP.ToString();
                ret.NewValStr = data.GetMaxHP(newLevel, newStrenghthenLevel).ToString();
            }

            return ret;
        }

        private static ValueStrs GetPhysicalAtkHPAbsorbRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.PhysicalAtkHPAbsorbRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.PhysicalAtkHPAbsorbRate);
            }

            return ret;
        }

        private static ValueStrs GetPhysicalAtkReflectRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.PhysicalAtkReflectRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.PhysicalAtkReflectRate);
            }

            return ret;
        }

        private static ValueStrs GetOppPhysicalDfsReduceRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.OppPhysicalDfsReduceRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.OppPhysicalDfsReduceRate);
            }

            return ret;
        }

        private static ValueStrs GetMagicAtkHPAbsorbRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.MagicAtkHPAbsorbRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.MagicAtkHPAbsorbRate);
            }

            return ret;
        }

        private static ValueStrs GetMagicAtkReflectRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.MagicAtkReflectRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.MagicAtkReflectRate);
            }

            return ret;
        }

        private static ValueStrs GetOppMagicDfsReduceRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.OppMagicDfsReduceRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.OppMagicDfsReduceRate);
            }

            return ret;
        }

        private static ValueStrs GetDamageReductionRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.DamageReductionRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.DamageReductionRate);
            }

            return ret;
        }

        private static ValueStrs GetAdditionalDamage(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.AdditionalDamage > 0f)
            {
                ret.CurValStr = ret.NewValStr = data.AdditionalDamage.ToString();
            }

            return ret;
        }

        private static ValueStrs GetCriticalHitRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.CriticalHitRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.CriticalHitRate);
            }

            return ret;
        }

        private static ValueStrs GetCriticalHitProb(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.CriticalHitProb > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.CriticalHitProb);
            }

            return ret;
        }

        private static ValueStrs GetAntiCriticalHitProb(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.AntiCriticalHitProb > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.AntiCriticalHitProb);
            }

            return ret;
        }

        private static ValueStrs GetReducedSkillCoolDownRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.ReducedSkillCoolDownRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.ReducedSkillCoolDownRate);
            }

            return ret;
        }

        private static ValueStrs GetReducedHeroSwitchCDRate(GearData data, int newLevel, int newStrenghthenLevel)
        {
            var ret = new ValueStrs();
            if (data.ReducedHeroSwitchCDRate > 0f)
            {
                ret.CurValStr = ret.NewValStr = GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", data.ReducedHeroSwitchCDRate);
            }

            return ret;
        }

        private class ValueStrs
        {
            public string CurValStr = string.Empty;
            public string NewValStr = string.Empty;
        }
    }
}
