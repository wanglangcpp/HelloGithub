using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public abstract class AbstractChasing : Action
    {
        [SerializeField]
        protected float m_RepathInterval = 1f;

        [SerializeField]
        protected float m_TimeOut = 0f;

        protected Character m_Self = null;
        protected CharacterMotion m_SelfMotion = null;
        protected ICanHaveTarget m_CanHaveTargetSelf = null;
        protected bool m_Running = false;
        protected float m_LastRepathTime = 0f;
        protected float m_StartTime = 0f;

        public override void OnStart()
        {
            m_Self = GetComponent<Character>();
            m_SelfMotion = GetComponent<CharacterMotion>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
            m_LastRepathTime = 0f;
            m_StartTime = Time.time;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_CanHaveTargetSelf == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (m_SelfMotion == null)
            {
                GameFramework.Log.Warning("Character motion is invalid.");
                return TaskStatus.Failure;
            }

            if (!m_CanHaveTargetSelf.HasTarget)
            {
                return TaskStatus.Failure;
            }

            if (m_Running)
            {
                if (m_TimeOut > 0f && Time.time - m_StartTime > m_TimeOut)
                {
                    return TaskStatus.Success;
                }

                if (m_RepathInterval >= 0f && Time.time - m_LastRepathTime > m_RepathInterval)
                {
                    return Repath();
                }

                if (m_SelfMotion.Moving)
                {
                    return TaskStatus.Running;
                }
                else
                {
                    m_Running = false;
                    return TaskStatus.Success;
                }
            }

            return Repath();
        }

        public override void OnReset()
        {
            m_Running = false;
        }

        protected abstract TaskStatus Repath();
    }
}
