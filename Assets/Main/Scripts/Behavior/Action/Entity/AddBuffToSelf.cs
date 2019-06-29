using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AddBuffToSelf : Action
    {
        [SerializeField]
        private int[] m_BuffIds = null;

        private TargetableObject m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            if (m_BuffIds == null)
            {
                m_BuffIds = new int[0];
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_BuffIds.Length; ++i)
            {
                m_Self.AddBuff(m_BuffIds[i], m_Self.Data, OfflineBuffPool.GetNextSerialId(), null);
            }

            return TaskStatus.Success;
        }
    }
}
