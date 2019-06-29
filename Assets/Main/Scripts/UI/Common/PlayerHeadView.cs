using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class PlayerHeadView : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private GameObject m_Select = null;

        [SerializeField]
        private BoxCollider m_Collider = null;

        private Action<int> m_OnClickReturn = null;

        private int m_PortraitId;

        public void InitPlayerHead(int portraitId, Action<int> onClickReturn = null)
        {
            m_PortraitId = portraitId;
            m_OnClickReturn = onClickReturn;
            if (m_Collider != null)
            {
                m_Collider.enabled = false;
            }
            m_Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(portraitId), OnPortraitLoadSuccess);
        }

        public bool Select
        {
            set
            {
                if (m_Select == null)
                {
                    return;
                }
                m_Select.SetActive(value);
            }
        }

        public int PortraitId
        {
            get
            {
                return m_PortraitId;
            }
        }

        private void OnPortraitLoadSuccess(UISprite sprite, string spriteName, object userData)
        {
            if (m_Collider != null)
            {
                m_Collider.enabled = true;
            }
        }

        public void OnClickView()
        {
            if (m_OnClickReturn != null)
            {
                m_OnClickReturn(m_PortraitId);
            }
        }
    }
}
