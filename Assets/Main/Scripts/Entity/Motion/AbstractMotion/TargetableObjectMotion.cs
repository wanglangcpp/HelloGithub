using GameFramework;

namespace Genesis.GameClient
{
    public abstract class TargetableObjectMotion : EntityMotion
    {
        public event GameFrameworkAction<int, int, bool, bool> OnPerformSkillStart;

        public event GameFrameworkAction<int, SkillEndReasonType> OnPerformSkillEnd;

        protected PerformSkillOperation m_NextPerformSkillOperation = null;
        protected PerformSkillOperation m_CurrentPerformSkillOperation = null;

        /// <summary>
        /// 释放技能。
        /// </summary>
        /// <param name="skillId">技能编号。</param>
        /// <param name="skillIndex">技能索引。</param>
        /// <param name="isInCombo">是否为连续技的一段。</param>
        /// <param name="isContinualTap">是否为连续点击技能。</param>
        /// <param name="forcePerform">是否强制释放。</param>
        /// <param name="isCharge">是否是蓄力技能</param>
        /// <param name="performType">触发类型</param>
        /// <returns>请求释放技能的操作。</returns>
        public abstract PerformSkillOperation PerformSkill(int skillId, int skillIndex, bool isInCombo, bool isContinualTap, bool isCharge, bool forcePerform, PerformSkillType performType);

        /// <summary>
        /// 执行血量伤害。
        /// </summary>
        /// <param name="impactSourceEntity">伤害来源实体。</param>
        /// <param name="impactSourceType">伤害来源类型。</param>
        /// <returns>是否成功。</returns>
        public abstract bool PerformHPDamage(Entity impactSourceEntity, ImpactSourceType impactSourceType);

        /// <summary>
        /// 执行死亡操作。
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract bool PerformGoDie();

        protected void SwitchToNewPerformSkillOperation()
        {
            //Log.Info("[TargetableObjectMotion SwitchToNewPerformSkillOperation] current = {0}, next = {1}.",
            //    m_CurrentPerformSkillOperation == null ? "<null>" : m_CurrentPerformSkillOperation.Data.SkillId.ToString(),
            //    m_NextPerformSkillOperation == null ? "<null>" : m_NextPerformSkillOperation.Data.SkillId.ToString());
            m_CurrentPerformSkillOperation = m_NextPerformSkillOperation;
            m_NextPerformSkillOperation = null;
        }

        protected void CallCurrentPerformSkillOperationStart(int skillId, int skillIndex, bool isInCombo, bool isContinualTap)
        {
            SwitchToNewPerformSkillOperation();
            if (OnPerformSkillStart != null)
            {
                OnPerformSkillStart(skillId, skillIndex, isInCombo, isContinualTap);
            }

            m_CurrentPerformSkillOperation.PerformSkillStart();
        }

        protected void CallCurrentPerformSkillOperationEnd(int skillId, SkillEndReasonType reason)
        {
            m_CurrentPerformSkillOperation.PerformSkillEnd();

            if (OnPerformSkillEnd != null)
            {
                OnPerformSkillEnd(skillId, reason);
            }
        }

        protected void CallNextPerformSkillOperationFailure()
        {
            m_NextPerformSkillOperation.PerformSkillFailure();
            m_NextPerformSkillOperation = null;
        }

        protected void BreakCurrentTimeLine(ITimeLineInstance<Entity> timeLineInstance, bool instant, int skillId, SkillEndReasonType reason)
        {
            if (timeLineInstance == null || !timeLineInstance.IsActive)
            {
                return;
            }

            bool shouldCallCurrentPerformSkillOperationEnd = !timeLineInstance.IsBroken;
            if (instant)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.TimeLine.DestroyTimeLineInstance(timeLineInstance);
                }
            }
            else
            {
                if (!timeLineInstance.IsBroken)
                {
                    timeLineInstance.Break();
                }
            }

            if (shouldCallCurrentPerformSkillOperationEnd)
            {
                CallCurrentPerformSkillOperationEnd(skillId, reason);
            }
        }
    }
}
