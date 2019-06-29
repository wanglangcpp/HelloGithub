using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AddBuffToKilledMeTarget : Action
    {
        [SerializeField]
        private int[] m_BuffIds = null;

        private TargetableObject m_Self = null;
        private CharacterMotion m_SelfMotion = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            m_SelfMotion = (m_Self as NpcCharacter).Motion;
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

            if (m_Self == null || m_SelfMotion == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the NpcCharacter/CharacterMotion component!");
                return TaskStatus.Failure;
            }

            if (m_SelfMotion.DeadlyImpactSourceEntity == null)
            {
                return TaskStatus.Failure;
            }

            if (m_Self.IsDead)
            {
                var targetOwerData = AIUtility.GetFinalOwnerData(m_SelfMotion.DeadlyImpactSourceEntity.Data);
                var targetOwer = targetOwerData.Entity as TargetableObject;
                for (int i = 0; i < m_BuffIds.Length; i++)
                {
                    targetOwer.AddBuff(m_BuffIds[i], m_Self.Data, OfflineBuffPool.GetNextSerialId(), null);
                }
            }

            return TaskStatus.Success;
        }
    }
}