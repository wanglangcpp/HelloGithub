using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    [TaskDescription("Check whether the normal skill of the current hero is cooling down.")]
    public class SkillAtIndexInCD : Conditional
    {
        [SerializeField]
        private int m_Index = 1;

        private HeroCharacter m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<HeroCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            return m_Self.Data.GetSkillCoolDownTime(m_Index).IsReady ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
