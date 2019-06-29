using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class EntityColliderTriggerImpactTimeLineActionDataAbstract : TimeLineActionData
    {
        public enum Strategy
        {
            Normal,
            LightningChain,
        }

        public class BuffConditionalImpactGroup
        {
            public BuffType? RequiredBuffType { get; set; }
            public int[] ImpactIds { get; set; }
        }

        private BuffConditionalImpactGroup[] m_BuffConditionalImpactGroups = null;

        public BuffConditionalImpactGroup[] BuffConditionalImpactGroups
        {
            get
            {
                return m_BuffConditionalImpactGroups;
            }
        }

        protected Vector3? m_Offset;
        protected int[] m_ImpactIds = new int[] { };
        protected bool m_AffectFriendly;
        protected bool m_AffectNeutral;
        protected bool m_AffectHostile;
        protected bool m_AcceptRepeatedImpact;
        protected float? m_RepeatedImpactIntervalTime;
        protected string m_HitEffectResourceName;
        protected int m_HitEffectAttachPointId;
        protected int[] m_HitSoundIds;
        protected bool m_ShouldBroadcastSound;
        protected float m_HitSoundMinInterval;
        protected int[] m_BuffIds;
        protected string[] m_BuffConditionalImpactIds;
        protected int m_ColorChangeId;
        protected float m_ColorChangeDuration;
        protected string m_LineEffectResourceName;
        protected string m_LineEffectMySideAttachPoint;
        protected Strategy m_Strategy;
        protected int m_ChainIntervalCount;
        protected float m_ChainTransferRadius;

        /// <summary>
        /// 伤害盒偏移。
        /// </summary>
        public Vector3? Offset
        {
            get
            {
                return m_Offset;
            }
        }

        /// <summary>
        /// 伤害列表。
        /// </summary>
        public int[] ImpactIds
        {
            get
            {
                return m_ImpactIds;
            }
        }

        /// <summary>
        /// 是否伤害友好实体。
        /// </summary>
        public bool AffectFriendly
        {
            get
            {
                return m_AffectFriendly;
            }
        }

        /// <summary>
        /// 是否伤害中立实体。
        /// </summary>
        public bool AffectNeutral
        {
            get
            {
                return m_AffectNeutral;
            }
        }

        /// <summary>
        /// 是否伤害敌方实体。
        /// </summary>
        public bool AffectHostile
        {
            get
            {
                return m_AffectHostile;
            }
        }

        /// <summary>
        /// 是否接受重复伤害。
        /// </summary>
        public bool AcceptRepeatedImpact
        {
            get
            {
                return m_AcceptRepeatedImpact;
            }
        }

        /// <summary>
        /// 重复伤害的时间间隔。
        /// </summary>
        public float? RepeatedImpactIntervalTime
        {
            get
            {
                return m_RepeatedImpactIntervalTime;
            }
        }

        /// <summary>
        /// 受击特效资源名称。
        /// </summary>
        public string HitEffectResourceName
        {
            get
            {
                return m_HitEffectResourceName;
            }
        }

        /// <summary>
        /// 受击特效挂点。
        /// </summary>
        public int HitEffectAttachPointId
        {
            get
            {
                return m_HitEffectAttachPointId;
            }
        }

        /// <summary>
        /// 受击音效编号列表。
        /// </summary>
        public int[] HitSoundIds
        {
            get
            {
                return m_HitSoundIds;
            }
        }

        /// <summary>
        /// 是否广播音效。
        /// </summary>
        public bool ShouldBroadcastSound
        {
            get
            {
                return m_ShouldBroadcastSound;
            }
        }

        /// <summary>
        /// 受击音效最小间隔时间。
        /// </summary>
        public float HitSoundMinInterval
        {
            get
            {
                return m_HitSoundMinInterval;
            }
        }

        /// <summary>
        /// Buff 编号列表。
        /// </summary>
        public int[] BuffIds
        {
            get
            {
                return m_BuffIds;
            }
        }

        /// <summary>
        /// 以 Buff 为条件的伤害编号。
        /// </summary>
        public string[] BuffConditionalImpactIds
        {
            get
            {
                return m_BuffConditionalImpactIds;
            }
        }

        /// <summary>
        /// 变色编号。
        /// </summary>
        public int ColorChangeId
        {
            get
            {
                return m_ColorChangeId;
            }
        }

        /// <summary>
        /// 变色持续时间。
        /// </summary>
        public float ColorChangeDuration
        {
            get
            {
                return m_ColorChangeDuration;
            }
        }

        /// <summary>
        /// 连线特效资源名称。
        /// </summary>
        public string LineEffectResourceName
        {
            get
            {
                return m_LineEffectResourceName;
            }
        }

        /// <summary>
        /// 连线特效在攻击方的挂点路径。
        /// </summary>
        public string LineEffectMySideAttachPoint
        {
            get
            {
                return m_LineEffectMySideAttachPoint;
            }
        }

        /// <summary>
        /// 使用策略。
        /// </summary>
        public Strategy ItsStrategy
        {
            get
            {
                return m_Strategy;
            }
        }

        /// <summary>
        /// 闪电链分割的时间区间数。<see cref="m_Strategy"/> 为 <see cref="Strategy.LightningChain"/> 时有效。
        /// </summary>
        public int ChainIntervalCount
        {
            get
            {
                return m_ChainIntervalCount <= 0 ? 1 : m_ChainIntervalCount;
            }
        }

        /// <summary>
        /// 闪电链传播半径。<see cref="m_Strategy"/> 为 <see cref="Strategy.LightningChain"/> 时有效。
        /// </summary>
        public float ChainTransferRadius
        {
            get
            {
                return m_ChainTransferRadius;
            }
        }

        protected int ParseData(string[] timeLineActionArgs, int index)
        {
            m_ImpactIds = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_AffectFriendly = bool.Parse(timeLineActionArgs[index++]);
            m_AffectNeutral = bool.Parse(timeLineActionArgs[index++]);
            m_AffectHostile = bool.Parse(timeLineActionArgs[index++]);
            m_AcceptRepeatedImpact = bool.Parse(timeLineActionArgs[index++]);
            m_RepeatedImpactIntervalTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_HitEffectResourceName = timeLineActionArgs[index++];
            m_HitEffectAttachPointId = int.Parse(timeLineActionArgs[index++]);
            m_HitSoundIds = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_ShouldBroadcastSound = bool.Parse(timeLineActionArgs[index++]);
            m_HitSoundMinInterval = float.Parse(timeLineActionArgs[index++]);
            m_BuffIds = ConverterEx.ParseIntArray(timeLineActionArgs[index++]);
            m_BuffConditionalImpactIds = ConverterEx.ParseStringArray(timeLineActionArgs[index++]);
            ParseBuffConditionalImpactGroups(m_BuffConditionalImpactIds);
            m_ColorChangeId = ConverterEx.ParseInt(timeLineActionArgs[index++]).Value;
            m_ColorChangeDuration = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
            m_LineEffectResourceName = timeLineActionArgs[index++];
            m_LineEffectMySideAttachPoint = timeLineActionArgs[index++];
            m_Strategy = timeLineActionArgs.Length > index ? ConverterEx.ParseEnum<Strategy>(timeLineActionArgs[index++]).Value : Strategy.Normal;
            m_ChainIntervalCount = timeLineActionArgs.Length > index ? ConverterEx.ParseInt(timeLineActionArgs[index++]).Value : 1;
            m_ChainTransferRadius = timeLineActionArgs.Length > index ? ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value : 0f;
            return index;
        }

        protected void SerializeData(List<string> ret)
        {
            ret.Add(ConverterEx.GetStringFromArray(ImpactIds));
            ret.Add(ConverterEx.GetString(AffectFriendly));
            ret.Add(ConverterEx.GetString(AffectNeutral));
            ret.Add(ConverterEx.GetString(AffectHostile));
            ret.Add(ConverterEx.GetString(AcceptRepeatedImpact));
            ret.Add(ConverterEx.GetString(RepeatedImpactIntervalTime));
            ret.Add(HitEffectResourceName);
            ret.Add(ConverterEx.GetString(HitEffectAttachPointId));
            ret.Add(ConverterEx.GetStringFromArray(HitSoundIds));
            ret.Add(ConverterEx.GetString(ShouldBroadcastSound));
            ret.Add(ConverterEx.GetString(HitSoundMinInterval));
            ret.Add(ConverterEx.GetStringFromArray(BuffIds));
            ret.Add(ConverterEx.GetStringFromArray(BuffConditionalImpactIds));
            ret.Add(ConverterEx.GetString(ColorChangeId));
            ret.Add(ConverterEx.GetString(ColorChangeDuration));
            ret.Add(LineEffectResourceName);
            ret.Add(LineEffectMySideAttachPoint);
            ret.Add(ConverterEx.GetString(ItsStrategy));
            ret.Add(ConverterEx.GetString(ChainIntervalCount));
            ret.Add(ConverterEx.GetString(ChainTransferRadius));
        }

        private void ParseBuffConditionalImpactGroups(string[] stringArrayToParse)
        {
            var groups = new List<BuffConditionalImpactGroup>();

            for (int i = 0; i < stringArrayToParse.Length; ++i)
            {
                var line = stringArrayToParse[i];

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var group = new BuffConditionalImpactGroup();
                var splittedBySemicolon = line.Split(':');

                string[] idStrs;
                if (splittedBySemicolon.Length == 1)
                {
                    group.RequiredBuffType = null;
                    idStrs = splittedBySemicolon[0].Split('#');
                }
                else
                {
                    group.RequiredBuffType = (BuffType)(int.Parse(splittedBySemicolon[0]));
                    idStrs = splittedBySemicolon[1].Split('#');
                }

                group.ImpactIds = new int[idStrs.Length];

                for (int j = 0; j < idStrs.Length; ++j)
                {
                    group.ImpactIds[j] = int.Parse(idStrs[j]);
                }

                groups.Add(group);
            }

            m_BuffConditionalImpactGroups = groups.ToArray();
        }
    }
}
