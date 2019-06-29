using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品信息提示界面。
    /// </summary>
    public class TipsForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_NameLbl = null;

        [SerializeField]
        private UILabel m_DescLbl = null;

        [SerializeField]
        private UIWidget m_Bg = null;

        [SerializeField]
        private int m_Margin = 5;

        private TipsFormDisplayData m_CachedDisplayData = null;

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_CachedDisplayData = userData as TipsFormDisplayData;
            if (m_CachedDisplayData == null)
            {
                Log.Error("Display data is invalid.");
                return;
            }

            m_NameLbl.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(m_CachedDisplayData.GeneralItemId));
            m_DescLbl.text = GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(m_CachedDisplayData.GeneralItemId)));
            RefreshPos();
        }

        protected override void OnClose(object userData)
        {
            m_CachedDisplayData = null;
            base.OnClose(userData);
        }

        #endregion NGUIForm

        private void RefreshPos()
        {
            m_Bg.cachedTransform.localPosition = Vector3.zero;

            var refTrans = m_CachedDisplayData.RefTransform;
            if (refTrans == null)
            {
                return;
            }

            var center = CachedTransform.InverseTransformPoint(refTrans.position);
            UIRoot uiRoot = GetComponentInParent<UIRoot>();

            float halfHeight = m_Bg.height * .5f;
            float halfWidth = m_Bg.width * .5f;
            float halfScreenHeight = uiRoot.manualHeight * .5f;
            float halfScreenWidth = (float)uiRoot.manualHeight / Screen.height * Screen.width * .5f;

            if (center.y + halfHeight * 2 > halfScreenHeight - m_Margin)
            {
                m_Bg.cachedTransform.SetLocalPositionY(center.y - halfHeight);
            }
            else if (center.y < halfHeight * 2 - halfScreenHeight + m_Margin)
            {
                m_Bg.cachedTransform.SetLocalPositionY(halfHeight - halfScreenHeight + m_Margin);
            }
            else
            {
                m_Bg.cachedTransform.SetLocalPositionY(center.y + halfHeight);
            }

            if (center.x + halfWidth > halfScreenWidth - m_Margin)
            {
                m_Bg.cachedTransform.SetLocalPositionX(halfScreenWidth - m_Margin - halfWidth);
            }
            else if (center.x - halfWidth < m_Margin - halfScreenWidth)
            {
                m_Bg.cachedTransform.SetLocalPositionX(m_Margin - halfScreenWidth + halfWidth);
            }
            else
            {
                m_Bg.cachedTransform.SetLocalPositionX(center.x);
            }
        }
    }
}
