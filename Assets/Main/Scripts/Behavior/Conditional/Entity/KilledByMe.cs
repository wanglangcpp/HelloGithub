using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    [TaskDescription("Used right after an NPC is dead.")]
    public class KilledByMe : Conditional
    {
        private NpcCharacter m_Self = null;
        private CharacterMotion m_SelfMotion = null;

        [SerializeField]
        private bool m_CheckCountForPlayerKill = true;

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

            if (!m_CheckCountForPlayerKill)
            {
                return TaskStatus.Failure;
            }

            if (m_SelfMotion.DeadlyImpactSourceEntity == null)
            {
                return TaskStatus.Failure;
            }

            var meHeroes = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.GetHeroes();

            for (int i = 0; i < meHeroes.Length; ++i)
            {
                var heroData = meHeroes[i];
                if (m_SelfMotion.DeadlyImpactSourceEntity.Id == heroData.Id)
                {
                    if (m_Self.Data.CountForPlayerKill)
                    {
                        return TaskStatus.Success;
                    }
                }
            }

            return TaskStatus.Failure;
        }
    }
}
