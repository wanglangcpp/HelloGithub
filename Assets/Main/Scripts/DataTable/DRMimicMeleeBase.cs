using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 大乱斗基础数值配置表。
    /// </summary>
    public class DRMimicMeleeBase : IDataRow
    {
        /// <summary>
        /// 英雄等级编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 升级所需经验。
        /// </summary>
        public int LevelUpExp { get; private set; }

        /// <summary>
        /// 基础生命值。
        /// </summary>
        public int MaxHPBase { get; private set; }

        /// <summary>
        /// 基础物理攻击。
        /// </summary>
        public int PhysicalAttackBase { get; private set; }

        /// <summary>
        /// 基础物理防御。
        /// </summary>
        public int PhysicalDefenseBase { get; private set; }

        /// <summary>
        /// 霸体值。
        /// </summary>
        public float Steady { get; private set; }

        /// <summary>
        /// 霸体值恢复速度。
        /// </summary>
        public float SteadyRecoverSpeed { get; private set; }

        /// <summary>
        /// 霸体Buff编号。
        /// </summary>
        public int SteadyBuffId { get; private set; }

        /// <summary>
        /// 伤害浮动比率。
        /// </summary>
        public float DamageRandomRate { get; private set; }

        /// <summary>
        /// 暴击率。
        /// </summary>
        public float CriticalHitProb { get; private set; }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate { get; private set; }

        /// <summary>
        /// 击杀该级别英雄获得经验。
        /// </summary>
        public int MeleeExp { get; private set; }

        /// <summary>
        /// 击杀该级别英雄获得的积分。
        /// </summary>
        public int MeleeScore { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            LevelUpExp = int.Parse(text[index++]);
            MaxHPBase = int.Parse(text[index++]);
            PhysicalAttackBase = int.Parse(text[index++]);
            PhysicalDefenseBase = int.Parse(text[index++]);
            Steady = float.Parse(text[index++]);
            SteadyRecoverSpeed = float.Parse(text[index++]);
            SteadyBuffId = int.Parse(text[index++]);
            DamageRandomRate = float.Parse(text[index++]);
            CriticalHitProb = float.Parse(text[index++]);
            CriticalHitRate = float.Parse(text[index++]);
            MeleeExp = int.Parse(text[index++]);
            MeleeScore = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRMimicMeleeBase>();
        }
    }
}
