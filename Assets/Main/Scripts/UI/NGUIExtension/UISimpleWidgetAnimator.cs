using UnityEngine;

namespace Genesis.GameClient
{
    public class UISimpleWidgetAnimator : MonoBehaviour
    {
        [SerializeField]
        private UIWidget m_ReferenceWidget = null;

        [SerializeField]
        private UIPanel m_ReferencePanel = null;

        [SerializeField]
        private float m_Speed = 0;

        private UIWidget m_CachedWidget;

        private float m_BegPos;
        private float m_EndPos;

        private void Awake()
        {
            m_CachedWidget = GetComponent<UIWidget>();
        }

        private void Start()
        {
            var referenceX = m_ReferencePanel == null ? m_ReferenceWidget.cachedTransform.localPosition.x : m_ReferencePanel.cachedTransform.localPosition.x;
            var referenceWidth = m_ReferencePanel == null ? m_ReferenceWidget.localSize.x : m_ReferencePanel.GetViewSize().x;
            m_BegPos = referenceX + .5f * referenceWidth + .5f * m_CachedWidget.localSize.x;
            m_EndPos = referenceX - .5f * referenceWidth - .5f * m_CachedWidget.localSize.x;

            ResetPos();
        }

        private void ResetPos()
        {
            m_CachedWidget.cachedTransform.SetLocalPositionX(m_BegPos);
        }

        private void OnDisable()
        {
            ResetPos();
        }

        private void Update()
        {
            var currentPos = m_CachedWidget.cachedTransform.localPosition.x;

            if ((currentPos - m_BegPos) / (m_EndPos - m_BegPos) > 1f)
            {
                ResetPos();
                return;
            }

            m_CachedWidget.cachedTransform.AddLocalPositionX(m_Speed * Time.deltaTime);
        }
    }
}
