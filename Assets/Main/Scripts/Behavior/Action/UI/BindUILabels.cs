using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class BindUILabels : Action
    {
        private enum Mode
        {
            /// <summary>
            /// 默认。
            /// </summary>
            Default,

            /// <summary>
            /// 以索引作为参数。
            /// </summary>
            Index,

            /// <summary>
            /// 获取物品编号，转为名字。
            /// </summary>
            ItemIdToName,

            /// <summary>
            /// 获取物品编号，转为描述。
            /// </summary>
            ItemIdToDesc,

            /// <summary>
            /// 获取英雄编号，转为名字。
            /// </summary>
            //HeroIdToName,

            /// <summary>
            /// 获取英雄编号，转为描述。
            /// </summary>
            //HeroIdToDesc,
        }

        [SerializeField]
        private Mode m_Mode = Mode.Default;

        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedString m_DataKeyFormat = null;

        [SerializeField]
        private SharedInt m_DataBegIndex = null;

        [SerializeField]
        private SharedInt m_DataEndIndex = null;

        [SerializeField]
        private SharedBool m_NeedReplacement = false;

        [SerializeField]
        private SharedString m_TextKey = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Only applies when Mode is Index.")]
        [SerializeField]
        private SharedInt m_IndexDelta = 1;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Only applies when Mode is Default.")]
        [SerializeField]
        private BasicDataType m_DataType = BasicDataType.Int32;

        private IUpdatableUIFragment m_Owner = null;

        private delegate bool TryGetStringDelegate(int index, out string text);

        private TryGetStringDelegate m_TryGetStringDelegate = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);

            switch (m_Mode)
            {
                case Mode.Index:
                    m_TryGetStringDelegate = TryGetString_Index;
                    break;
                case Mode.ItemIdToName:
                    m_TryGetStringDelegate = TryGetString_ItemIdToName;
                    break;
                case Mode.ItemIdToDesc:
                    m_TryGetStringDelegate = TryGetString_ItemIdToDesc;
                    break;
                default:
                    m_TryGetStringDelegate = TryGetString_Default;
                    break;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                return TaskStatus.Failure;
            }

            int dataCount = m_DataEndIndex.Value - m_DataBegIndex.Value;
            for (int i = 0; i < dataCount && i < m_GOList.Value.Count; ++i)
            {
                var go = m_GOList.Value[i];
                if (go == null)
                {
                    Log.Warning("Game object at index {0} is null.", i.ToString());
                    return TaskStatus.Failure;
                }

                var label = go.GetComponent<UILabel>();
                if (label == null)
                {
                    Log.Warning("Cannot find label at game object {0}.", i.ToString());
                    return TaskStatus.Failure;
                }

                string text;
                if (!m_TryGetStringDelegate(i + m_DataBegIndex.Value, out text))
                {
                    Log.Warning("Cannot get text at index {0}, in Mode '{1}'.", i.ToString(), m_Mode.ToString());
                    return TaskStatus.Failure;
                }

                if (m_NeedReplacement.Value)
                {
                    text = GameEntry.StringReplacement.GetString(text);
                }

                label.text = text;
            }

            return TaskStatus.Success;
        }

        private bool TryGetString_Default(int index, out string text)
        {
            text = string.Empty;
            string valStr;
            if (!TryGetDataStr(index, out valStr))
            {
                return false;
            }

            object val;
            switch (m_DataType)
            {
                case BasicDataType.Boolean:
                    bool valBool;
                    if (bool.TryParse(valStr, out valBool))
                    {
                        return false;
                    }

                    val = valBool;
                    break;

                case BasicDataType.Float:
                    float valFloat;
                    if (!float.TryParse(valStr, out valFloat))
                    {
                        return false;
                    }

                    val = valFloat;
                    break;

                case BasicDataType.Int32:
                    int valInt;
                    if (!int.TryParse(valStr, out valInt))
                    {
                        return false;
                    }

                    val = valInt;
                    break;

                case BasicDataType.String:
                default:

                    val = valStr;
                    return true;
            }

            text = string.IsNullOrEmpty(m_TextKey.Value) ? valStr : GameEntry.Localization.GetString(m_TextKey.Value, val);
            return true;
        }

        private bool TryGetString_Index(int index, out string text)
        {
            int num = index + m_IndexDelta.Value;
            text = string.IsNullOrEmpty(m_TextKey.Value) ? num.ToString() : GameEntry.Localization.GetString(m_TextKey.Value, num);
            return true;
        }

        private bool TryGetString_ItemIdToName(int index, out string text)
        {
            text = string.Empty;
            string dataStr;
            int itemId;
            if (!TryGetDataStr(index, out dataStr) || !int.TryParse(dataStr, out itemId))
            {
                return false;
            }

            var textKey = GeneralItemUtility.GetGeneralItemName(itemId);
            if (string.IsNullOrEmpty(textKey))
            {
                return false;
            }

            text = GameEntry.Localization.GetString(textKey);
            if (!string.IsNullOrEmpty(m_TextKey.Value))
            {
                text = GameEntry.Localization.GetString(m_TextKey.Value, text);
            }

            return true;
        }

        private bool TryGetString_ItemIdToDesc(int index, out string text)
        {
            text = string.Empty;
            string dataStr;
            int itemId;
            if (!TryGetDataStr(index, out dataStr) || !int.TryParse(dataStr, out itemId))
            {
                return false;
            }

            var textKey = GeneralItemUtility.GetGeneralItemDescription(itemId);
            if (!string.IsNullOrEmpty(textKey))
            {
                return false;
            }

            text = GameEntry.Localization.GetString(textKey);
            if (!string.IsNullOrEmpty(m_TextKey.Value))
            {
                text = GameEntry.Localization.GetString(m_TextKey.Value, text);
            }

            return true;
        }

        private bool TryGetDataStr(int index, out string dataStr)
        {
            if (string.IsNullOrEmpty(m_DataKeyFormat.Value))
            {
                dataStr = string.Empty;
            }

            string dataKey = string.Format(m_DataKeyFormat.Value, index.ToString());
            return m_Owner.TryGetData(dataKey, out dataStr);
        }
    }
}
