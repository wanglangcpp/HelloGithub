using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备配置表。
    /// </summary>
    public class DRGear : IDataRow
    {
        /// <summary>
        /// 装备编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 装备名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 装备描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 装备分类。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 装备品质。
        /// </summary>
        public int Quality
        {
            get;
            private set;
        }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用等级下限。
        /// </summary>
        public int MinLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 卖店价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否广播。
        /// </summary>
        public bool Broadcast
        {
            get;
            private set;
        }

        /// <summary>
        /// 强化所需道具编号。
        /// </summary>
        public int StrengthenItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大生命值。
        /// </summary>
        public int MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大生命值等级系数。
        /// </summary>
        public float LCMaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大生命值加成。
        /// </summary>
        public float MaxHPIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大生命值加成等级系数。
        /// </summary>
        public float SLCMaxHPIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理攻击力。
        /// </summary>
        public int PhysicalAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理攻击力等级系数。
        /// </summary>
        public float LCPhysicalAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理防御力。
        /// </summary>
        public int PhysicalDefense
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理防御力等级系数。
        /// </summary>
        public float LCPhysicalDefense
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理攻击力加成。
        /// </summary>
        public float PhysicalAtkIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理攻击力加成等级系数。
        /// </summary>
        public float SLCPhysicalAtkIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理防御力加成。
        /// </summary>
        public float PhysicalDfsIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理防御力加成等级系数。
        /// </summary>
        public float SLCPhysicalDfsIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理伤害吸血率。
        /// </summary>
        public float PhysicalAtkHPAbsorbRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 物理伤害反击率。
        /// </summary>
        public float PhysicalAtkReflectRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 降低对方物理防御百分比。
        /// </summary>
        public float OppPhysicalDfsReduceRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术攻击力。
        /// </summary>
        public int MagicAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术攻击力等级系数。
        /// </summary>
        public float LCMagicAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术防御力。
        /// </summary>
        public int MagicDefense
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术防御力等级系数。
        /// </summary>
        public float LCMagicDefense
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术攻击力加成。
        /// </summary>
        public float MagicAtkIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术攻击力加成等级系数。
        /// </summary>
        public float SLCMagicAtkIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术防御力加成。
        /// </summary>
        public float MagicDfsIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术防御力加成等级系数。
        /// </summary>
        public float SLCMagicDfsIncreaseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术伤害吸血率。
        /// </summary>
        public float MagicAtkHPAbsorbRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 法术伤害反击率。
        /// </summary>
        public float MagicAtkReflectRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 降低对方法术防御百分比。
        /// </summary>
        public float OppMagicDfsReduceRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 受击伤害减免率。
        /// </summary>
        public float DamageReductionRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 附加伤害。
        /// </summary>
        public int AdditionalDamage
        {
            get;
            private set;
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 暴击率。
        /// </summary>
        public float CriticalHitProb
        {
            get;
            private set;
        }

        /// <summary>
        /// 免除暴击率。
        /// </summary>
        public float AntiCriticalHitProb
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能冷却缩减百分比。
        /// </summary>
        public float ReducedSkillCoolDownRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 待战英雄冷却缩减百分比。
        /// </summary>
        public float ReducedHeroSwitchCDRate
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Type = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            MinLevel = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            Broadcast = bool.Parse(text[index++]);
            StrengthenItemId = int.Parse(text[index++]);

            MaxHP = int.Parse(text[index++]);
            LCMaxHP = float.Parse(text[index++]);

            SLCMaxHPIncreaseRate = float.Parse(text[index++]);

            PhysicalAttack = int.Parse(text[index++]);
            LCPhysicalAttack = float.Parse(text[index++]);

            PhysicalDefense = int.Parse(text[index++]);
            LCPhysicalDefense = float.Parse(text[index++]);

            SLCPhysicalAtkIncreaseRate = float.Parse(text[index++]);

            SLCPhysicalDfsIncreaseRate = float.Parse(text[index++]);

            PhysicalAtkHPAbsorbRate = float.Parse(text[index++]);
            PhysicalAtkReflectRate = float.Parse(text[index++]);
            OppPhysicalDfsReduceRate = float.Parse(text[index++]);

            MagicAttack = int.Parse(text[index++]);
            LCMagicAttack = float.Parse(text[index++]);

            MagicDefense = int.Parse(text[index++]);
            LCMagicDefense = float.Parse(text[index++]);

            SLCMagicAtkIncreaseRate = float.Parse(text[index++]);

            SLCMagicDfsIncreaseRate = float.Parse(text[index++]);

            MagicAtkHPAbsorbRate = float.Parse(text[index++]);
            MagicAtkReflectRate = float.Parse(text[index++]);
            OppMagicDfsReduceRate = float.Parse(text[index++]);
            DamageReductionRate = float.Parse(text[index++]);
            AdditionalDamage = int.Parse(text[index++]);
            CriticalHitRate = float.Parse(text[index++]);
            CriticalHitProb = float.Parse(text[index++]);
            AntiCriticalHitProb = float.Parse(text[index++]);
            ReducedSkillCoolDownRate = float.Parse(text[index++]);
            ReducedHeroSwitchCDRate = index < text.Length ? float.Parse(text[index++]) : 0f;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRGear>();
        }
    }
}
