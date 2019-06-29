using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIScrollViewFollower : MonoBehaviour
    {
        [SerializeField]
        private UIScrollView m_TargetScrollView = null;

        [SerializeField]
        private Transform m_TargetTransform = null;

        [SerializeField]
        private Vector2 m_FollowingFactor = Vector2.zero;

        [SerializeField]
        private bool m_ShouldClampDragAmount = false;

        private Vector3 m_TargetObjectOriginalPos = Vector3.zero;
        private Vector2 m_FollowingValue = Vector3.zero;
        private Vector2 m_MaxTargetPos = Vector2.zero;
        private Vector2 m_MinTargetPos = Vector2.zero;

        private void Start()
        {
            StartCoroutine(InitWait());
        }

        /// <remark>
        /// 协程，等一帧再初始化，否则读取控件所属面板的尺寸是错误的。
        /// </remark>
        private IEnumerator InitWait()
        {
            yield return null;

            m_TargetObjectOriginalPos = m_TargetTransform.localPosition;
            SetUtmostPos();
            if (m_TargetScrollView.movement == UIScrollView.Movement.Horizontal)
            {
                m_FollowingValue = new Vector2(m_FollowingValue.x, 0f);
                m_MaxTargetPos = new Vector2(m_MaxTargetPos.x, 0f);
                m_MinTargetPos = new Vector2(m_MinTargetPos.x, 0f);
            }
            else if (m_TargetScrollView.movement == UIScrollView.Movement.Vertical)
            {
                m_FollowingValue = new Vector2(0f, m_FollowingValue.y);
                m_MaxTargetPos = new Vector2(0f, m_MaxTargetPos.y);
                m_MinTargetPos = new Vector2(0f, m_MinTargetPos.y);
            }
            else if (m_TargetScrollView.movement == UIScrollView.Movement.Unrestricted)
            {
                m_FollowingValue = new Vector2(m_FollowingValue.x, m_FollowingValue.y);
            }
            UpdateTargetObjectPosition();
        }

        private void Update()
        {
            UpdateTargetObjectPosition();
        }

        /// <summary>
        /// 每一帧根据DragAmount来移动图片位置。
        /// </summary>
        private void UpdateTargetObjectPosition()
        {
            if (m_TargetScrollView == null || !m_TargetScrollView.gameObject.activeSelf)
            {
                return;
            }

            var dragAmount = NGUIExtensionMethods.GetDragAmount(m_TargetScrollView);
            if (m_ShouldClampDragAmount)
            {
                dragAmount = new Vector2(Mathf.Clamp(dragAmount.x * m_FollowingFactor.x * m_FollowingValue.x, m_MinTargetPos.x, m_MaxTargetPos.x),
                    Mathf.Clamp(dragAmount.y * m_FollowingFactor.y * m_FollowingValue.y, m_MinTargetPos.y, m_MaxTargetPos.y));
            }

            m_TargetTransform.localPosition = new Vector3(
                m_TargetObjectOriginalPos.x + dragAmount.x,
                m_TargetObjectOriginalPos.y + dragAmount.y,
                m_TargetObjectOriginalPos.z);
        }

        /// <summary>
        /// 根据背景图片和面板剪切计算出来图片可以移动的极限坐标。
        /// </summary>
        private void SetUtmostPos()
        {
            if (m_TargetTransform == null || m_TargetScrollView == null)
            {
                return;
            }
            var targetSVPanel = m_TargetScrollView.GetComponent<UIPanel>();
            var targetBgWidget = m_TargetTransform.GetComponent<UIWidget>();
            Vector3[] pos = targetBgWidget.GetSides(targetSVPanel.transform);

            float deltaX = (targetBgWidget.width - targetSVPanel.width) / 2;
            float deltaY = (targetBgWidget.height - targetSVPanel.height) / 2;

            deltaX = deltaX < 0 ? 0 : deltaX;
            deltaY = deltaY < 0 ? 0 : deltaY;

            m_FollowingValue = new Vector2(deltaX, deltaY);

            m_MaxTargetPos = new Vector2(m_TargetTransform.localPosition.x - pos[1].x + deltaX,
                m_TargetTransform.localPosition.y - pos[0].y + deltaY);

            m_MinTargetPos = new Vector2(m_TargetTransform.localPosition.x - pos[1].x - deltaX,
               m_TargetTransform.localPosition.y - pos[0].y - deltaY);
        }

    }
}
