using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AddRegionToObserve : Action
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private Vector2 m_Center = Vector2.zero;

        [SerializeField]
        private float m_Radius = 0f;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.AddRegionToObserve(m_Id, m_Center, m_Radius) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
