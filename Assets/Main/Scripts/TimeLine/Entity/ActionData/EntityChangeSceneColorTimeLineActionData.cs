using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityChangeSceneColorTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityChangeSceneColorTimeLineAction";
            }
        }

        private Color m_TargetColor = Color.white;

        public Color TargetColor
        {
            get
            {
                return m_TargetColor;
            }
        }

        private float m_Attack;

        public float Attack
        {
            get
            {
                return Mathf.Max(m_Attack, 0f);
            }
        }

        private float m_Sustain;

        public float Sustain
        {
            get
            {
                return Mathf.Max(m_Sustain, 0f);
            }
        }

        private float m_Release;

        public float Release
        {
            get
            {
                return Mathf.Max(m_Release, 0f);
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            UnityEngine.ColorUtility.TryParseHtmlString(timeLineActionArgs[index++], out m_TargetColor);
            m_Attack = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_Sustain = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_Release = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add("#" + UnityEngine.ColorUtility.ToHtmlStringRGBA(TargetColor));
            ret.Add(ConverterEx.GetString(Attack));
            ret.Add(ConverterEx.GetString(Sustain));
            ret.Add(ConverterEx.GetString(Release));
            return ret.ToArray();
        }
    }
}
