using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    public class ActivateGameObjects : Action
    {
        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedInt m_BegIndex = 0;

        [SerializeField]
        private SharedInt m_EndIndex = 0;

        [SerializeField]
        private SharedBool m_All = false;

        [SerializeField]
        private SharedBool m_Active = false;

        public override TaskStatus OnUpdate()
        {
            var beg = m_All.Value ? 0 : m_BegIndex.Value;
            var end = m_All.Value ? m_GOList.Value.Count : m_EndIndex.Value;

            for (int i = beg; i < end; ++i)
            {
                if (i < 0 || i >= m_GOList.Value.Count)
                {
                    Log.Warning("Index '{1}' out of range in game object list '{0}'.", m_GOList.Name, i.ToString());
                    return TaskStatus.Failure;
                }

                var go = m_GOList.Value[i];
                if (go == null)
                {
                    Log.Warning("Game object at index '{0}' is invalid in game object list '{1}.", i.ToString(), m_GOList.Name);
                    return TaskStatus.Failure;
                }

                go.SetActive(m_Active.Value);
            }

            return TaskStatus.Success;
        }
    }
}
