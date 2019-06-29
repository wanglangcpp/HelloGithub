using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity"), SkipErrorCheck]
    public partial class PerformSkill : Action
    {
        [SerializeField]
        private SharedInt m_SkillId = 0;

        [SerializeField]
        private bool m_FaceTarget = false;

        private Entity m_Self = null;
        private StrategyBase m_Strategy = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
            CreateStrategy();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Strategy == null)
            {
                return TaskStatus.Failure;
            }

            return m_Strategy.PerformSkill();
        }

        public override void OnReset()
        {
            m_SkillId = 0;
            m_Strategy.Reset();
        }

        private void CreateStrategy()
        {
            var character = m_Self as Character;
            if (character != null)
            {
                m_Strategy = new StrategyCharacter(this, character);
            }

            var bullet = m_Self as Bullet;
            if (bullet != null)
            {
                m_Strategy = new StrategyBullet(this, bullet);
            }

            var building = m_Self as Building;
            if (building != null)
            {
                m_Strategy = new StrategyBuilding(this, building);
            }
        }
    }
}
