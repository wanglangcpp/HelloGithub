using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class HideManagedEffect : Action
    {
        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Managed effect key in instance logic.")]
        private int m_Key = 0;

        public override TaskStatus OnUpdate()
        {
            GameEntry.SceneLogic.BaseInstanceLogic.HideManagedEffect(m_Key);
            return TaskStatus.Success;
        }
    }
}
