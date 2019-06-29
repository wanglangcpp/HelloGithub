using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityTransformTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityTransformTimeLineAction";
            }
        }

        private Vector2? m_Offset;
        private float? m_ContactCheckAllowance;
        private float m_ContactCheckAngle;
        private bool m_AcceptDirInput = false;

        /// <summary>
        /// 平动偏移量。
        /// </summary>
        public Vector2? Offset
        {
            get
            {
                return m_Offset;
            }
        }

        /// <summary>
        /// 接触检测裕量。
        /// </summary>
        public float? ContactCheckAllowance
        {
            get
            {
                return m_ContactCheckAllowance;
            }
        }

        /// <summary>
        /// 接触检测角度。
        /// </summary>
        public float ContactCheckAngle
        {
            get
            {
                return m_ContactCheckAngle;
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

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Offset = ConverterEx.ParseVector2(timeLineActionArgs[index++]);
            m_ContactCheckAllowance = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_ContactCheckAngle = index < timeLineActionArgs.Length ? ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value : 90f;
            m_AcceptDirInput = index < timeLineActionArgs.Length ? ConverterEx.ParseBool(timeLineActionArgs[index++]).Value : false;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetStringFromVector2(Offset));
            ret.Add(ConverterEx.GetString(ContactCheckAllowance));
            ret.Add(ConverterEx.GetString(ContactCheckAngle));
            ret.Add(ConverterEx.GetString(AcceptDirInput));
            return ret.ToArray();
        }
    }
}
