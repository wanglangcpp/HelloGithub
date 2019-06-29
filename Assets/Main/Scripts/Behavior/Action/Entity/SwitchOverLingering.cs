using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SwitchOverLingering : Action
    {
        [SerializeField]
        private float m_MaxTime = float.MaxValue;

        [SerializeField]
        private float m_MinTime = 0f;

        [SerializeField]
        private float m_DurationTime = 0f;

        [SerializeField]
        private float m_SpeedFactor = 1f;

        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The max angle allowed betwween the forward of self and the vector from self to target.")]
        private float m_AngleScope = 90f;

        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The max distance allowed betwween self and target.")]
        private float m_MaxDistance = 25f;

        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Whether the owner should face its target before starting lingering.")]
        private bool m_FaceTarget = true;

        protected Character m_Self = null;
        protected CharacterMotion m_SelfMotion = null;
        protected ICanHaveTarget m_CanHaveTargetSelf = null;

        private Vector3 m_TargetPosition = Vector3.zero;
        private float m_StartTime = -1f;

        private const float LingeringDirectionFactor = 0.7f;//正向概率70%，反向30%
        private const float FlatCorner = 180.0f;

        public override void OnStart()
        {
            base.OnStart();

            m_Self = GetComponent<Character>();
            m_SelfMotion = GetComponent<CharacterMotion>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
            if (m_MaxTime != float.MaxValue && m_MinTime != float.MinValue)
            {
                m_DurationTime = Random.Range(m_MinTime, m_MaxTime);
            }

            CreateLingeringPosition();
            m_StartTime = -1f;
        }

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (((m_MaxTime == float.MaxValue || m_MinTime <= 0f) && m_DurationTime == 0) || m_SpeedFactor == 0)
            {
                return TaskStatus.Failure;
            }

            if (!m_CanHaveTargetSelf.HasTarget || AIUtility.GetAttackDistance(m_Self, m_CanHaveTargetSelf.Target) > m_MaxDistance)
            {
                return TaskStatus.Failure;
            }

            var targetEntity = m_CanHaveTargetSelf.Target as Entity;
            if (m_FaceTarget)
            {
                m_Self.CachedTransform.LookAt2D(targetEntity.CachedTransform.position.ToVector2());
            }
            else if (AIUtility.GetFaceAngle(m_Self, targetEntity) > m_AngleScope)
            {
                return TaskStatus.Failure;
            }

            if (m_StartTime < 0f)
            {
                m_StartTime = Time.time;
                Log.Info("[SwitchOverLingering OnUpdate] Start lingering at time {0}.", m_StartTime.ToString());
                m_Self.Motion.StartLingering(new CharacterMotion.CharacterLingeringParams(m_TargetPosition, m_SpeedFactor, m_MaxDistance, m_AngleScope));
            }

            if (!m_Self.Motion.IsLingering)
            {
                Log.Info("[SwitchOverLingering OnUpdate] Lingering stops at time {0}.", Time.time.ToString());
                return TaskStatus.Success;
            }

            if (Time.time - m_StartTime > m_DurationTime)
            {
                Log.Info("[SwitchOverLingering OnUpdate] Stop lingering at time {0}.", Time.time.ToString());
                m_Self.Motion.StopLingering();
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        private void CreateLingeringPosition()
        {
            int dir = Random.value > .5f ? 1 : -1;
            float radius = m_DurationTime * m_SpeedFactor * m_Self.NavAgent.speed;
            m_TargetPosition = m_Self.CachedTransform.position + dir * radius * m_Self.CachedTransform.right;
        }
    }
}
