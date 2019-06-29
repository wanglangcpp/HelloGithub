using UnityEngine;

namespace Genesis.GameClient
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField]
        private UILabel[] m_BubbleContents = null;

        [SerializeField]
        private UISprite[] m_BubbleBgs = null;

        [SerializeField]
        private TargetableObject m_Owner = null;

        [SerializeField]
        private float m_HeightFactor = 1.25f;

        private Transform m_CachedTransform = null;
        private float m_StartTime = 0f;
        private float m_Height = 0f;
        private bool m_IsDead = false;
        private float m_AnimSeconds = 0f;

        public float HeightFactor
        {
            get { return m_HeightFactor; }
        }

        public int TargetType
        {
            get;
            set;
        }

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

        public float StartTime
        {
            get
            {
                return m_StartTime;
            }
            set
            {
                m_StartTime = value;
            }
        }

        public float Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                m_Height = value;
            }
        }

        public void SetContent(string content)
        {
            m_BubbleContents[TargetType].text = content;
            for (int i = 0; i < m_BubbleBgs.Length; i++)
            {
                m_BubbleBgs[i].UpdateAnchors();
            }
        }

        public void RefreshPosition()
        {
            Vector3 uiPoint;
            if (UIUtility.WorldToUIPoint(m_Owner.CachedTransform.position + Vector3.up * m_Height, out uiPoint))
            {
                m_CachedTransform.position = uiPoint;
            }
        }

        private void Awake()
        {
            m_CachedTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (m_AnimSeconds > 0f)
            {
                m_AnimSeconds -= Time.deltaTime;
                if (m_AnimSeconds < 0f)
                {
                    m_AnimSeconds = 0f;
                }
            }

            if (m_Owner.IsDead && !m_IsDead)
            {
                m_IsDead = true;
                m_StartTime = Time.time - Constant.HPBarKeepTime + 0.5f;
            }
            else if (m_Owner.IsFakeDead)
            {
                m_StartTime = Time.time;
            }

            RefreshPosition();
        }

    }
}
