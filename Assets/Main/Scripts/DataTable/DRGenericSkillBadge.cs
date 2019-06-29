using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能通用徽章表。
    /// </summary>
    public class DRGenericSkillBadge : DRBaseSkillBadge
    {
        private const int ParamCount = 10;

        /// <summary>
        /// 是否闪避技能徽章。
        /// </summary>
        public bool IsDodge { get; private set; }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 颜色编号。
        /// </summary>
        public int ColorId { get; private set; }

        /// <summary>
        /// 元素编号。
        /// </summary>
        public int ElementId { get; private set; }

        /// <summary>
        /// 物攻提升百分比。
        /// </summary>
        public float PhysicalAtkIncreaseRate { get; private set; }

        /// <summary>
        /// 法攻提升百分比。
        /// </summary>
        public float MagicAtkIncreaseRate { get; private set; }

        /// <summary>
        /// 暴击率提升数值。
        /// </summary>
        public float CriticalHitProb { get; private set; }

        /// <summary>
        /// 暴击伤害提升数值。
        /// </summary>
        public float CriticalHitRate { get; private set; }

        /// <summary>
        /// 物理攻击防御减免附加值。
        /// </summary>
        public float OppPhysicalDfsReduceRate { get; private set; }

        /// <summary>
        /// 技能冷却减少值。
        /// </summary>
        public float ReducedSkillCoolDown { get; private set; }

        /// <summary>
        /// （其他）参数。
        /// </summary>
        public float[] Params { get; private set; }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            IsDodge = bool.Parse(text[index++]);
            Order = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            ColorId = int.Parse(text[index++]);
            ElementId = int.Parse(text[index++]);
            LevelUpBadgeId = int.Parse(text[index++]);
            LevelUpCount = int.Parse(text[index++]);
            LevelUpCoin = int.Parse(text[index++]);
            PhysicalAtkIncreaseRate = float.Parse(text[index++]);
            MagicAtkIncreaseRate = float.Parse(text[index++]);
            CriticalHitProb = float.Parse(text[index++]);
            CriticalHitRate = float.Parse(text[index++]);
            OppPhysicalDfsReduceRate = float.Parse(text[index++]);
            ReducedSkillCoolDown = float.Parse(text[index++]);

            Params = new float[ParamCount];
            for (int i = 0; i < ParamCount; ++i)
            {
                Params[i] = float.Parse(text[index++]);
            }

            EffectDesc = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRGenericSkillBadge>();
        }
    }
}
