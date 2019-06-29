using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能徽章表基类。
    /// </summary>
    public abstract class DRBaseSkillBadge : IDataRow
    {
        /// <summary>
        /// 徽章编号。
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// 排序参数。
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// 徽章等级。
        /// </summary>
        public int Level { get; protected set; }

        /// <summary>
        /// 进阶徽章编号。
        /// </summary>
        public int LevelUpBadgeId { get; protected set; }

        /// <summary>
        /// 进阶所需数量。
        /// </summary>
        public int LevelUpCount { get; protected set; }

        /// <summary>
        /// 进阶所需金币。
        /// </summary>
        public int LevelUpCoin { get; protected set; }

        /// <summary>
        /// 徽章特效描述。
        /// </summary>
        public string EffectDesc { get; protected set; }

        public abstract void ParseDataRow(string dataRowText);
    }
}
