using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AddRegionToObserveRectangle : Action
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private Vector2 m_Center = Vector2.zero;

        [SerializeField]
        private float m_Rotation = 0f;

        [SerializeField]
        private float m_Width = 0f;

        [SerializeField]
        private float m_Height = 0f;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.AddRegionToObserve(m_Id, m_Center, m_Rotation, m_Width, m_Height) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
