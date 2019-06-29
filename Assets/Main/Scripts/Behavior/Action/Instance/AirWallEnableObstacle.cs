using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AirWallEnableObstacle : Action
    {
        [SerializeField]
        private int m_AirWallId = 0;

        [SerializeField]
        private bool m_ObstacleEnabled = false;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.AirWallEnableObstacle(m_AirWallId, m_ObstacleEnabled))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_AirWallId = 0;
            m_ObstacleEnabled = false;
        }
    }
}
