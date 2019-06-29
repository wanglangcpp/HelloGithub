using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySummonNpcTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntitySummonNpcTimeLineAction";
            }
        }

        private int[] m_NpcIndices = null;
        private TransformType? m_TransformType;
        private float? m_RandomRadius = 0f;
        private bool m_AttackOwnerTarget = false;
        private bool m_DieWithOwner = false;
        private int m_RetryCount = 10;

        /// <summary>
        /// 召唤 NPC 的编号。
        /// </summary>
        public int[] NpcIndices
        {
            get
            {
                return m_NpcIndices == null ? new int[0] : m_NpcIndices;
            }
        }

        /// <summary>
        /// 坐标类型。
        /// </summary>
        public TransformType TransformType
        {
            get
            {
                return m_TransformType ?? TransformType.Default;
            }
        }

        /// <summary>
        /// 随机半径。
        /// </summary>
        public float? RandomRadius
        {
            get
            {
                return m_RandomRadius;
            }
        }

        /// <summary>
        /// 是否集成召唤者的目标。
        /// </summary>
        public bool AttackOwnerTarget
        {
            get
            {
                return m_AttackOwnerTarget;
            }
        }

        /// <summary>
        /// 是否随召唤者一同死亡。
        /// </summary>
        public bool DieWithOwner
        {
            get
            {
                return m_DieWithOwner;
            }
        }

        /// <summary>
        /// 重试次数。
        /// </summary>
        /// <remarks>给 NPC 的出生位置增加随机性可能使 NPC 落在当前连同范围之外，因此可能需要重试。</remarks>
        public int RetryCount
        {
            get
            {
                return m_RetryCount;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_NpcIndices = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_TransformType = ConverterEx.ParseEnum<TransformType>(timeLineActionArgs[index++]);
            m_RandomRadius = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_AttackOwnerTarget = bool.Parse(timeLineActionArgs[index++]);
            m_DieWithOwner = bool.Parse(timeLineActionArgs[index++]);

            if (index < timeLineActionArgs.Length)
            {
                m_RetryCount = int.Parse(timeLineActionArgs[index]);
            }

            index++;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetStringFromArray(NpcIndices));
            ret.Add(ConverterEx.GetString(TransformType));
            ret.Add(ConverterEx.GetString(RandomRadius));
            ret.Add(ConverterEx.GetString(AttackOwnerTarget));
            ret.Add(ConverterEx.GetString(DieWithOwner));
            ret.Add(ConverterEx.GetString(RetryCount));
            return ret.ToArray();
        }
    }
}
