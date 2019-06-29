using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class ClearUILabels : Action
    {
        [SerializeField]
        private SharedGameObjectList[] m_GOLists = null;

        public override TaskStatus OnUpdate()
        {
            if (m_GOLists == null)
            {
                Log.Error("m_GOList is invalid.");
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_GOLists.Length; ++i)
            {
                for (int j = 0; j < m_GOLists[i].Value.Count; ++j)
                {
                    var go = m_GOLists[i].Value[j];
                    if (go == null)
                    {
                        Log.Warning("Game object at index '{0}' of m_GOLists[{1}] is invalid.", j.ToString(), i.ToString());
                        return TaskStatus.Failure;
                    }

                    var label = go.GetComponent<UILabel>();
                    if (label == null)
                    {
                        Log.Warning("Label at index '{0}' of m_GOLists[{1}] is invalid.", j.ToString(), i.ToString());
                        return TaskStatus.Failure;
                    }

                    label.text = string.Empty;
                }
            }

            return TaskStatus.Success;
        }
    }
}
