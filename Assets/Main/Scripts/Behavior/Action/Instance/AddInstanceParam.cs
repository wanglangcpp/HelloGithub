using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    public abstract class AddInstanceParam<T> : Action
        where T : System.IComparable<T>
    {
        [SerializeField]
        protected string m_Key = string.Empty;

        [SerializeField]
        protected T m_InitValue = default(T);

        public override TaskStatus OnUpdate()
        {
            var instanceData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data;

            if (instanceData.HasMiscParam(m_Key))
            {
                return TaskStatus.Failure;
            }

            instanceData.AddMiscParam<T>(m_Key, m_InitValue);
            return TaskStatus.Success;
        }
    }
}
