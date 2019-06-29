using UnityEngine;

namespace Genesis.GameClient
{
    public class OpenLinkOnClick : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_Label = null;

        private void OnClick()
        {
            if (m_Label != null)
            {
                string url = m_Label.GetUrlAtPosition(UICamera.lastWorldPosition);
                if (!string.IsNullOrEmpty(url))
                {
                    m_Label.text = "hello world";
                }
            }
        }
    }
}
