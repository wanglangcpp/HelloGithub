using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HideAirWall : Action
    {
        [SerializeField]
        private int m_AirWallId = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HideAirWall(m_AirWallId))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_AirWallId = 0;
        }
    }
}
