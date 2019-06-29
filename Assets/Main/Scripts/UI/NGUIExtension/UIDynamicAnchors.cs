using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIDynamicAnchors : MonoBehaviour
    {
        private enum TargetHorizontalType
        {
            None,
            Left,
            Right,
            Center,
        }

        private enum TargetVerticalType
        {
            None,
            Bottom,
            Top,
            Center,
        }

        [SerializeField]
        private string m_TargetPath = string.Empty;

        [SerializeField]
        private TargetHorizontalType m_Left = TargetHorizontalType.Left;

        [SerializeField]
        private float m_LeftAbsolute = 0.0f;

        [SerializeField]
        private TargetHorizontalType m_Right = TargetHorizontalType.Right;

        [SerializeField]
        private float m_RightAbsolute = 0.0f;

        [SerializeField]
        private TargetVerticalType m_Bottom = TargetVerticalType.Bottom;

        [SerializeField]
        private float m_BottomAbsolute = 0.0f;

        [SerializeField]
        private TargetVerticalType m_Top = TargetVerticalType.Top;

        [SerializeField]
        private float m_TopAbsolute = 0.0f;

        private GameObject m_AnchorTarget = null;

        public GameObject AnchorTarget
        {
            set
            {
                m_AnchorTarget = value;
            }
            get
            {
                return m_AnchorTarget;
            }
        }

        private void Start()
        {
            if (AnchorTarget == null)
            {
                AnchorTarget = GetTargetObj(m_TargetPath);
            }

            if (AnchorTarget == null)
            {
                return;
            }

            UIRect rect = GetComponent<UIRect>();
            SetHorizontalAnchor(rect.leftAnchor, m_Left, m_LeftAbsolute);
            SetHorizontalAnchor(rect.rightAnchor, m_Right, m_RightAbsolute);
            SetVerticalAnchor(rect.topAnchor, m_Top, m_TopAbsolute);
            SetVerticalAnchor(rect.bottomAnchor, m_Bottom, m_BottomAbsolute);
            rect.UpdateAnchors();
        }

        private void SetHorizontalAnchor(UIRect.AnchorPoint point, TargetHorizontalType type, float absolute)
        {
            point.rect = m_AnchorTarget.GetComponent<UIRect>();
            switch (type)
            {
                case TargetHorizontalType.Left:
                    point.Set(m_AnchorTarget.transform, 0, absolute);
                    break;
                case TargetHorizontalType.Right:
                    point.Set(m_AnchorTarget.transform, 1, absolute);
                    break;
                case TargetHorizontalType.Center:
                    point.Set(m_AnchorTarget.transform, 0.5f, absolute);
                    break;
                default:
                    point.target = null;
                    break;
            }
        }

        private void SetVerticalAnchor(UIRect.AnchorPoint point, TargetVerticalType type, float absolute)
        {
            point.rect = m_AnchorTarget.GetComponent<UIRect>();
            switch (type)
            {
                case TargetVerticalType.Bottom:
                    point.Set(m_AnchorTarget.transform, 0, absolute);
                    break;
                case TargetVerticalType.Top:
                    point.Set(m_AnchorTarget.transform, 1, absolute);
                    break;
                case TargetVerticalType.Center:
                    point.Set(m_AnchorTarget.transform, 0.5f, absolute);
                    break;
                default:
                    point.target = null;
                    break;
            }
        }

        private GameObject GetTargetObj(string targetPath)
        {
            if (targetPath == string.Empty)
            {
                return null;
            }

            Transform target = transform;
            string[] pathSegments = targetPath.Split('/');

            for (int index = 0; index < pathSegments.Length; index++)
            {
                var currentSegment = pathSegments[index];
                if (currentSegment == string.Empty || currentSegment == ".")
                {
                    continue;
                }

                if (currentSegment == "..")
                {
                    target = target.parent;
                    continue;
                }

                target = target.Find(currentSegment);

                if (target == null)
                {
                    Log.Error("Cannot find the wanted transform with relative path '{0}' and current segment '{1}'.", targetPath, currentSegment);
                    return null;
                }
            }

            return target.gameObject;
        }
    }
}
