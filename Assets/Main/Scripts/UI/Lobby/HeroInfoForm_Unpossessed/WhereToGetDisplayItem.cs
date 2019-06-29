using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取途径的界面显示类。
    /// </summary>
    public class WhereToGetDisplayItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_WhereToGoDescription = null;

        [SerializeField]
        private UISprite m_ContentIcon = null;

        [SerializeField]
        private UISprite m_LockIcon = null;

        private WhereToGetLogic_Base m_WhereToGetLogic = null;

        private GameFrameworkAction<object> m_OnPostClick = null;
        private object m_PostClickUserData = null;

        /// <summary>
        /// 刷新数据。
        /// </summary>
        /// <param name="whereToGetLogic">获取途径逻辑。</param>
        public void RefreshData(WhereToGetLogic_Base whereToGetLogic)
        {
            m_WhereToGetLogic = whereToGetLogic;
            m_WhereToGoDescription.color = Color.white;
            m_WhereToGoDescription.applyGradient = true;
            m_WhereToGoDescription.text = GameEntry.Localization.GetString(m_WhereToGetLogic.TextKey);
            m_LockIcon.gameObject.SetActive(whereToGetLogic.IsLocked);

            if (whereToGetLogic.Type == WhereToGetType.SinglePlayerInstance)
            {
                if (whereToGetLogic.IsLocked)
                {
                    m_ContentIcon.LoadAsync(AssetUtility.GetAtlasAsset("Instance"), "icon_instance_lock", onSuccess: OnLoadContentIconSuccess);
                    m_ContentIcon.color = Color.grey;
                    m_ContentIcon.spriteName = "icon_instance_lock";    // 没查到为啥、直接用LoadAsync不是很好用，所以在这这样设置一下。
                    m_WhereToGoDescription.color = new Color(82 / 255f, 96 / 255f, 103 / 255f);
                    m_WhereToGoDescription.applyGradient = false;
                }
                else
                {
                    m_ContentIcon.LoadAsync(AssetUtility.GetAtlasAsset("Instance"), "icon_instance_common", onSuccess: OnLoadContentIconSuccess);
                    m_ContentIcon.spriteName = "icon_instance_common";
                    m_ContentIcon.color = Color.white;
                }
            }
            else
            {
                m_ContentIcon.LoadAsync(whereToGetLogic.IconId, OnLoadContentIconSuccess);
            }
        }

        /// <summary>
        /// 执行点击行为。
        /// </summary>
        public void OnClickItem()
        {
            if (m_WhereToGetLogic.IsLocked)
            {
                return;
            }

            m_WhereToGetLogic.OnClick();

            if (m_OnPostClick != null && !m_WhereToGetLogic.IsCleanOutInstance)
            {
                m_OnPostClick(m_PostClickUserData);
            }
        }

        /// <summary>
        /// 设置点击后的行为。
        /// </summary>
        /// <param name="onPostClick">回调函数。</param>
        /// <param name="forbidDefaultOnClick">是否屏蔽默认点击行为。</param>
        /// <param name="userData">用户数据。</param>
        public void SetPostClickDelegate(GameFrameworkAction<object> onPostClick, bool forbidDefaultOnClick, object userData = null)
        {
            m_OnPostClick = onPostClick;
            m_PostClickUserData = userData;
        }

        #region MonoBehaviour

        private void OnDestroy()
        {
            SetPostClickDelegate(null, false);
        }

        #endregion MonoBehaviour

        private void OnLoadContentIconSuccess(UISprite sprite, string spriteName, object userData)
        {
            UIButton button;
            if ((button = sprite.GetComponent<UIButton>()) != null)
            {
                button.normalSprite = spriteName;
            }
        }
    }
}
