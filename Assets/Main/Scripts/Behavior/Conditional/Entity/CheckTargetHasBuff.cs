using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckTargetHasBuff : Conditional
    {
        [SerializeField]
        private int m_BuffId = 0;

        private ICanHaveTarget m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>() as ICanHaveTarget;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (!m_Self.HasTarget)
            {
                return TaskStatus.Failure;
            }
            var target = m_Self.Target as TargetableObject;

            var buff = target.Data.BuffPool.GetById(m_BuffId);

            return buff != null ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
