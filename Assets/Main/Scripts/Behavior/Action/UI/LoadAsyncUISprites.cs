using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameFramework;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class LoadAsyncUISprites : Action
    {
        private enum Mode
        {
            IconId,
            ItemId,
        }

        [SerializeField]
        private Mode m_Mode = Mode.IconId;

        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedString m_DataKeyFormat = null;

        [SerializeField]
        private SharedInt m_DataBegIndex = null;

        [SerializeField]
        private SharedInt m_DataEndIndex = null;

        private IUpdatableUIFragment m_Owner = null;
        private GameFrameworkAction<UISprite, int> m_LoadAsyncDelegate = null;

        public override void OnStart()
        {
            base.OnStart();
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);

            switch (m_Mode)
            {
                case Mode.IconId:
                    m_LoadAsyncDelegate = LoadAsyncWithIconId;
                    break;
                case Mode.ItemId:
                default:
                    m_LoadAsyncDelegate = LoadAsyncWithItemId;
                    break;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_DataEndIndex.Value - m_DataBegIndex.Value && i < m_GOList.Value.Count; ++i)
            {
                var key = string.Format(m_DataKeyFormat.Value, (i + m_DataBegIndex.Value).ToString());
                string valStr;
                int itemOrIconId;
                if (!m_Owner.TryGetData(key, out valStr) || !int.TryParse(valStr, out itemOrIconId))
                {
                    return TaskStatus.Failure;
                }

                var go = m_GOList.Value[i];
                if (go == null)
                {
                    return TaskStatus.Failure;
                }

                var sprite = go.GetComponent<UISprite>();
                if (sprite == null)
                {
                    return TaskStatus.Failure;
                }

                m_LoadAsyncDelegate(sprite, itemOrIconId);
            }

            return TaskStatus.Success;
        }

        private void LoadAsyncWithIconId(UISprite sprite, int iconId)
        {
            sprite.LoadAsync(iconId);
        }

        private void LoadAsyncWithItemId(UISprite sprite, int itemId)
        {
            var iconId = GeneralItemUtility.GetGeneralItemIconId(itemId);
            LoadAsyncWithIconId(sprite, iconId);
        }
    }
}
