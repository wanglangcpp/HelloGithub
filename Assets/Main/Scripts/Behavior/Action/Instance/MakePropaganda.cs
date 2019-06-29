using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class MakePropaganda : Action
    {
        [SerializeField]
        private InstancePropagandaData m_PropagandaData = null;

        public override TaskStatus OnUpdate()
        {
            if (m_PropagandaData == null)
            {
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.AddPropaganda(m_PropagandaData);
            return TaskStatus.Success;
        }
    }
}
