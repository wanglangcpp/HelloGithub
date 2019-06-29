using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class NGUISubForm : MonoBehaviour
    {
        protected UIEffectsController m_EffectsController = null;

        private UIPanel m_CachedUIPanel = null;
        private UIAnimation m_CachedAnimation = null;
        private UISound m_CachedUISound = null;

        private bool m_EnableOpenAnimation = false;
        private int m_ToggleGroupBaseValue = 0;
        private UIResourceReleaser m_ResourceReleaser = new UIResourceReleaser();

        protected int ToggleGroupBaseValue
        {
            get
            {
                return m_ToggleGroupBaseValue;
            }
        }

        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;
            }
        }

        public NGUIForm ParentForm
        {
            get;
            set;
        }

        public bool IsAvailable
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        public Transform CachedTransform
        {
            get;
            private set;
        }

        #region MonoBehaviour

        private void Awake()
        {
            CachedTransform = transform;
            m_CachedUIPanel = GetComponent<UIPanel>();
            if (m_CachedUIPanel == null)
            {
                Log.Error("Can not find UI panel in sub form.");
                return;
            }

            m_CachedAnimation = GetComponent<UIAnimation>();
            m_CachedUISound = GetComponent<UISound>();

            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
            m_ToggleGroupBaseValue = UIUtility.RefreshToggleGroupsForChildren(gameObject);

            m_EffectsController = GetComponent<UIEffectsController>();
            OnInit();
        }

        private void OnEnable()
        {
            if (m_EnableOpenAnimation)
            {
                m_EnableOpenAnimation = false;
                if (m_CachedAnimation != null)
                {
                    m_CachedAnimation.PlayOpenAnimations();
                }

                if (m_CachedUISound != null)
                {
                    m_CachedUISound.PlayOpenSounds();
                }
            }

            OnOpen();
        }

        private void OnDisable()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            OnClose();
        }

        private void Start()
        {
            m_ResourceReleaser.CollectWidgets(gameObject);
        }

        private void OnDestroy()
        {
            m_ResourceReleaser.ReleaseResources();
        }

        #endregion MonoBehaviour

        protected internal virtual void OnInit()
        {

        }

        protected internal virtual void OnOpen()
        {
            if (m_EffectsController != null)
            {
                m_EffectsController.Resume();
            }
        }

        protected internal virtual void OnClose()
        {
            if (m_EffectsController != null)
            {
                m_EffectsController.Pause();
            }
        }

        public void EnableOpenAnimation()
        {
            m_EnableOpenAnimation = true;
        }

        public void InternalClose()
        {
            if (m_CachedUISound != null)
            {
                m_CachedUISound.PlayCloseSounds();
            }

            if (m_CachedAnimation != null)
            {
                m_CachedAnimation.PlayCloseAnimations(CloseSelfImmediately);
            }
            else
            {
                CloseSelfImmediately(null);
            }
        }

        private void CloseSelfImmediately(object data)
        {
            gameObject.SetActive(false);
        }
    }
}
