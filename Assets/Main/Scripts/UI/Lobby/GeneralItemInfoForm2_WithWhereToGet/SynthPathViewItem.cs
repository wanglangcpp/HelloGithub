using UnityEngine;

namespace Genesis.GameClient
{
    public class SynthPathViewItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_RightArrow = null;

        [SerializeField]
        private UISprite m_DownArrow = null;

        [SerializeField]
        private GeneralItemView m_InnerItemView = null;

        public UISprite RightArrow
        {
            get
            {
                return m_RightArrow;
            }
        }

        public UISprite DownArrow
        {
            get
            {
                return m_DownArrow;
            }
        }

        public GeneralItemView InnerItemView
        {
            get
            {
                return m_InnerItemView;
            }
        }
    }
}
