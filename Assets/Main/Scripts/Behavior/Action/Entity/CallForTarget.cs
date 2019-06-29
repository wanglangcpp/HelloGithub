using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CallForTarget : Action
    {
        [SerializeField]
        private string[] m_CallKeys = null;

        [SerializeField]
        private float m_Radius = 1f;

        private NpcCharacter m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.SinglePlayer)
            {
                GameFramework.Log.Warning("Not in a SinglePlayer instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the NpcCharacter component!");
                return TaskStatus.Failure;
            }

            if (!m_Self.HasTarget)
            {
                GameFramework.Log.Warning("Npc do not have a target, can not use call for target.");
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.CallForTarget(m_Self, m_CallKeys, m_Radius, m_Self.Target);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_CallKeys = null;
            m_Radius = 1f;
        }
    }
}
