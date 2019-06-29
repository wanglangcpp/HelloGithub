using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class TargetAngle : Conditional
    {
        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The max angle allowed betwween the forward of self and the vector from self to target.")]
        private float m_AngleScope = 90f;

        private TargetableObject m_Self = null;
        private ICanHaveTarget m_CanHaveTargetSelf = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
            m_AngleScope = Mathf.Clamp(m_AngleScope, 0f, 90f);
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null || m_CanHaveTargetSelf == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (!m_CanHaveTargetSelf.HasTarget)
            {
                return TaskStatus.Failure;
            }

            bool success = AIUtility.GetFaceAngle(m_Self, m_CanHaveTargetSelf.Target as Entity) <= m_AngleScope;

            return success ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
