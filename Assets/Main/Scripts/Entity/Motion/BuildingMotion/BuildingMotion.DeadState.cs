using GameFramework;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BuildingMotion
    {
        private class DeadState : StateBase
        {
            private float m_KeepTime = 0f;

            protected override string AnimClipAlias
            {
                get
                {
                    return "Dying";
                }
            }

            protected override void OnEnter(IFsm<BuildingMotion> fsm)
            {
                fsm.Owner.BuildingOwner.EnableNavMeshObstacles(false);
                fsm.Owner.Owner.Data.IsDead = true;
                base.OnEnter(fsm);
                fsm.Owner.Owner.ImpactCollider.enabled = false;
                fsm.Owner.Owner.ClearBuffs();
                fsm.Owner.DeadlyImpactSourceEntity = fsm.Owner.ImpactSourceEntity;
                fsm.Owner.DeadlyImpactSourceType = fsm.Owner.ImpactSourceType;

                var dataTable = GameEntry.DataTable.GetDataTable<DRBuilding>();

                DRBuilding dataRow = dataTable.GetDataRow(fsm.Owner.Owner.Data.BuildingTypeId);
                if (dataRow == null)
                {
                    Log.Error("Building '{0}' not found.", fsm.Owner.Owner.Data.BuildingTypeId);
                    return;
                }

                m_KeepTime = dataRow.KeepTime;

                bool hasDyingAnimClip = !m_AnimInfo.IsEmpty;
                if (hasDyingAnimClip)
                {
                    PlayAnimation(fsm, m_AnimInfo);
                }

                var animationDataRow = GetAnimationDataRow(fsm);

                var deadAnimInfo = new AnimInfo();
                if (InitAnims(fsm, animationDataRow, deadAnimInfo, "Dead"))
                {
                    if (hasDyingAnimClip)
                    {
                        QueueAnimation(fsm, deadAnimInfo);
                    }
                    else
                    {
                        PlayAnimation(fsm, deadAnimInfo);
                    }
                }

                GameEntry.Event.FireNow(this, new BuildingDeadEventArgs(fsm.Owner.Owner, fsm.Owner.ImpactSourceEntity));
            }

            protected override void OnUpdate(IFsm<BuildingMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime >= m_KeepTime)
                {
                    GameEntry.Entity.HideEntity(fsm.Owner.Owner.Entity);
                    return;
                }
            }
        }
    }
}
