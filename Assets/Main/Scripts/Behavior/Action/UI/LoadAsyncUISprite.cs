using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class LoadAsyncUISprite : Action
    {
        private enum Mode
        {
            IconId,
            ItemId,
            SpriteAndAtlas,
        }

        [SerializeField]
        private Mode m_Mode = Mode.IconId;

        [SerializeField]
        private SharedGameObjectList m_GOList = null;

        [SerializeField]
        private SharedInt m_GOIndex = null;

        [SerializeField]
        private SharedInt m_ItemId = null;

        [SerializeField]
        private SharedInt m_IconId = null;

        [SerializeField]
        private SharedString m_AtlasPath = null;

        [SerializeField]
        private SharedString m_SpritePath = null;

        public override TaskStatus OnUpdate()
        {
            if (m_GOIndex.Value >= m_GOList.Value.Count)
            {
                return TaskStatus.Failure;
            }

            var go = m_GOList.Value[m_GOIndex.Value];
            if (go == null)
            {
                return TaskStatus.Failure;
            }

            var sprite = go.GetComponent<UISprite>();
            if (sprite == null)
            {
                return TaskStatus.Failure;
            }

            if (m_Mode == Mode.ItemId)
            {
                int iconId = GeneralItemUtility.GetGeneralItemIconId(m_ItemId.Value);
                sprite.LoadAsync(iconId);
            }
            else if (m_Mode == Mode.IconId)
            {
                sprite.LoadAsync(m_IconId.Value);
            }
            else
            {
                sprite.LoadAsync(m_AtlasPath.Value, m_SpritePath.Value);
            }

            return TaskStatus.Success;
        }
    }
}
