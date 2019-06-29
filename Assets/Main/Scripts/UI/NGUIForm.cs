using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public abstract class NGUIForm : UIFormLogic
    {
        public const int GroupDepthFactor = 10000;
        public const int FormDepthFactor = 100;
        protected UIEffectsController m_EffectsController = null;

        private int m_ToggleGroupBaseValue = 0;
        private UIPanel m_CachedUIPanel = null;
        private UIBackground m_CachedUIBackground = null;
        private UIAnimation m_CachedAnimation = null;
        private UISound m_CachedUISound = null;
        private IDictionary<string, NGUISubForm> m_SubForms = new Dictionary<string, NGUISubForm>();
        private bool m_HasJustOpened = false;
        private IDictionary<string, bool> m_ActivenessForRecoveryOnClose = new Dictionary<string, bool>();
        private UIResourceReleaser m_ResourceReleaser = new UIResourceReleaser();

        public int OriginalDepth
        {
            get; private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedUIPanel.depth;
            }
        }

        protected int ToggleGroupBaseValue
        {
            get
            {
                return m_ToggleGroupBaseValue;
            }
        }

        protected virtual bool BackButtonIsAvailable
        {
            get
            {
                return true;
            }
        }

        public bool CacheActivenessForRecoveryOnClose(string transformPath)
        {
            Transform trans = CachedTransform.Find(transformPath);
            if (trans == null)
            {
                return false;
            }

            m_ActivenessForRecoveryOnClose[transformPath] = trans.gameObject.activeSelf;
            return true;
        }

        [Obsolete("Use OpenSubForm method instead.")]
        public GameObject AddChild(GameObject parent, GameObject prefab)
        {
            var go = NGUITools.AddChild(parent, prefab);

            var panels = go.GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < panels.Length; ++i)
            {
                panels[i].depth += Depth;
            }

            return go;
        }

        public T CreateSubForm<T>(string name, GameObject parent, GameObject prefab, bool isOpen = true) where T : NGUISubForm
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.Warning("Open sub form failed, name is invalid.");
                return null;
            }

            NGUISubForm subForm = null;
            if (m_SubForms.TryGetValue(name, out subForm))
            {
                Log.Warning("Sub form '{0}' is already exist.", name);
                return subForm as T;
            }

            if (parent == null)
            {
                Log.Warning("Open sub form failed, parent is invalid.");
                return null;
            }

            if (prefab == null)
            {
                Log.Warning("Open sub form failed, prefab is invalid.");
                return null;
            }

            var go = NGUITools.AddChild(parent, prefab);

            subForm = go.GetComponent<NGUISubForm>();
            if (subForm == null)
            {
                Log.Error("Can not open sub from '{0}', which no NGUISubForm component exist.", name);
                Destroy(go);
                return null;
            }

            subForm.Name = name;
            subForm.ParentForm = this;
            subForm.gameObject.SetActive(isOpen);

            var panels = go.GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < panels.Length; ++i)
            {
                panels[i].depth += Depth - OriginalDepth;
            }

            m_SubForms.Add(name, subForm);

            return subForm as T;
        }

        public void OpenSubForm(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.Warning("Open sub form failed, name is invalid.");
                return;
            }

            NGUISubForm subForm = null;
            if (!m_SubForms.TryGetValue(name, out subForm))
            {
                Log.Warning("Can not open sub form '{0}'.", name);
                return;
            }

            OpenSubForm(subForm);
        }

        public void OpenSubForm(NGUISubForm subForm)
        {
            if (subForm == null)
            {
                Log.Warning("Open sub form failed, sub form is invalid.");
                return;
            }

            subForm.EnableOpenAnimation();
            subForm.gameObject.SetActive(true);
        }

        public void CloseSubForm(string name, bool immediate = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.Warning("Close sub form failed, name is invalid.");
                return;
            }

            NGUISubForm subForm = null;
            if (!m_SubForms.TryGetValue(name, out subForm))
            {
                Log.Warning("Can not close sub form '{0}'.", name);
                return;
            }

            CloseSubForm(subForm, immediate);
        }

        public void CloseSubForm(NGUISubForm subForm, bool immediate = false)
        {
            if (subForm == null)
            {
                Log.Warning("Close sub form failed, sub form is invalid.");
                return;
            }

            if (immediate)
            {
                subForm.gameObject.SetActive(false);
            }
            else
            {
                subForm.InternalClose();
            }
        }

        public void DestroySubForm(NGUISubForm subForm)
        {
            if (subForm == null)
            {
                Log.Warning("Destroy sub form failed, sub form is invalid.");
                return;
            }

            CloseSubForm(subForm, true);
            Destroy(subForm.gameObject);
        }

        public void DestroyAllSubForms()
        {
            foreach (NGUISubForm subForm in m_SubForms.Values)
            {
                CloseSubForm(subForm, true);
                Destroy(subForm.gameObject);
            }

            m_SubForms.Clear();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_EffectsController = GetComponent<UIEffectsController>();
            m_CachedUIPanel = GetComponent<UIPanel>();
            if (m_CachedUIPanel == null)
            {
                Log.Error("UI panel for '{0}' is invalid.", Name);
                return;
            }

            OriginalDepth = m_CachedUIPanel.depth;
            m_CachedUIBackground = GetComponent<UIBackground>();
            m_CachedAnimation = GetComponent<UIAnimation>();
            m_CachedUISound = GetComponent<UISound>();

            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
            m_ToggleGroupBaseValue = UIUtility.RefreshToggleGroupsForChildren(gameObject);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.ChangeSceneStart, OnChangeSceneStart);

            if (m_CachedUIBackground != null)
            {
                m_CachedUIBackground.OnOpen();
            }

            var myUserData = userData as UIFormBaseUserData;
            if (myUserData != null && myUserData.ShouldOpenImmediately || m_CachedAnimation == null)
            {
                StartCoroutine(OpenImmediatelyCo());
            }
            else
            {
                m_CachedAnimation.PlayOpenAnimations(OnPostOpen);
            }

            if (m_CachedUISound != null)
            {
                m_CachedUISound.PlayOpenSounds();
            }

            m_HasJustOpened = true;

            
        }

        protected virtual void OnPostOpen(object data)
        {
            if (m_EffectsController != null)
            {
                m_EffectsController.Resume();
            }
        }

        protected virtual void OnPreClose()
        {
            if (m_EffectsController != null)
            {
                m_EffectsController.Pause();
            }
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable)
            {
                Log.Info("[NGUIForm OnClose] GameEntry is not available.");
                return;
            }

            DestroyAllSubForms();

            if (m_CachedUIBackground != null)
            {
                m_CachedUIBackground.OnClose();
            }

            GameEntry.Event.Unsubscribe(EventId.ChangeSceneStart, OnChangeSceneStart);

            foreach (var kvPair in m_ActivenessForRecoveryOnClose)
            {
                var trans = CachedTransform.Find(kvPair.Key);
                if (trans != null)
                {
                    trans.gameObject.SetActive(kvPair.Value);
                }
            }

            base.OnClose(userData);
        }

        protected override void OnPause()
        {
            if (m_CachedUIBackground != null)
            {
                m_CachedUIBackground.OnPause();
            }

            if (m_EffectsController != null)
            {
                m_EffectsController.Pause();
            }

            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (m_EffectsController != null && !m_HasJustOpened)
            {
                m_EffectsController.Resume();
            }

            if (m_CachedUIBackground != null)
            {
                m_CachedUIBackground.OnResume();
            }

            m_HasJustOpened = false;

            GameEntry.NoviceGuide.CheckNoviceGuide(this);
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);

            if (m_EffectsController != null)
            {
                m_EffectsController.Resume();
            }

            if (m_CachedUIBackground != null)
            {
                m_CachedUIBackground.OnRefocus();
            }
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = GroupDepthFactor * uiGroupDepth + FormDepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            UIPanel[] panels = GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].depth += deltaDepth;
            }
        }

        public virtual void OnClickBackButton()
        {
            if (!BackButtonIsAvailable)
            {
                return;
            }

            PerformBackButtonAction();
        }

        protected virtual void PerformBackButtonAction()
        {
            CloseSelf();
        }

        protected void CloseSelf(bool immediate = false)
        {
            OnPreClose();

            if (immediate)
            {
                CloseSelfImmediately(null);
            }
            else
            {
                CloseSelfCo();
            }
        }

        private void CloseSelfCo()
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
            GameEntry.UI.CloseUIForm(UIForm);
        }

        private IEnumerator OpenImmediatelyCo()
        {
            if (m_CachedAnimation != null)
            {
                m_CachedAnimation.SkipToOpenAnimationsEnd();
            }

            yield return new WaitForEndOfFrame();
            OnPostOpen(null);
        }

        protected virtual void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            Log.Info("[NGUIForm OnChangeSceneStart] This is a '{0}', type ID is '{1}'.", GetType().Name, this.UIForm.TypeId);
            CloseSelf(true);
        }

        #region MonoBehaviour

        private void Start()
        {
            m_ResourceReleaser.CollectWidgets(gameObject);
        }

        private void OnDestroy()
        {
            m_ResourceReleaser.ReleaseResources();
        }

        #endregion MonoBehaviour
    }
}
