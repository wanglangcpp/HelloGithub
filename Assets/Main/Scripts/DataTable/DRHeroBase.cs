using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄基础属性配置表。
    /// </summary>
    public class DRHeroBase : IDataRow
    {
        /// <summary>
        /// 英雄等级编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 升级所需经验。
        /// </summary>
        public int LevelUpExp
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础生命值。
        /// </summary>
        public int MaxHPBase
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础物理攻击。
        /// </summary>
        public int PhysicalAttackBase
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础物理防御。
        /// </summary>
        public int PhysicalDefenseBase
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础法术攻击。
        /// </summary>
        public int MagicAttackBase
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础法术防御。
        /// </summary>
        public int MagicDefenseBase
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
            LevelUpExp = int.Parse(text[index++]);
            MaxHPBase = int.Parse(text[index++]);
            PhysicalAttackBase = int.Parse(text[index++]);
            PhysicalDefenseBase = int.Parse(text[index++]);
            MagicAttackBase = int.Parse(text[index++]);
            MagicDefenseBase = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRHeroBase>();
        }
    }
}
