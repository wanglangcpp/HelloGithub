using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityAttackAssistTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityAttackAssistTimeLineActionData m_Data;

        private TargetableObject m_TargetableEnemy = null;
        private Vector2 m_CachedDirection;
        private Vector2 m_OwnerOriginalPosition;
        private Vector2 m_EnemyOriginalPosition;

        private bool m_ShouldPerformDistanceAssist;
        private bool m_Disabled = false;

        private const float DefaultValidDistance = 1000f;

        private GameFrameworkFunc<ITimeLineInstance<Entity>, Character, ITargetable> m_SelectTarget;

        public EntityAttackAssistTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityAttackAssistTimeLineActionData;

            switch (m_Data.SelectionMode)
            {
                case AttackAssistSelectionMode.MinimumAngleDiff:
                    m_SelectTarget = SelectTarget_MinimumAngleDiff;
                    break;
                case AttackAssistSelectionMode.MinimumDistance:
                    m_SelectTarget = SelectTarget_MinimumDistance;
                    break;
                case AttackAssistSelectionMode.MinimumAngleDiffAdvanced:
                default:
                    m_SelectTarget = SelectTarget_MinimumAngleDiffAdvanced;
                    break;
            }
        }

        private float ValidDistance
        {
            get
            {
                return m_Data.ValidDistance ?? DefaultValidDistance;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var characterOwner = timeLineInstance.Owner as Character;
            if (characterOwner == null)
            {
                Log.Warning("'{0}' is not a character.", timeLineInstance.Owner.Name);
                return;
            }

            if (!(characterOwner is HeroCharacter))
            {
                m_Disabled = true;
                return;
            }

            m_TargetableEnemy = m_SelectTarget(timeLineInstance, characterOwner) as TargetableObject;
            if (m_TargetableEnemy != null)
            {
                var transform = timeLineInstance.Owner.CachedTransform;
                m_EnemyOriginalPosition = m_TargetableEnemy.CachedTransform.position.ToVector2();
                transform.LookAt2D(m_EnemyOriginalPosition);
                m_OwnerOriginalPosition = transform.position.ToVector2();
                m_CachedDirection = (m_EnemyOriginalPosition - m_OwnerOriginalPosition).normalized;
            }

            m_ShouldPerformDistanceAssist = !(m_TargetableEnemy is Building) && (Vector2.Distance(m_OwnerOriginalPosition, m_EnemyOriginalPosition) > m_Data.NoAssistDistance);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            m_Disabled |= GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled;
            if (m_Disabled || !m_ShouldPerformDistanceAssist || !AIUtility.TargetCanBeSelected(m_TargetableEnemy))
            {
                FastForwardSelfAndCommonCD(timeLineInstance);
                return;
            }

            var characterOwner = timeLineInstance.Owner as Character;
            var myNavMeshAgent = characterOwner.NavAgent;
            if (m_Data.ContactCheckAllowance != null && GameEntry.SceneLogic.BaseInstanceLogic.ContactChecker.HasContact(myNavMeshAgent, m_Data.ContactCheckAllowance.Value, m_Data.ContactCheckAngle))
            {
                FastForwardSelfAndCommonCD(timeLineInstance);
                return;
            }

            if (Vector2.Distance(characterOwner.CachedTransform.position.ToVector2(), m_EnemyOriginalPosition) < m_Data.TargetDistance)
            {
                FastForwardSelfAndCommonCD(timeLineInstance);
                return;
            }

            myNavMeshAgent.nextPosition = characterOwner.CachedTransform.position + m_CachedDirection.ToVector3() * characterOwner.Data.Speed * m_Data.SpeedFactor * elapseSeconds;
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private ITargetable SelectTarget_MinimumAngleDiff(ITimeLineInstance<Entity> timeLineInstance, Character characterOwner)
        {
            return GetNearestEnemy(timeLineInstance, characterOwner);
        }

        private ITargetable SelectTarget_MinimumDistance(ITimeLineInstance<Entity> timeLineInstance, Character characterOwner)
        {
            return GetNearestEnemy(timeLineInstance, characterOwner);
        }

        private bool CheckTargetAvailability(ITargetable targetableEntity, Character characterOwner)
        {
            if (!AIUtility.TargetCanBeSelected(targetableEntity))
            {
                return false;
            }

            RelationType relation = AIUtility.GetRelation(characterOwner.Camp, targetableEntity.Camp);
            if (relation != RelationType.Hostile)
            {
                return false;
            }

            return true;
        }

        private ITargetable SelectTarget_MinimumAngleDiffAdvanced(ITimeLineInstance<Entity> timeLineInstance, Character characterOwner)
        {
            ITargetable target = null;

            Collider[] hitColliders = GetHitColliders(timeLineInstance);
            float anglePerSector = 360f / m_Data.SectorCount;

            var myTransform = characterOwner.CachedTransform;
            Vector2 myPosition2D = myTransform.position.ToVector2();
            Vector2 myForward = myTransform.forward.ToVector2();

            int currentSectorPriority = int.MaxValue;
            float currentDistance = float.PositiveInfinity;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Entity entity = hitColliders[i].GetComponent<Entity>();
                ITargetable targetableEntity = entity as ITargetable;
                if (!CheckTargetAvailability(targetableEntity, characterOwner))
                {
                    continue;
                }

                float angle = Vector2.Angle(entity.CachedTransform.position.ToVector2() - myPosition2D, myForward);
                int sectorPriority = Mathf.FloorToInt((angle + anglePerSector * .5f) / anglePerSector);
                if (sectorPriority > currentSectorPriority)
                {
                    continue;
                }

                float distance = CalcDistanceBetweenEntities(timeLineInstance.Owner, entity);
                if (sectorPriority < currentSectorPriority)
                {
                    currentSectorPriority = sectorPriority;
                    currentDistance = distance;
                    target = targetableEntity;
                    continue;
                }

                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    target = targetableEntity;
                    continue;
                }
            }

            return target;
        }

        private Collider[] GetHitColliders(ITimeLineInstance<Entity> timeLineInstance)
        {
            return Physics.OverlapSphere(timeLineInstance.Owner.CachedTransform.position, ValidDistance, LayerMask.GetMask(Constant.Layer.TargetableObjectLayerName));
        }

        private ITargetable GetNearestEnemy(ITimeLineInstance<Entity> timeLineInstance, Character characterOwner)
        {
            ITargetable nearestEnemy = null;
            float nearestEnemyAngleOrDistance = float.MaxValue;

            Collider[] hitColliders = GetHitColliders(timeLineInstance);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Entity entity = hitColliders[i].GetComponent<Entity>();
                ITargetable targetableEntity = entity as ITargetable;
                if (!CheckTargetAvailability(targetableEntity, characterOwner))
                {
                    continue;
                }

                var angleOrDistance = CalcAngleOrDistance(entity, timeLineInstance);
                if (angleOrDistance >= nearestEnemyAngleOrDistance)
                {
                    continue;
                }

                nearestEnemy = targetableEntity;
                nearestEnemyAngleOrDistance = angleOrDistance;
            }

            return nearestEnemy;
        }

        private float CalcAngleOrDistance(Entity entity, ITimeLineInstance<Entity> timeLineInstance)
        {
            switch (m_Data.SelectionMode)
            {
                case AttackAssistSelectionMode.MinimumAngleDiff:
                    var direction = entity.CachedTransform.position - timeLineInstance.Owner.CachedTransform.position;
                    return Vector3.Angle(direction, timeLineInstance.Owner.CachedTransform.forward);
                case AttackAssistSelectionMode.MinimumDistance:
                    return CalcDistanceBetweenEntities(timeLineInstance.Owner, entity);
                default:
                    return DefaultValidDistance;
            }
        }

        private static float CalcDistanceBetweenEntities(Entity a, Entity b)
        {
            if (a == null || b == null)
            {
                return float.PositiveInfinity;
            }

            return Vector2.Distance(a.CachedTransform.position.ToVector2(), b.CachedTransform.position.ToVector2());
        }
    }
}
