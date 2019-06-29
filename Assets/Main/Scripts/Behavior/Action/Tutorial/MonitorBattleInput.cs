using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class MonitorBattleInput : Action
    {
        public enum Category
        {
            Input,
            NoInputForAWhile,
        }

        [SerializeField]
        private Category m_Category = Category.Input;

        [SerializeField]
        private float m_TimeOutForNoInput = 5f;

        private bool m_Success = false;
        private float m_StartTime = 0f;

        public override void OnStart()
        {
            m_Success = false;
            GameEntry.Input.OnPlayerTryingToMove += OnPlayerTryingToMove;
            GameEntry.Input.OnPlayerTryingToPerformSkill += OnPlayerTryingToPerformSkill;
            m_StartTime = Time.unscaledTime;
        }

        public override void OnEnd()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Input.OnPlayerTryingToMove -= OnPlayerTryingToMove;
                GameEntry.Input.OnPlayerTryingToPerformSkill -= OnPlayerTryingToPerformSkill;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Category == Category.NoInputForAWhile)
            {
                if (Time.unscaledTime - m_StartTime >= m_TimeOutForNoInput)
                {
                    m_Success = true;
                }
            }

            return m_Success ? TaskStatus.Success : TaskStatus.Running;
        }

        private void OnPlayerTryingToPerformSkill(int skillIndex, PerformSkillType performType)
        {
            OnInput();
        }

        private void OnPlayerTryingToMove(float x, float y)
        {
            OnInput();
        }

        private void OnInput()
        {
            if (m_Category == Category.Input)
            {
                m_Success = true;
            }
            else if (m_Category == Category.NoInputForAWhile)
            {
                m_StartTime = Time.unscaledTime;
            }
        }
    }
}
