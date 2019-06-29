using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 第三人称控制器。
    /// </summary>
    [Serializable]
    public class ThirdPersonController
    {
        [SerializeField]
        private MeHeroCharacter m_Me = null;

        [SerializeField]
        private float m_MinRaw = 0f;

        [SerializeField]
        private float m_TargetOffsetFactor = 1f;


        private Transform m_CharacterCachedTransform = null;
        private Transform m_CameraCachedTransform = null;

        private float m_HorizontalRaw = 0f;
        private float m_VerticalRaw = 0f;

        public ThirdPersonController()
        {

        }

        public MeHeroCharacter MeHeroCharacter
        {
            get
            {
                return m_Me;
            }
            set
            {
                m_Me = value;
                m_CharacterCachedTransform = m_Me != null ? m_Me.transform : null;
                m_CameraCachedTransform = m_Me != null ? GameEntry.Scene.MainCamera.transform : null;
            }
        }

        public float HorizontalRaw
        {
            get
            {
                return m_HorizontalRaw;
            }
            set
            {
                m_HorizontalRaw = value;
            }
        }

        public float VerticalRaw
        {
            get
            {
                return m_VerticalRaw;
            }
            set
            {
                m_VerticalRaw = value;
            }
        }

        public void Update()
        {
            if (m_Me == null || !m_Me.isActiveAndEnabled)
            {
                return;
            }

            bool isMoving = Mathf.Abs(m_HorizontalRaw) + Mathf.Abs(m_VerticalRaw) > m_MinRaw;
            if (isMoving)
            {
                Vector3 forward = m_CameraCachedTransform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;

                Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                Vector3 direction = m_HorizontalRaw * right + m_VerticalRaw * forward;
                Vector3 targetPosition = m_CharacterCachedTransform.localPosition + m_TargetOffsetFactor * m_Me.Data.Speed * Time.deltaTime * direction;

                m_Me.StartMove(targetPosition, direction);
            }
            else if (m_Me.AutoMove)
            {
                GameEntry.TaskComponent.CheckHeroPostion();
            }
            else
            {
                m_Me.StopMove();
            }

            IsTryingToMove = isMoving;
        }

        public bool IsTryingToMove
        {
            get; private set;
        }
    }
}
