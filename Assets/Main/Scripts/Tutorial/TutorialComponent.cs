using GameFramework;
using GameFramework.Event;
using GameFramework.UI;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 教程组件。
    /// </summary>
    public class TutorialComponent : MonoBehaviour
    {
        [SerializeField]
        private TutorialConfig m_Config = null;

        [SerializeField]
        private string m_TemplateName = null;

        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Tutorial Mask";

        private const string DefaultErrorMessage = "You have to show the tutorial mask before showing or hiding a tip or tutorial step.";

        private GameObject m_Template = null;
        private TutorialMask m_TutorialMask = null;

        /// <summary>
        /// Get the tutorial configurations.
        /// </summary>
        public TutorialConfig Config
        {
            get
            {
                return m_Config;
            }
        }

        /// <summary>
        /// Check whether the <see cref="TutorialMask"/> is showing.
        /// </summary>
        public bool MaskIsShowing
        {
            get
            {
                return m_TutorialMask != null && m_TutorialMask.IsShowing;
            }
        }

        /// <summary>
        /// Preload resources.
        /// </summary>
        public void Preload()
        {
            PreloadUtility.LoadPreloadResource(m_TemplateName);
        }

        /// <summary>
        /// Show the <see cref="TutorialMask"/>.
        /// </summary>
        /// <param name="uiFormId">The ID of the <see cref="NGUIForm"/> in front of which the mask should display.</param>
        public void ShowMask(UIFormId uiFormId)
        {
            ShowMask(uiFormId, Config.DefaultDepthAllowance);
        }

        /// <summary>
        /// Show the <see cref="TutorialMask"/>.
        /// </summary>
        /// <param name="uiFormId">The ID of the <see cref="NGUIForm"/> in front of which the mask should display.</param>
        /// <param name="depthAllowance">Depth allowance from the next <see cref="NGUIForm"/>.</param>
        public void ShowMask(UIFormId uiFormId, int depthAllowance)
        {
            if (MaskIsShowing)
            {
                return;
            }

            IUIGroup uiGroup;
            NGUIForm uiForm = UIUtility.GetUIForm(uiFormId, out uiGroup);
            if (uiForm == null)
            {
                GameFramework.Log.Error("UI form '{0}' not found.", uiFormId);
                return;
            }

            int depth = uiGroup.Depth * NGUIForm.GroupDepthFactor + (uiForm.UIForm.DepthInUIGroup + 1) * NGUIForm.FormDepthFactor - depthAllowance;
            m_TutorialMask.Show(uiForm.CachedTransform, depth);
        }

        /// <summary>
        /// Hide the <see cref="TutorialMask"/>.
        /// </summary>
        public void HideMask()
        {
            if (MaskIsShowing)
            {
                m_TutorialMask.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Show tip for the Joystick.
        /// </summary>
        /// <returns>Whether the operation is successful.</returns>
        public bool ShowJoystickTip()
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.ShowJoystickTip();
            return true;
        }

        /// <summary>
        /// Hide tip for the Joystick.
        /// </summary>
        /// <returns>Whether the operation is successful.</returns>
        public bool HideJoystickTip()
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.HideJoystickTip();
            return true;
        }

        /// <summary>
        /// Show normal tip.
        /// </summary>
        /// <param name="displayData">Display data for the normal tip.</param>
        /// <returns>Whether the operation is successful.</returns>
        public bool ShowNormalTip(TutorialNormalTipDisplayData displayData)
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.ShowNormalTip(displayData);
            return true;
        }

        /// <summary>
        /// Hide normal tip.
        /// </summary>
        /// <returns>Whether the operation is successful.</returns>
        public bool HideNormalTip()
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.HideNormalTip();
            return true;
        }

        /// <summary>
        /// Hide compulsory tip.
        /// </summary>
        /// <returns>Whether the operation is successful.</returns>
        public bool HideCompulsoryTip()
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.HideCompulsoryTip();
            return true;
        }

        /// <summary>
        /// Show compulsory tip.
        /// </summary>
        /// <param name="displayData">Display data for the compulsory tip.</param>
        /// <returns>Whether the operation is successful.</returns>
        public bool ShowCompulsoryTip(TutorialCompulsoryTipDisplayData displayData)
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            m_TutorialMask.ShowCompulsoryTip(displayData);
            return true;
        }

        /// <summary>
        /// Pause game and wait for some widget to be clicked.
        /// </summary>
        /// <param name="widgetPath">The widget path relative to the current UI Form the <see cref="TutorialMask"/> is pointing to.</param>
        /// <param name="hideNormalTipOnResume">Whether to hide the normal tip (if any) on resuming game.</param>
        /// <returns>Whether the operation is successful.</returns>
        public bool PauseGameWaitClick(string widgetPath, bool hideNormalTipOnResume)
        {
            if (!MaskIsShowing)
            {
                return false;
            }

            return m_TutorialMask.PauseGameWaitClick(widgetPath, hideNormalTipOnResume);
        }

        private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
        {
            LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
            if (ne.Name != m_TemplateName)
            {
                return;
            }

            m_Template = ne.Resource as GameObject;
            var go = NGUITools.AddChild(m_ParentNode, m_Template);
            go.name = m_NodeName;
            m_TutorialMask = go.GetComponent<TutorialMask>();
            go.SetActive(false);
        }

        #region MonoBehaviour

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        #endregion MonoBehaviour
    }
}
