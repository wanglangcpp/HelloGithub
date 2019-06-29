using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public enum SkillCategory
    {
        Undefined = 0,

        /// <summary>
        /// 连续点击技。
        /// </summary>
        ContinualTapSkill = 1,

        /// <summary>
        /// 蓄力技。
        /// </summary>
        ChargeSkill = 2,

        /// <summary>
        /// 切换技。
        /// </summary>
        SwitchSkill = 3,
    }
}