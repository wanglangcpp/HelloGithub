using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    public class CacheUIData : Action
    {
        [SerializeField]
        private BasicDataType m_DataType = BasicDataType.Int32;

        [SerializeField]
        private string m_KeyFormat = string.Empty;

        [SerializeField]
        private SharedInt[] m_KeyParams = null;

        [SerializeField]
        private SharedBool m_TargetBool = null;

        [SerializeField]
        private SharedInt m_TargetInt = null;

        [SerializeField]
        private SharedFloat m_TargetFloat = null;

        [SerializeField]
        private SharedString m_TargetString = null;

        private IUpdatableUIFragment m_Owner = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
        }

        public override TaskStatus OnUpdate()
        {
            string key = m_KeyFormat;
            if (m_KeyParams != null && m_KeyParams.Length > 0)
            {
                var parameters = new object[m_KeyParams.Length];
                for (int i = 0; i < m_KeyParams.Length; ++i)
                {
                    parameters[i] = m_KeyParams[i].Value.ToString();
                }

                key = string.Format(m_KeyFormat, parameters);
            }

            string valStr;
            if (!m_Owner.TryGetData(key, out valStr))
            {
                return TaskStatus.Failure;
            }

            switch (m_DataType)
            {
                case BasicDataType.Boolean:
                    bool valBool;
                    if (m_TargetBool == null || !bool.TryParse(valStr, out valBool))
                    {
                        return TaskStatus.Failure;
                    }

                    m_TargetBool.Value = valBool;
                    return TaskStatus.Success;

                case BasicDataType.Float:
                    float valFloat;
                    if (m_TargetFloat == null || !float.TryParse(valStr, out valFloat))
                    {
                        return TaskStatus.Failure;
                    }

                    m_TargetFloat.Value = valFloat;
                    return TaskStatus.Success;

                case BasicDataType.Int32:
                    int valInt;
                    if (m_TargetInt == null || !int.TryParse(valStr, out valInt))
                    {
                        return TaskStatus.Failure;
                    }

                    m_TargetInt.Value = valInt;
                    return TaskStatus.Success;

                case BasicDataType.String:
                default:
                    if (m_TargetString == null)
                    {
                        return TaskStatus.Failure;
                    }

                    m_TargetString.Value = valStr ?? string.Empty;
                    return TaskStatus.Success;
            }
        }
    }
}
