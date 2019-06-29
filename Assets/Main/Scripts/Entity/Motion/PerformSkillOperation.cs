using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 请求释放技能的操作。
    /// </summary>
    [Serializable]
    public class PerformSkillOperation
    {
        [SerializeField]
        private readonly PerformSkillOperationData m_Data = null;

        /// <summary>
        /// 获取数据。
        /// </summary>
        public PerformSkillOperationData Data
        {
            get { return m_Data; }
        }

        [SerializeField]
        private PerformSkillOperationState m_State;

        /// <summary>
        /// 获取状态。
        /// </summary>
        public PerformSkillOperationState State
        {
            get { return m_State; }
        }

        public bool IsCharging
        {
            get;
            set;
        }

        public bool IsChargeSkill
        {
            get;
            set;
        }

        public event GameFrameworkAction OnPerformSkillStart;
        public event GameFrameworkAction OnPerformSkillFailure;
        public event GameFrameworkAction OnPerformSkillEnd;

        private int m_PerformerId = 0;

        public PerformSkillOperation(PerformSkillOperationData data, int performerId)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data is invalid.");
            }

            m_Data = data;
            m_PerformerId = performerId;
            m_State = PerformSkillOperationState.Waiting;
        }

        public void PerformSkillStart()
        {
            if (m_State != PerformSkillOperationState.Waiting)
            {
                throw new NotSupportedException(string.Format("Received PerformSkillStart event in '{0}' state, whose skill id is '{1}'.", m_State.ToString(), Data.SkillId.ToString()));
            }

            //if (PerformerIsMyHero) Log.Info("[PerformSkillOperation PerformSkillStart] skill id is '{0}'.", Data.SkillId.ToString());
            m_State = PerformSkillOperationState.Performing;
            if (OnPerformSkillStart != null) OnPerformSkillStart();
        }

        public void PerformSkillEnd()
        {
            if (m_State != PerformSkillOperationState.Performing)
            {
                throw new NotSupportedException(string.Format("Received PerformSkillEnd event in '{0}' state, whose skill id is '{1}'.", m_State.ToString(), Data.SkillId.ToString()));
            }

            //if (PerformerIsMyHero) Log.Info("[PerformSkillOperation PerformSkillEnd] skill id is '{0}'.", Data.SkillId.ToString());
            m_State = PerformSkillOperationState.PerformEnd;
            if (OnPerformSkillEnd != null) OnPerformSkillEnd();
        }

        public void PerformSkillFailure()
        {
            if (m_State != PerformSkillOperationState.Waiting)
            {
                throw new NotSupportedException(string.Format("Received PerformSkillFailure event in '{0}' state, whose skill id is '{1}'.", m_State.ToString(), Data.SkillId.ToString()));
            }

            //if (PerformerIsMyHero) Log.Info("[PerformSkillOperation PerformSkillFailure] skill id is '{0}'.", Data.SkillId.ToString());
            m_State = PerformSkillOperationState.PerformFailure;
            if (OnPerformSkillFailure != null) OnPerformSkillFailure();
        }

        private bool PerformerIsMyHero
        {
            get
            {
                var meHeroCharacter = GameEntry.Entity.GetGameEntity(m_PerformerId) as MeHeroCharacter;
                return meHeroCharacter != null;
            }
        }
    }
}
