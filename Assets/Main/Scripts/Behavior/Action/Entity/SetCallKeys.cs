using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SetCallKeys : Action
    {
        [SerializeField]
        private string[] m_CallKeys = null;

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.SinglePlayer)
            {
                GameFramework.Log.Warning("Not in a SinglePlayer instance.");
                return TaskStatus.Failure;
            }

            NpcCharacter self = Owner.GetComponent<NpcCharacter>();
            if (self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the NpcCharacter component!");
                return TaskStatus.Failure;
            }

            self.CallForTargetKeys = m_CallKeys;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_CallKeys = null;
        }
    }
}
