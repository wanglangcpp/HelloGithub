using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    public class ActivateGameObject : Action
    {
        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedInt m_Index = 0;

        [SerializeField]
        private SharedInt m_IndexDelta = 0;

        [SerializeField]
        private SharedBool m_Active = false;

        public override TaskStatus OnUpdate()
        {
            int index = m_Index.Value + m_IndexDelta.Value;

            if (index < 0 || index >= m_GOList.Value.Count)
            {
                Log.Warning("Index '{1}' out of range in game object list '{0}'.", m_GOList.Name, index.ToString());
                return TaskStatus.Failure;
            }

            var go = m_GOList.Value[index];
            if (go == null)
            {
                Log.Warning("Game object at index '{0}' is invalid in game object list '{1}.", index.ToString(), m_GOList.Name);
                return TaskStatus.Failure;
            }

            go.SetActive(m_Active.Value);
            return TaskStatus.Success;
        }
    }
}
