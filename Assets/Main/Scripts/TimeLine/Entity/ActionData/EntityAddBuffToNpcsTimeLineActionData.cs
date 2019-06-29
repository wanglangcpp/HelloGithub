using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityAddBuffToNpcsTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityAddBuffToNpcsTimeLineAction";
            }
        }

        private int[] m_BuffIds = null;

        private int[] m_NpcIndices = null;

        private int m_CountToAffect = 0;

        private bool m_AvoidAffectedNpcs = false;

        /// <summary>
        /// Buff 编号集合。
        /// </summary>
        public int[] BuffIds
        {
            get
            {
                return m_BuffIds;
            }
        }

        /// <summary>
        /// NPC 索引集合。
        /// </summary>
        public int[] NpcIndices
        {
            get
            {
                return m_NpcIndices;
            }
        }

        /// <summary>
        /// 选择 NPC 的最大个数。
        /// </summary>
        public int CountToAffect
        {
            get
            {
                return m_CountToAffect;
            }
        }

        /// <summary>
        /// 是否避免在当前技能周期内给已经加过 Buff 的 NPC 再次加 Buff。
        /// </summary>
        public bool AvoidAffectedNpcs
        {
            get
            {
                return m_AvoidAffectedNpcs;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_BuffIds = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_NpcIndices = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_CountToAffect = int.Parse(timeLineActionArgs[index++]);
            m_AvoidAffectedNpcs = bool.Parse(timeLineActionArgs[index++]);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetStringFromArray(BuffIds));
            ret.Add(ConverterEx.GetStringFromArray(NpcIndices));
            ret.Add(ConverterEx.GetString(CountToAffect));
            ret.Add(ConverterEx.GetString(AvoidAffectedNpcs));
            return ret.ToArray();
        }
    }
}
