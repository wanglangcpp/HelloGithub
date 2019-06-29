using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckTargetCamp : Conditional
    {
        private NpcCharacter m_Self = null;

        [SerializeField]
        private int m_CampType = 0;

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

            if (!(m_Self as ICanHaveTarget).HasTarget)
            {
                return TaskStatus.Failure;
            }

            var target = m_Self.Target as TargetableObject;

            if (target.Data.Camp == (CampType)m_CampType)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}