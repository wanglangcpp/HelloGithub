using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class EnableUIButton : Action
    {
        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedInt m_GOIndex = 0;

        [SerializeField]
        private SharedBool m_Enabled = false;

        public override TaskStatus OnUpdate()
        {
            var goList = m_GOList.Value;
            var index = m_GOIndex.Value;

            if (index >= goList.Count)
            {
                Log.Warning("Index '{0}' out of range of game object list '{1}'.", index, m_GOList.Name);
                return TaskStatus.Failure;
            }

            var go = goList[index];
            if (go == null)
            {
                Log.Warning("Game object at index '{0}' is invalid in game object list '{1}'.", index.ToString(), m_GOList.Name);
                return TaskStatus.Failure;
            }

            var button = go.GetComponent<UIButton>();
            if (button == null)
            {
                Log.Warning("Button at index '{0}' is invalid in game object list '{1}'.", index.ToString(), m_GOList.Name);
                return TaskStatus.Failure;
            }

            button.isEnabled = m_Enabled.Value;
            return TaskStatus.Success;
        }
    }
}
