using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 抽奖详情界面 -- 预览项。
    /// </summary>
    public class ChanceDetailPreviewItem : MonoBehaviour
    {
        [SerializeField]
        private GeneralItemView m_GeneralItemView = null;

        [SerializeField]
        private GameObject m_ItemPanel = null;

        [SerializeField]
        private GameObject m_ItemBgGet = null;

        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UISprite m_Mask = null;

        private Animation m_CachedAnimation = null;

        public Animation CachedAnimation
        {
            get
            {
                return m_CachedAnimation;
            }
        }

        public float MaskAlpha
        {
            set
            {
                m_Mask.alpha = value;
            }

            get
            {
                return m_Mask.alpha;
            }
        }

        public GeneralItemView GeneralItemView
        {
            get
            {
                return m_GeneralItemView;
            }
        }

        public void RefreshAsHero(int heroId,
            GameFrameworkAction<UITexture, object> onSuccess = null,
            GameFrameworkAction<UITexture, object> onFailure = null,
            GameFrameworkAction<UITexture, object> onAbort = null,
            object userData = null)
        {
            m_ItemPanel.SetActive(false);
        }

        public void RefreshAsGeneralItem(int itemTypeId, int itemCount,
            GameFrameworkAction<UISprite, string, object> onSuccess = null,
            GameFrameworkAction<UISprite, object> onFailure = null,
            GameFrameworkAction<UISprite, object> onAbort = null,
            object userData = null)
        {
            m_ItemPanel.SetActive(true);

            m_GeneralItemView.InitGeneralItem(itemTypeId, itemCount);

            m_ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(itemTypeId), onSuccess, onFailure, onAbort, userData);
        }

        public void RefreshItemBgGet(int dummyIndex)
        {
            m_ItemBgGet.SetActive(dummyIndex >= 0);
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedAnimation = GetComponent<Animation>();
            MaskAlpha = 0f;
        }

        #endregion MonoBehaviour
    }
}
