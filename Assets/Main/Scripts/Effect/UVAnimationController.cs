using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// UV 动画控制器。
    /// </summary>
    public class UVAnimationController : MonoBehaviour
    {
        /// <summary>
        /// 模式。
        /// </summary>
        private enum Mode
        {
            /// <summary>
            /// 连续。
            /// </summary>
            Continuity,

            /// <summary>
            /// 序列图。
            /// </summary>
            Sequence,
        }

        [SerializeField]
        private bool m_Loop = true;

        [SerializeField]
        private float m_ScrollSpeed = 5f;

        [SerializeField]
        private float m_CountX = 4f;

        [SerializeField]
        private float m_CountY = 4f;

        [SerializeField]
        private Mode m_Mode = Mode.Continuity;

        [SerializeField]
        private float m_SpeedX = 0f;

        [SerializeField]
        private float m_SpeedY = 0f;

        [SerializeField]
        private Renderer m_Renderer = null;

        private float m_OffsetX = 0f;
        private float m_OffsetY = 0f;
        private float m_Frame = 0f;
        private float m_StepX = 0f;
        private float m_StepY = 0f;
        private GameFrameworkAction m_UpdateParamsDelegate = null;

        #region MonoBehaviour

        private void Awake()
        {
            if (m_CountX <= 0f)
            {
                m_CountX = 1f;
            }

            if (m_CountY <= 0f)
            {
                m_CountY = 1f;
            }

            var mats = m_Renderer.materials;

            for (int i = 0; i < mats.Length; ++i)
            {
                mats[i].mainTextureScale = new Vector2(1.0f / m_CountX, 1.0f / m_CountY);
                
            }

            switch (m_Mode)
            {
                case Mode.Sequence:
                    m_UpdateParamsDelegate = UpdateParams_SequenceMode;
                    break;
                case Mode.Continuity:
                default:
                    m_UpdateParamsDelegate = UpdateParams_ContinuityMode;
                    break;
            }
        }

        private void OnDestroy()
        {

        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {

        }

        private void Update()
        {
            m_UpdateParamsDelegate();
            RefreshTextureOffsets();
        }

        #endregion MonoBehaviour

        private void RefreshTextureOffsets()
        {
            var mats = m_Renderer.materials;

            for (int i = 0; i < mats.Length; ++i)
            {
                mats[i].SetTextureOffset("_MainTex", new Vector2(m_OffsetX, m_OffsetY));
            }
        }

        private void Reset()
        {
            m_OffsetX = 0f;
            m_OffsetY = 0f;
            m_Frame = 0f;
            m_StepX = 0f;
            m_StepY = 0f;
            RefreshTextureOffsets();
        }

        private void UpdateParams_ContinuityMode()
        {
            if (m_Loop)
            {
                if (m_OffsetX < -1f) m_OffsetX += 1f;
                if (m_OffsetY < -1f) m_OffsetY += 1f;
                if (m_OffsetX > 1f) m_OffsetX -= 1f;
                if (m_OffsetY > 1f) m_OffsetY -= 1f;
            }
            else
            {
                if (m_OffsetX < -1f) return;
                if (m_OffsetY < -1f) return;
                if (m_OffsetX > 1f) return;
                if (m_OffsetY > 1f) return;
            }

            m_OffsetX += m_SpeedX * Time.deltaTime;
            m_OffsetY += m_SpeedY * Time.deltaTime;
        }

        private void UpdateParams_SequenceMode()
        {
            if (m_StepY >= m_CountY)
            {
                if (!m_Loop)
                {
                    return;
                }

                m_StepY = 0f;
            }
            m_Frame += Time.deltaTime;
            m_StepX = Mathf.Floor(m_Frame * m_ScrollSpeed);
            m_OffsetX = m_StepX / m_CountX;

            if (m_StepX >= m_CountX)
            {
                m_StepY += 1;
                m_Frame = 0;
            }

            m_OffsetY = m_StepY / m_CountY;
        }
    }
}
