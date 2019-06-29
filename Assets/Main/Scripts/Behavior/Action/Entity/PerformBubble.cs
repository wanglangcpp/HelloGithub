using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    internal class PerformBubble : Action
    {
        [SerializeField]
        private float m_StartTime = 0;

        [SerializeField]
        private float m_KeepTime = 0;

        [SerializeField]
        private string[] m_Contents = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var self = Owner.GetComponent<TargetableObject>();

            if (self == null)
            {
                return TaskStatus.Failure;
            }

            int count = m_Contents.Length;
            int index = Random.Range(0, count);

            GameEntry.Impact.CreateBubble(self, m_Contents[index], m_StartTime, m_KeepTime);

            return TaskStatus.Success;
        }
    }
}
