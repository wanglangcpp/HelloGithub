using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityRushTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityRushTimeLineAction";
            }
        }

        private float m_SpeedFactor;
        private bool m_AcceptDirInput;
        private bool m_ForbidRotate;
        private bool m_MoveOnlyOnDirInput;
        private float m_AngularSpeed;

        /// <summary>
        /// 移动速率的倍率。
        /// </summary>
        public float SpeedFactor
        {
            get
            {
                return m_SpeedFactor;
            }
        }

        /// <summary>
        /// 是否接受方向输入。
        /// </summary>
        public bool AcceptDirInput
        {
            get
            {
                return m_AcceptDirInput;
            }
        }

        /// <summary>
        /// 角速度的绝对值。小于等于零时认为不限制角速度。
        /// </summary>
        public float AngularSpeed
        {
            get
            {
                return m_AngularSpeed;
            }
        }

        /// <summary>
        /// 是否禁止角色转动。仅在 <see cref="AcceptDirInput"/> 为真时有效。为真时 <see cref="AngularSpeed"/> 将被忽略。
        /// </summary>
        public bool ForbidRotate
        {
            get
            {
                return m_ForbidRotate;
            }
        }

        /// <summary>
        /// 是否仅在有方向输入时移动。仅在 <see cref="AcceptDirInput"/> 为真时有效。
        /// </summary>
        public bool MoveOnlyOnDirInput
        {
            get
            {
                return m_MoveOnlyOnDirInput;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_SpeedFactor = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_AcceptDirInput = ConverterEx.ParseBool(timeLineActionArgs[index++]).Value;
            m_AngularSpeed = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_ForbidRotate = ConverterEx.ParseBool(timeLineActionArgs[index++]).Value;
            m_MoveOnlyOnDirInput = ConverterEx.ParseBool(timeLineActionArgs[index++]).Value;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(SpeedFactor));
            ret.Add(ConverterEx.GetString(AcceptDirInput));
            ret.Add(ConverterEx.GetString(AngularSpeed));
            ret.Add(ConverterEx.GetString(ForbidRotate));
            ret.Add(ConverterEx.GetString(MoveOnlyOnDirInput));
            return ret.ToArray();
        }
    }
}
