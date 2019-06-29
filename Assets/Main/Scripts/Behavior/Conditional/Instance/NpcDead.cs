using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class NpcDead : Conditional
    {
        [SerializeField]
        private int[] m_NpcIndices = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_NpcIndices.Length; i++)
            {
                if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsNpcDead(m_NpcIndices[i]))
                {
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Success;
        }
    }
}
