using GameFramework.ObjectPool;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class BaseMonsterPositionArrow
    {
        [SerializeField]
        private TargetableObject m_Owner = null;

        protected MonsterPositionArrow m_ArrowPrompt = null;

        private const float FromScreenDistanceRadio = 0.1f;

        public TargetableObject Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
            }
        }

        public void Init(MonsterPositionArrow arrowPrompt)
        {
            m_ArrowPrompt = arrowPrompt;
        }

        public void DestroyArrowPrompt(IObjectPool<MonsterPositionArrowObject> arrowPrompt)
        {
            Owner = null;
            if (m_ArrowPrompt != null && m_ArrowPrompt.gameObject != null)
            {
                m_ArrowPrompt.gameObject.SetActive(false);
                arrowPrompt.Unspawn(m_ArrowPrompt);
                m_ArrowPrompt = null;
            }
        }

        public virtual void RefreshArrowPrompt(TargetableObject targetableObject)
        {
            if (m_ArrowPrompt != null)
            {
                m_ArrowPrompt.transform.localScale = Vector3.one;
                Owner = targetableObject;
                SetArrowPromptPosition();
            }
        }

        public virtual void OnUpdate()
        {
            SetArrowPromptPosition();
        }

        private void SetArrowPromptPosition()
        {
            if (m_Owner == null || GameEntry.Scene.MainCamera == null)
            {
                return;
            }
            Vector3 viewVec = GameEntry.Scene.MainCamera.WorldToViewportPoint(m_Owner.transform.position);
            Vector3 newViewVec = new Vector3();
            Vector3 offsetSize = new Vector3();

            if (viewVec.x >= 1 && (viewVec.y < 1 && viewVec.y > 0))
            {
                newViewVec.y = viewVec.y - 0.5f;
                newViewVec.x = 0.5f;
                m_ArrowPrompt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                offsetSize.x = -m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
            }
            else if (viewVec.x <= 0 && (viewVec.y < 1 && viewVec.y > 0))
            {
                newViewVec.y = viewVec.y - 0.5f;
                newViewVec.x = -0.5f;
                m_ArrowPrompt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                offsetSize.x = m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
            }
            else if (viewVec.y >= 1)
            {
                newViewVec.y = 0.5f;
                if (viewVec.x >= 1)
                {
                    offsetSize.x = -m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
                    newViewVec.x = 0.5f;
                }
                else if (viewVec.x <= 0)
                {
                    newViewVec.x = -0.5f;
                    offsetSize.x = m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
                }
                else
                {
                    newViewVec.x = viewVec.x - 0.5f;
                }
                m_ArrowPrompt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                offsetSize.y = -m_ArrowPrompt.ArrowIcon.height * FromScreenDistanceRadio;
            }
            else if (viewVec.y <= 0)
            {
                newViewVec.y = -0.5f;
                if (viewVec.x >= 1)
                {
                    newViewVec.x = 0.5f;
                    offsetSize.x = -m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
                }
                else if (viewVec.x <= 0)
                {
                    newViewVec.x = -0.5f;
                    offsetSize.x = m_ArrowPrompt.ArrowIcon.width * FromScreenDistanceRadio;
                }
                else
                {
                    newViewVec.x = viewVec.x - 0.5f;
                }
                m_ArrowPrompt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                offsetSize.y = m_ArrowPrompt.ArrowIcon.height * FromScreenDistanceRadio;
            }

            Vector3 uiPoint = GameEntry.Scene.MainCamera.ViewportToScreenPoint(newViewVec);

            var uiRoot = NGUITools.FindInParents<UIRoot>(m_ArrowPrompt.gameObject);
            float ratio = (uiRoot != null) ? (float)uiRoot.activeHeight / Screen.height : 1.0f;

            m_ArrowPrompt.transform.localPosition = uiPoint * ratio;

            m_ArrowPrompt.transform.localPosition += offsetSize;
        }
    }
}
