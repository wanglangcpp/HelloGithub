using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class SetIntInstanceParam : Action
    {
        [SerializeField]
        protected string m_Key = string.Empty;

        [SerializeField]
        protected int m_Value = 0;

        [SerializeField]
        protected bool m_Incremental = false;

        public override TaskStatus OnUpdate()
        {
            var instanceData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data;

            if (!instanceData.HasMiscParam(m_Key))
            {
                return TaskStatus.Failure;
            }

            if (m_Incremental)
            {
                int oldValue = instanceData.GetMiscParam<int>(m_Key);
                instanceData.SetMiscParam<int>(m_Key, oldValue + m_Value);
            }
            else
            {
                instanceData.SetMiscParam<int>(m_Key, m_Value);
            }

            return TaskStatus.Success;
        }
    }
}
