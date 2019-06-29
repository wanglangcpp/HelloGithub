using GameFramework;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BuildingMotion : TargetableObjectMotion
    {
        private IFsm<BuildingMotion> m_Fsm = null;

        public event GameFrameworkAction OnStateChanged;

        public new Building Owner
        {
            get
            {
                return m_Owner as Building;
            }
        }

        public Entity ImpactSourceEntity
        {
            get;
            private set;
        }

        public ImpactSourceType ImpactSourceType
        {
            get;
            private set;
        }

        public Entity DeadlyImpactSourceEntity
        {
            get;
            set;
        }

        public ImpactSourceType DeadlyImpactSourceType
        {
            get;
            private set;
        }

        public int PerformSkillId
        {
            get;
            private set;
        }

        private DRBuildingAnimation m_AnimationDataRow = null;

        private DRBuildingAnimation AnimationDataRow
        {
            get
            {
                return m_AnimationDataRow;
            }
        }

        private DRBuilding m_BuildingDataRow = null;

        private DRBuilding BuildingDataRow
        {
            get
            {
                return m_BuildingDataRow;
            }
        }

        private Building BuildingOwner
        {
            get
            {
                return m_Owner as Building;
            }
        }

        public string CurrentStateName
        {
            get
            {
                return m_Fsm == null ? string.Empty : m_Fsm.CurrentState.GetType().Name;
            }
        }

        public void PlayAliasedAnimation(string animClipAlias)
        {
            (m_Fsm.CurrentState as StateBase).PlayAliasedAnimation(m_Fsm, animClipAlias);
        }

        public override PerformSkillOperation PerformSkill(int skillId, int skillIndex, bool isCombo, bool isContinualTap, bool isCharge, bool forcePerform, PerformSkillType performType)
        {
            if (m_NextPerformSkillOperation != null)
            {
                return null;
            }

            PerformSkillId = skillId;

            PerformSkillOperation op = null;
            op = new PerformSkillOperation(new PerformSkillOperationData
            {
                SkillId = skillId,
                IsInCombo = isCombo,
                IsContinualTap = isContinualTap,
                ForcePerform = forcePerform,
            }, Owner.Id);

            m_NextPerformSkillOperation = op;
            (m_Fsm.CurrentState as StateBase).PerformSkill(m_Fsm);
            return op;
        }

        public override bool PerformHPDamage(Entity impactSourceEntity, ImpactSourceType impactSourceType)
        {
            ImpactSourceEntity = impactSourceEntity;
            ImpactSourceType = impactSourceType;
            return (m_Fsm.CurrentState as StateBase).PerformHPDamage(m_Fsm);
        }

        public override bool PerformGoDie()
        {
            return (m_Fsm.CurrentState as StateBase).PerformGoDie(m_Fsm);
        }

        protected override void Awake()
        {
            base.Awake();

            var dtBuilding = GameEntry.DataTable.GetDataTable<DRBuilding>();
            m_BuildingDataRow = dtBuilding.GetDataRow(Owner.Data.BuildingTypeId);
            if (m_BuildingDataRow == null)
            {
                Log.Error("Building ID '{0}' not found.", Owner.Data.BuildingTypeId);
                return;
            }

            var dtAnim = GameEntry.DataTable.GetDataTable<DRBuildingAnimation>();
            m_AnimationDataRow = dtAnim.GetDataRow(Owner.Data.BuildingModelId);

            m_Fsm = GameEntry.Fsm.CreateFsm(string.Format("Entity{0}", Owner.Id.ToString()), this,
                new EnteringState(),
                new DefaultState(),
                new DeadState());
            m_Fsm.Start<EnteringState>();
        }

        public void Shutdown()
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
        }
    }
}
