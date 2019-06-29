using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityAttackAssistTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityAttackAssistTimeLineAction";
            }
        }

        private AttackAssistSelectionMode m_SelectionMode;
        private float? m_ValidDistance;
        private float m_TargetDistance;
        private float m_SpeedFactor;
        private float m_NoAssistDistance;
        private float? m_ContactCheckAllowance;
        private float m_ContactCheckAngle = 90f;
        private int m_SectorCount = 1;

        /// <summary>
        /// 选取目标的最大距离。
        /// </summary>
        public float? ValidDistance
        {
            get
            {
                return m_ValidDistance;
            }
        }

        /// <summary>
        /// 目标选择方式。
        /// </summary>
        public AttackAssistSelectionMode SelectionMode
        {
            get
            {
                return m_SelectionMode;
            }
        }

        /// <summary>
        /// 距离辅助的目标距离。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumDistance"/> 模式生效。
        /// </summary>
        public float TargetDistance
        {
            get
            {
                return m_TargetDistance;
            }
        }

        /// <summary>
        /// 距离辅助的速度倍率。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumDistance"/> 模式生效。
        /// </summary>
        public float SpeedFactor
        {
            get
            {
                return m_SpeedFactor;
            }
        }

        /// <summary>
        /// 进行距离辅助的最小距离。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumDistance"/> 模式生效。
        /// </summary>
        public float NoAssistDistance
        {
            get
            {
                return m_NoAssistDistance;
            }
        }

        /// <summary>
        /// 接触检测裕量。空则不检测接触。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumDistance"/> 模式生效。
        /// </summary>
        public float? ContactCheckAllowance
        {
            get
            {
                return m_ContactCheckAllowance;
            }
        }

        /// <summary>
        /// 接触检测角度。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumDistance"/> 模式生效。
        /// </summary>
        public float ContactCheckAngle
        {
            get
            {
                return m_ContactCheckAngle;
            }
        }

        /// <summary>
        /// 扇形数量。仅在 <see cref="Genesis.GameClient.AttackAssistSelectionMode.MinimumAngleDiffAdvanced"/> 模式生效。
        /// </summary>
        public int SectorCount
        {
            get
            {
                return m_SectorCount > 0 ? m_SectorCount : 1;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_ValidDistance = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_SelectionMode = ConverterEx.ParseEnum<AttackAssistSelectionMode>(timeLineActionArgs[index++]).Value;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_TargetDistance = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_SpeedFactor = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_NoAssistDistance = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_ContactCheckAllowance = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_ContactCheckAngle = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_SectorCount = ConverterEx.ParseInt(timeLineActionArgs[index++]).Value;
            if (m_SectorCount < 1) m_SectorCount = 1;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(ValidDistance));
            ret.Add(ConverterEx.GetString(SelectionMode));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(TargetDistance));
            ret.Add(ConverterEx.GetString(SpeedFactor));
            ret.Add(ConverterEx.GetString(NoAssistDistance));
            ret.Add(ConverterEx.GetString(ContactCheckAllowance));
            ret.Add(ConverterEx.GetString(ContactCheckAngle));
            ret.Add(ConverterEx.GetString(SectorCount));
            return ret.ToArray();
        }
    }
}
