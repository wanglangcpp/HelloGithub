using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityChargeSkillTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityChargeSkillTimeLineAction";
            }
        }

        private float m_JumpSkillActionTime;
        /// <summary>
        /// 需要跳转到对应技能的时间线节点的时间
        /// </summary>
        public float JumpSkillActionTime
        {
            get
            {
                return m_JumpSkillActionTime;
            }
        }

        private bool m_JumpImmediatelyOnRelease = true;
        /// <summary>
        /// 是否当手指离开按钮的时候，即可就跳转到相应的技能。默认即刻跳转
        /// True:马上就跳转过去
        /// False:走完Duration长的时间再跳转
        /// </summary>
        public bool JumpImmediatelyOnRelease
        {
            get
            {
                return m_JumpImmediatelyOnRelease;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 1;
            m_JumpSkillActionTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_JumpImmediatelyOnRelease = ConverterEx.ParseBool(timeLineActionArgs[index++]) ?? true;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(m_JumpSkillActionTime));
            ret.Add(ConverterEx.GetString(m_JumpImmediatelyOnRelease));
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            return ret.ToArray();
        }
    }
}