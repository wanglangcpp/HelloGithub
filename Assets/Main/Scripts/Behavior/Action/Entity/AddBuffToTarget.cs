using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AddBuffToTarget : Action
    {
        [SerializeField]
        private int[] m_BuffIds = null;

        private TargetableObject m_Self = null;
        private ICanHaveTarget m_CanHaveTargetSelf = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
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

            if (m_CanHaveTargetSelf == null)
            {
                GameFramework.Log.Warning("Self is not valid.");
                return TaskStatus.Failure;
            }

            var targetableObj = m_CanHaveTargetSelf.Target as TargetableObject;
            if (!m_CanHaveTargetSelf.HasTarget || targetableObj == null)
            {
                GameFramework.Log.Warning("Target is invalid.");
            }

            for (int i = 0; i < m_BuffIds.Length; ++i)
            {
                targetableObj.AddBuff(m_BuffIds[i], m_Self.Data, OfflineBuffPool.GetNextSerialId(), null);
            }

            return TaskStatus.Success;
        }
    }
}
