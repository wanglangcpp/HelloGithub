using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机视口控制器。
    /// </summary>
    /// <remarks>支持标准化坐标为负值的情况。</remarks>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraViewportController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_ViewportOffset = Vector2.zero;

        private Camera m_Camera = null;
        public void UpdateCameraViewport(Vector2 offset)
        {
            m_ViewportOffset = offset;
            UpdateCameraViewport();
        }
        [ContextMenu("Excute")]
        public void UpdateCameraViewport()
        {
            // Reference: http://answers.unity3d.com/questions/555447/move-the-viewport-but-maintain-its-size.html

            var r = new Rect(0f, 0f, 1f, 1f);
            var alignFactor = Vector2.one;
            if (m_ViewportOffset.y >= 0f)
            {
                r.height = 1f - m_ViewportOffset.y;
                alignFactor.y = 1f;
            }
            else
            {
                r.y = -m_ViewportOffset.y;
                r.height = 1f + m_ViewportOffset.y;
                alignFactor.y = -1f;
            }

            if (m_ViewportOffset.x >= 0f)
            {
                r.width = 1f - m_ViewportOffset.x;
                alignFactor.x = 1f;
            }
            else
            {
                r.x = -m_ViewportOffset.x;
                r.width = 1f + m_ViewportOffset.x;
                alignFactor.x = -1f;
            }

            if (r.width == 0f)
            {
                r.width = 0.001f;
            }

            if (r.height == 0f)
            {
                r.height = 0.001f;
            }

            m_Camera.rect = new Rect(0, 0, 1, 1);
            m_Camera.ResetProjectionMatrix();
            Matrix4x4 m = m_Camera.projectionMatrix;
            m_Camera.rect = r;

            Matrix4x4 m2 = Matrix4x4.TRS(new Vector3(alignFactor.x * (-1 / r.width + 1), alignFactor.y * (-1 / r.height + 1), 0),
                Quaternion.identity,
                new Vector3(1 / r.width, 1 / r.height, 1));

            m_Camera.projectionMatrix = m2 * m;
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
            UpdateCameraViewport();
        }


#if UNITY_EDITOR

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            UpdateCameraViewport();
        }

#endif

        #endregion MonoBehaviour
    }
}
