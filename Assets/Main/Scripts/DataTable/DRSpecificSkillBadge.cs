using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能专属徽章表。
    /// </summary>
    public class DRSpecificSkillBadge : DRBaseSkillBadge
    {
        /// <summary>
        /// 对应的技能组编号。
        /// </summary>
        public int OriginalSkillGroupId { get; private set; }

        /// <summary>
        /// 替代为的技能组编号。
        /// </summary>
        public int ReplaceSkillGroupId { get; private set; }

        /// 伤害提升百分比。
        /// </summary>
        public float DamageIncreaseRate { get; private set; }

        /// <summary>
        /// 是否可以直接使用切换技。
        /// </summary>
        public bool UseSwitchSkillDirectly { get; private set; }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Order = int.Parse(text[index++]);
            OriginalSkillGroupId = int.Parse(text[index++]);
            ReplaceSkillGroupId = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
            LevelUpBadgeId = int.Parse(text[index++]);
            LevelUpCount = int.Parse(text[index++]);
            LevelUpCoin = int.Parse(text[index++]);
            DamageIncreaseRate = float.Parse(text[index++]);
            UseSwitchSkillDirectly = bool.Parse(text[index++]);
            EffectDesc = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSpecificSkillBadge>();
        }
    }
}
