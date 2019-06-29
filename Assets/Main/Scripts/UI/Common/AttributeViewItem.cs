using UnityEngine;

namespace Genesis.GameClient
{
    public class AttributeViewItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_NameLabel = null;

        [SerializeField]
        private UILabel m_NumberLabel = null;

        [SerializeField]
        private GameObject m_NewIcon = null;

        public string Name
        {
            set { m_NameLabel.text = value; }
        }

        public string Value
        {
            set { m_NumberLabel.text = value; }
        }

        public void SetValue(float value, int type, bool isNew = false)
        {
            if (m_NewIcon != null)
            {
                m_NewIcon.SetActive(isNew);
                m_NewIcon.GetComponent<UILabel>().text = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_NEW");
            }
            m_NumberLabel.text = UIUtility.GetAttributeValueStr((AttributeType)type, value);
        }

        public void SetPlusValue(float value, int type, bool isNew = false)
        {
            if (m_NewIcon != null)
            {
                m_NewIcon.SetActive(isNew);
                m_NewIcon.GetComponent<UILabel>().text = GameEntry.Localization.GetString("UI_TEXT_ATTRIBUTE_NEW");
            }
            m_NumberLabel.text = UIUtility.GetAttributeValueStr((AttributeType)type, value);
        }
    }
}
