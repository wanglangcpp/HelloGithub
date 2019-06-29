using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AirWallPlayAnimation : Action
    {
        [SerializeField]
        private int m_AirWallId = 0;

        [SerializeField]
        private string m_AnimationName = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.AirWallPlayAnimation(m_AirWallId, m_AnimationName))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_AirWallId = 0;
            m_AnimationName = null;
        }
    }
}
