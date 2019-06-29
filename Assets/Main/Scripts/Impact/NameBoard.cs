using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class NameBoard : MonoBehaviour
    {
        [SerializeField]
        private Color m_MyNameColor = Color.red;

        [SerializeField]
        private Color m_OtherNameColor = Color.white;

        [SerializeField]
        private UILabel m_NameText = null;

        [SerializeField]
        private UILabel m_MeleeNameText = null;

        [SerializeField]
        private UILabel m_MyMeleeNameText = null;

        [SerializeField]
        private UIProgressBar[] m_HpBars = null;

        [SerializeField]
        private GoodsView[] m_Elements = null;

        [SerializeField]
        private UISprite[] m_BarForegrounds = null;

        [SerializeField]
        private Entity m_Owner = null;

        [SerializeField]
        private UISprite m_PvpSteadyBar = null;

        [SerializeField]
        private UISprite m_RecoverBar = null;

        [SerializeField]
        private UISprite m_PvpSelfSteadyBar = null;

        [SerializeField]
        private UISprite m_PvpSelfRecoverBar = null;

        [SerializeField]
        private UISprite m_LobbyNpcIcon = null;

        [SerializeField]
        private GameObject m_LobbyNpcObject = null;

        [SerializeField]
        private UILabel m_MeleeLevel = null;

        [SerializeField]
        private UILabel m_MyMeleeLevel = null;

        private Transform m_CachedTransform = null;
        private float m_StartTime = 0f;
        private float m_Height = 0f;
        private string m_BarForegroundName = string.Empty;
        private bool m_IsDead = false;
        private float m_AnimSeconds = 0f;
        private float m_RatioSpeed = 0f;
        private TargetableObject m_Target = null;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
                m_Target = m_Owner as TargetableObject;
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < m_HpBars.Length; i++)
            {
                m_HpBars[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < m_Elements.Length; i++)
            {
                m_Elements[i].gameObject.SetActive(false);
            }
        }

        public void SetLobbyNpcIcon(int iconId)
        {
            if (iconId <= 0)
            {
                return;
            }
            m_LobbyNpcIcon.LoadAsync(iconId);
        }

        public GameObject LobbyNpcObject
        {
            get
            {
                return m_LobbyNpcObject;
            }
        }

        public Action OnClickLobbyHeroButtonReturn
        {
            get;
            set;
        }

        public UISprite PvpSteadyBar
        {
            get
            {
                return m_PvpSteadyBar;
            }
        }

        public UILabel MeleeLevel
        {
            get
            {
                return m_MeleeLevel;
            }
        }

        public UILabel MyMeleeLevel
        {
            get
            {
                return m_MyMeleeLevel;
            }
        }

        public UISprite PvpSelfSteadyBar
        {
            get
            {
                return m_PvpSelfSteadyBar;
            }
        }

        public UISprite PvpSelfRecoverBar
        {
            get
            {
                return m_PvpSelfRecoverBar;
            }
        }

        public UISprite RecoverBar
        {
            get
            {
                return m_RecoverBar;
            }
        }

        public UIProgressBar[] HpBars
        {
            get
            {
                return m_HpBars;
            }
        }

        public GoodsView[] Elements
        {
            get
            {
                return m_Elements;
            }
        }

        public UILabel NameLabel
        {
            get
            {
                return m_NameText;
            }
        }

        public UILabel MeleeNameLabel
        {
            get
            {
                return m_MeleeNameText;
            }
        }

        public Color MyNameColor
        {
            get
            {
                return m_MyNameColor;
            }
        }

        public Color OtherNameColor
        {
            get
            {
                return m_OtherNameColor;
            }
        }

        public bool IsElite
        {
            get
            {
                NpcCharacter character = Owner as NpcCharacter;
                return character != null && character.Data.Category == NpcCategory.Elite && (m_Target.Camp == CampType.Enemy || m_Target.Camp == CampType.Enemy2);
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

        public int TargetType
        {
            get;
            set;
        }

        public UIProgressBar ProgressBar
        {
            get
            {
                return m_HpBars[TargetType];
            }
        }

        public UISprite ProgressBarForeground
        {
            get
            {
                return m_BarForegrounds[TargetType];
            }
        }

        public void OnClickLobbyHeroButton()
        {
            if (OnClickLobbyHeroButtonReturn != null)
            {
                OnClickLobbyHeroButtonReturn();
            }
        }

        public void SetName(string name)
        {
            m_NameText.text = name;
        }

        public void SetMeleeName(string name)
        {
            m_MeleeNameText.text = name;
        }

        public void SetMyMeleeName(string name)
        {
            m_MyMeleeNameText.text = name;
        }

        public void SetNameColor(Color color)
        {
            m_NameText.color = color;
        }

        public void SetAlpha(float alpha)
        {
            ProgressBar.alpha = alpha;
        }

        public void SetHPBarColor(string colorName)
        {
            m_BarForegroundName = colorName;
            if (ProgressBarForeground != null)
            {
                ProgressBarForeground.spriteName = m_BarForegroundName;
            }
        }

        public void SetHPWithAnim(float hpRatio, float animSeconds)
        {
            if (m_Target == null)
            {
                return;
            }
            if (animSeconds <= 0f)
            {
                animSeconds = 0.01f;
            }

            m_AnimSeconds = animSeconds;
            m_RatioSpeed = (hpRatio - m_Target.Data.HPRatio) / animSeconds;
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
            ProgressBarForeground.spriteName = m_BarForegroundName;
            m_CachedTransform.position = m_CachedTransform.position + new Vector3(0, 10000, 0);
        }

        private void Update()
        {
            RefreshPosition();
            if (m_Target == null)
            {
                return;
            }
            if (m_AnimSeconds > 0f)
            {
                m_AnimSeconds -= Time.deltaTime;
                ProgressBar.value += m_RatioSpeed * Time.deltaTime;
                if (m_AnimSeconds < 0f)
                {
                    ProgressBar.value += m_RatioSpeed * m_AnimSeconds;
                    m_AnimSeconds = 0f;
                }
            }
            else
            {
                ProgressBar.value = m_Target.Data.HPRatio;
            }

            if (m_Target.IsDead && !m_IsDead)
            {
                m_IsDead = true;
                m_StartTime = Time.time - Constant.HPBarKeepTime + 0.5f;
            }
            else if (m_Target.IsFakeDead)
            {
                m_StartTime = Time.time;
            }
        }
    }
}
