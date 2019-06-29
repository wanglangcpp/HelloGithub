using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AddPatrolPoints : Action
    {
        [SerializeField]
        private Vector2[] m_PatrolPoints = null;

        private NpcCharacter m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the NpcCharacter component!");
                return TaskStatus.Failure;
            }

            m_Self.AddPatrolPoints(m_PatrolPoints);
            return TaskStatus.Success;
        }
    }
}
