using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 附加技能配置表。
    /// </summary>
    public class DRAltSkill : IDataRow
    {
        /// <summary>
        /// 附加技能编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 附加技能组编号。
        /// </summary>
        public int[] AltSkillGroupId
        {
            get;
            private set;
        }

        /// <summary>
        /// 附加技能组编号。
        /// </summary>
        /// <param name="index">技能索引。</param>
        /// <returns>附加技能组编号。</returns>
        public int GetAltSkillGroupId(int index)
        {
            return AltSkillGroupId[index];
        }

        /// <summary>
        /// 附加技能组是否可用。
        /// </summary>
        public bool[] AltSkillGroupEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// 附加技能组是否可用。
        /// </summary>
        /// <param name="index">技能索引。</param>
        /// <returns>附加技能组是否可用。</returns>
        public bool GetAltSkillGroupEnabled(int index)
        {
            return AltSkillGroupEnabled[index];
        }

        /// <summary>
        /// 附加技能是否播放界面特效。
        /// </summary>
        public bool[] AltSkillEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// 附加技能是否播放界面特效。
        /// </summary>
        /// <param name="index">技能索引。</param>
        /// <returns>附加技能是否播放界面特效。</returns>
        public bool GetAltSkillEffect(int index)
        {
            return AltSkillEffect[index];
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            AltSkillGroupId = new int[Constant.SkillGroupCount];
            AltSkillGroupEnabled = new bool[Constant.SkillGroupCount];
            AltSkillEffect = new bool[Constant.SkillGroupCount];
            for (int i = 0; i < Constant.SkillGroupCount; i++)
            {
                AltSkillGroupId[i] = int.Parse(text[index++]);
                AltSkillGroupEnabled[i] = bool.Parse(text[index++]);
                AltSkillEffect[i] = bool.Parse(text[index++]);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAltSkill>();
        }
    }
}
