using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PvpSelectForm
    {
        [Serializable]
        public class PvpItem
        {
            [SerializeField]
            private Transform m_CachedTransform = null;

            public Transform CachedTransform
            {
                get
                {
                    return m_CachedTransform;
                }
            }

            [SerializeField]
            private UITexture m_Image = null;

            public UITexture Image
            {
                get
                {
                    return m_Image;
                }
            }

            [SerializeField]
            private UISprite m_NameIcon = null;

            public UISprite NameIcon
            {
                get
                {
                    return m_NameIcon;
                }
            }

            [SerializeField]
            private UIButton m_Button = null;

            public UIButton Button
            {
                get
                {
                    return m_Button;
                }
            }
        }
    }
}
