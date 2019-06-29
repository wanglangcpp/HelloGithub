using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HideBulletRebounder : Action
    {
        [SerializeField]
        private int m_BulletRebounderId = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HideBulletRebounder(m_BulletRebounderId))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_BulletRebounderId = 0;
        }
    }
}
