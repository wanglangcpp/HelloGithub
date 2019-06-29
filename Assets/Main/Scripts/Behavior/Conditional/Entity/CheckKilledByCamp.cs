using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckKilledByCamp : Conditional
    {
        private NpcCharacter m_Self = null;
        private CharacterMotion m_SelfMotion = null;

        [SerializeField]
        private int m_CampType = 0;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
            m_SelfMotion = m_Self.Motion;
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

            var targetOwerData = AIUtility.GetFinalOwnerData(m_SelfMotion.DeadlyImpactSourceEntity.Data);
            var targetOwer = targetOwerData.Entity as NpcCharacter;
            if (targetOwer.Camp == (CampType)m_CampType)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}