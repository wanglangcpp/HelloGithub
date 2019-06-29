using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ShowManagedEffect : Action
    {
        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Managed key to bind in instance logic.")]
        private int m_Key = 0;

        [SerializeField]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("ID in Effect data table.")]
        private int m_EffectId = 0;

        [SerializeField]
        private Vector2 m_Position = Vector2.zero;

        [SerializeField]
        private float m_Rotation = 0f;

        public override TaskStatus OnUpdate()
        {
            int entityId = GameEntry.SceneLogic.BaseInstanceLogic.ShowManagedEffect(m_Key, m_EffectId, m_Position, m_Rotation);
            return entityId == 0 ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
