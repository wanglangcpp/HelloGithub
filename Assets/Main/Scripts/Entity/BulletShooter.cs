using UnityEngine;

namespace Genesis.GameClient
{
    public class BulletShooter : MonoBehaviour
    {
        [SerializeField]
        private Transform m_ShooterTransform = null;

        private Animation m_ShooterAnimation = null;

        public Transform ShooterTransform
        {
            get
            {
                return m_ShooterTransform;
            }
        }

        public Animation ShooterAnimation
        {
            get
            {
                return m_ShooterAnimation;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_ShooterAnimation = ShooterTransform.GetComponent<Animation>();
        }

        private void OnDestroy()
        {
            m_ShooterAnimation = null;
        }

        #endregion MonoBehaviour
    }
}
