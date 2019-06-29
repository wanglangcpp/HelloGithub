using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class BindUILabel : Action
    {
        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedInt m_GOIndex = 0;

        [SerializeField]
        private SharedBool m_NeedReplacement = false;

        [SerializeField]
        private SharedString m_TextKey = null;

        [SerializeField]
        private BasicDataType[] m_TextParamDateTypes = null;

        [SerializeField]
        private SharedGenericVariable[] m_TextParams = null;

        private IUpdatableUIFragment m_Owner = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                return TaskStatus.Failure;
            }

            if (m_TextParamDateTypes == null || m_TextParams == null || m_TextParams.Length != m_TextParamDateTypes.Length)
            {
                return TaskStatus.Failure;
            }

            if (m_GOIndex.Value >= m_GOList.Value.Count)
            {
                return TaskStatus.Failure;
            }

            var go = m_GOList.Value[m_GOIndex.Value];
            if (go == null)
            {
                return TaskStatus.Failure;
            }

            var label = go.GetComponent<UILabel>();
            if (label == null)
            {
                return TaskStatus.Failure;
            }

            object[] objParams = new object[m_TextParams.Length];
            for (int i = 0; i < objParams.Length; ++i)
            {
                objParams[i] = m_TextParams[i].Value.value.GetValue();
            }

            var text = GameEntry.Localization.GetString(m_TextKey.Value, objParams);

            if (m_NeedReplacement.Value)
            {
                text = GameEntry.StringReplacement.GetString(text);
            }

            label.text = text;
            return TaskStatus.Success;
        }
    }
}
