using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 仅用于显示元素（克制属性）。
    /// </summary>
    public class GoodsView : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_Icon = null;

        public int Count
        {
            get;
            private set;
        }

        public void InitElementView(int elementId)
        {
            if (m_Icon != null)
            {
                m_Icon.spriteName = UIUtility.GetElementSpriteName(elementId);
                var button = m_Icon.GetComponent<UIButton>();
                if (button != null)
                {
                    button.normalSprite = m_Icon.spriteName;
                }
            }
        }

        // Called via reflection by NGUI.
        private void OnClick()
        {

        }
    }
}
