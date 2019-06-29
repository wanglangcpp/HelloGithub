using UnityEngine;

namespace Genesis.GameClient
{
    public class GearInfoAttributeItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_AttributeNameText = null;

        [SerializeField]
        private UILabel m_AttributeValueText = null;

        public string Name
        {
            get { return m_AttributeNameText.text; }
            set { m_AttributeNameText.text = value; }
        }

        public string Value
        {
            get { return m_AttributeValueText.text; }
            set { m_AttributeValueText.text = value; }
        }
    }
}
