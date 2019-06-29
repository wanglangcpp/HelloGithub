using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能组配置表。
    /// </summary>

    public class DRSkillGroup : IDataRow
    {
        public const int GenericSkillBadgeSlotCount = 4;
        public const int GenericBadgeUnlockContitionIdsCount = GenericSkillBadgeSlotCount;

        /// <summary>
        /// 技能组编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 技能编号。
        /// </summary>
        public int SkillId
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否是连续技的一部分。
        /// </summary>
        public bool IsInCombo
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为连续点击技能。
        /// </summary>
        public bool IsContinualTap
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为蓄力技能。
        /// </summary>
        public bool IsCharge
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能解锁等级。
        /// </summary>
        public int SkillUnlockLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 专属徽章开启条件。
        /// </summary>
        public int SpecificBadgeUnlockConditionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 通用徽章开启条件。
        /// </summary>
        public int[] GenericBadgeUnlockContitionIds
        {
            get;
            private set;
        }

        /// <summary>
        /// 识别元素编号。
        /// </summary>
        public int ElementId
        {
            get;
            private set;
        }

        /// <summary>
        /// 通用徽章位种类。
        /// </summary>
        public int[] GenericSkillBadgeSlotColors
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能升级数值描述。
        /// </summary>
        public string SkillUpgradeValueDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能种类角标。
        /// </summary>
        public int SkillCategory
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能类型描述。
        /// </summary>
        public string SkillType
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
            IsInCombo = bool.Parse(text[index++]);
            IsContinualTap = bool.Parse(text[index++]);
            IsCharge = bool.Parse(text[index++]);
            SkillId = int.Parse(text[index++]);
            SkillUnlockLevel = int.Parse(text[index++]);
            SpecificBadgeUnlockConditionId = int.Parse(text[index++]);
            GenericBadgeUnlockContitionIds = new int[GenericBadgeUnlockContitionIdsCount];
            for (int i = 0; i < GenericBadgeUnlockContitionIdsCount; i++)
            {
                GenericBadgeUnlockContitionIds[i] = int.Parse(text[index++]);
            }
            ElementId = int.Parse(text[index++]);
            GenericSkillBadgeSlotColors = new int[GenericSkillBadgeSlotCount];
            for (int i = 0; i < GenericSkillBadgeSlotCount; i++)
            {
                GenericSkillBadgeSlotColors[i] = int.Parse(text[index++]);
            }
            SkillUpgradeValueDescription = text[index++];
            SkillCategory = int.Parse(text[index++]);
            SkillType = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkillGroup>();
        }
    }
}
