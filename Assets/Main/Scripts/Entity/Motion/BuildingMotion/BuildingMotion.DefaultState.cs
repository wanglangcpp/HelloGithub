using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BuildingMotion
    {
        private class DefaultState : StateBase
        {
            private ITimeLineInstance<Entity> m_TimeLineInstance = null;

            protected override string AnimClipAlias
            {
                get
                {
                    return "Default";
                }
            }

            protected override void OnEnter(IFsm<BuildingMotion> fsm)
            {
                PlayDefaultAnim(fsm);
                fsm.Owner.BuildingOwner.EnableNavMeshObstacles(true);
                base.OnEnter(fsm);
            }

            protected override void OnLeave(IFsm<BuildingMotion> fsm, bool isShutdown)
            {
                BreakCurrentTimeLine(fsm, true, SkillEndReasonType.Other);
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<BuildingMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_TimeLineInstance != null && !m_TimeLineInstance.IsActive)
                {
                    fsm.Owner.CallCurrentPerformSkillOperationEnd(m_TimeLineInstance.Id, SkillEndReasonType.Finish);
                    m_TimeLineInstance = null;
                    PlayDefaultAnim(fsm);
                }
            }

            public override void PerformSkill(IFsm<BuildingMotion> fsm)
            {
                BreakCurrentTimeLine(fsm, false, SkillEndReasonType.BreakBySkill);
                InternalPerformSkill(fsm);
            }

            public override bool PerformHPDamage(IFsm<BuildingMotion> fsm)
            {
                if (fsm.Owner.Owner.Data.HP <= 0)
                {
                    fsm.Owner.Owner.TryGoDie();
                }

                return true;
            }

            private bool InternalPerformSkill(IFsm<BuildingMotion> fsm)
            {
                var skillId = fsm.Owner.PerformSkillId;
                m_TimeLineInstance = GameEntry.TimeLine.Entity.CreateTimeLineInstance(fsm.Owner.Owner, skillId, new Dictionary<string, object>());
                if (m_TimeLineInstance == null)
                {
                    Log.Warning("Can not create entity time line instance '{0}'.", skillId.ToString());
                    fsm.Owner.CallNextPerformSkillOperationFailure();
                    return false;
                }

                fsm.Owner.CallCurrentPerformSkillOperationStart(skillId, -1, false, false);
                return true;
            }

            private void PlayDefaultAnim(IFsm<BuildingMotion> fsm)
            {
                PlayAnimation(fsm, m_AnimInfo);
            }

            private void BreakCurrentTimeLine(IFsm<BuildingMotion> fsm, bool instant, SkillEndReasonType reason)
            {
                int skillId = m_TimeLineInstance == null ? 0 : m_TimeLineInstance.Id;
                fsm.Owner.BreakCurrentTimeLine(m_TimeLineInstance, instant, skillId, reason);
            }
        }
    }
}
