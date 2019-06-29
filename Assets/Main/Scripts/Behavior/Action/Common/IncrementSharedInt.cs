using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    public class IncrementSharedInt : Action
    {
        [SerializeField]
        private SharedInt m_Target = null;

        [SerializeField]
        private SharedInt m_Delta = 1;

        public override TaskStatus OnUpdate()
        {
            m_Target.Value += m_Delta.Value;
            return TaskStatus.Success;
        }
    }
}
