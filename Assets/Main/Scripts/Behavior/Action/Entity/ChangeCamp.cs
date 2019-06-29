using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ChangeCamp : Action
    {
        [SerializeField]
        private CampType m_ToCamp = CampType.Player;

        private NpcCharacter m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.IsAvailable || GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic == null)
            {
                return TaskStatus.Failure;
            }

            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ChangeNpcCamp(m_Self, m_ToCamp) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
