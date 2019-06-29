using UnityEngine;

namespace Genesis.GameClient
{
    public class SoulPacketItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_SoulNameText = null;

        [SerializeField]
        private UILabel m_SoulDescText = null;

        [SerializeField]
        private UISprite m_SoulIcon = null;

        [SerializeField]
        private GameObject m_MultiSelectIcon = null;

#pragma warning disable 0414

        [SerializeField]
        private UILabel m_MultiSelectIndex = null;

#pragma warning restore 0414

        private SoulPacketPanel m_ParentPanel = null;
        private SoulData m_Soul = null;

        public SoulData SoulInfo
        {
            get { return m_Soul; }
            private set { m_Soul = value; }
        }

        public bool IsMultiSelected
        {
            get { return m_MultiSelectIcon.activeSelf; }
        }

        public void InitData(SoulData soul, SoulPacketPanel parent, bool IsMultiChoice)
        {
            m_Soul = soul;
            m_ParentPanel = parent;
            m_SoulIcon.LoadAsync(soul.IconId);
            m_SoulNameText.text = GameEntry.Localization.GetString(soul.Name);
            m_SoulDescText.text = GameEntry.Localization.GetString(soul.Description);
        }

        public void OnClickMe()
        {
            m_ParentPanel.OnSelectItem(this);
        }
    }
}
