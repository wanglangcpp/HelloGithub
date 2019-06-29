using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckSelfHasBuff : Conditional
    {
        [SerializeField]
        private int m_BuffId = 0;

        private Entity m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the Entity component!");
                return TaskStatus.Failure;
            }

            var target = m_Self as TargetableObject;

            if (target == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the TargetableObject!");
                return TaskStatus.Failure;
            }
            var buff = target.Data.BuffPool.GetById(m_BuffId);

            return buff != null ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
