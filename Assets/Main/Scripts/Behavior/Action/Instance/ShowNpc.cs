using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowNpc : Action
    {
        [SerializeField]
        private int[] m_NpcIndices = null;

        [SerializeField]
        private bool m_ForceInstant = false;

        private int m_CurrentIndex = 0;

        public override void OnStart()
        {
            m_CurrentIndex = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (IsFinished)
            {
                return TaskStatus.Success;
            }

            do
            {
                var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic;
                int npcIndex = m_NpcIndices[m_CurrentIndex];
                if (instanceLogic.IsNpcForbidden(npcIndex) || !instanceLogic.ShowNpc(npcIndex))
                {
                    return TaskStatus.Failure;
                }

                m_CurrentIndex++;
            }
            while (m_ForceInstant && !IsFinished);

            return IsFinished ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            m_NpcIndices = null;
            m_CurrentIndex = 0;
        }

        private bool IsFinished
        {
            get
            {
                return m_CurrentIndex >= m_NpcIndices.Length;
            }
        }
    }
}
