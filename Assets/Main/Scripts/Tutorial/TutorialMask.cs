using System;
using GameFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Genesis.GameClient
{
    /// <summary>
    /// Tutorial mask. Capable of showing a tip for joystick and a text tip in front of some <see cref="NGUIForm"/>.
    /// </summary>
    /// <remarks>
    public class TutorialMask : MonoBehaviour
    {
        private const float MinScale = 1e-6f;

        [SerializeField]
        private GameObject m_JoystickTip = null;

        private UITexture m_JoystickTexture = null;
        private Animation m_JoystickTipAnim = null;

        [SerializeField]
        private GameObject m_ArrowRoot = null;

        private Transform m_ArrowTransform = null;

        [SerializeField]
        private UIWidget m_ArrowRootWidget = null;

        [SerializeField]
        private Animation m_ArrowAnim = null;

        [SerializeField]
        private GameObject m_FixedPositionTipTextRoot = null;

        [SerializeField]
        private UILabel m_FixedPositionTipText = null;

        [SerializeField]
        private GameObject m_FloatTipTextRoot = null;

        [SerializeField]
        private UILabel m_FloatTipText = null;

        [SerializeField]
        private UISprite m_FloatTipTextBg = null;

        [SerializeField]
        private GameObject m_ScreenBlocker = null;

        [SerializeField]
        private UISprite m_ScreenMask = null;

        [SerializeField]
        private GameObject[] m_JoystickTipNodes = null;

        [SerializeField]
        private GameObject[] m_NormalTipNodes = null;

        [SerializeField]
        private GameObject[] m_CompulsoryTipNodes = null;

        [SerializeField]
        private int m_CopiedWidgetMinDepth = 1;

        private UIEffectsController m_EffectsController = null;
        private UIPanel m_Panel = null;
        private UIRoot m_UIRoot = null;
        private Rect? m_PendingJoystickTipRect = null;
        private Transform m_UIFormRoot = null;
        private List<GameObject> m_GOsWaitingForClick = null;
        private HashSet<GameObject> m_AllNodes = null;
        private List<GameObject> m_GOsForCompulsoryTipInteraction = null;

        private float m_ArrowAnimTime = 0f;
        private float m_JoystickAnimTime = 0f;
        private bool m_HideNormalTipOnResume = false;
        private bool m_HideCompulsoryTipOnResume = false;

        public bool IsShowing { get { return gameObject.activeSelf; } }

        /// <summary>
        /// Show.
        /// </summary>
        /// <param name="uiFormRoot">Root node of the corresponding <see cref="NGUIForm"/>.</param>
        /// <param name="depth">Target depth of the panel.</param>
        public void Show(Transform uiFormRoot, int depth)
        {
            gameObject.SetActive(true);
            m_Panel.depth = depth;
            m_UIFormRoot = uiFormRoot;
        }

        /// <summary>
        /// Hide.
        /// </summary>
        public void Hide()
        {
            m_UIFormRoot = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Show tip for the Joystick.
        /// </summary>
        public void ShowJoystickTip()
        {
            ShowJoystickTip(GameEntry.Tutorial.Config.JoystickTipRect);
        }

        /// <summary>
        /// Show tip for the Joystick.
        /// </summary>
        /// <param name="rect">Rect from the bottom-left of the screen.</param>
        public void ShowJoystickTip(Rect rect)
        {
            if (m_JoystickTip.activeSelf)
            {
                return;
            }

            if (m_UIRoot == null)
            {
                m_PendingJoystickTipRect = rect;
                return;
            }

            if (m_PendingJoystickTipRect.HasValue)
            {
                m_PendingJoystickTipRect = null;
            }

            m_JoystickTip.SetActive(true);
            m_JoystickTexture.SetRect(rect.x - (float)Screen.width / Screen.height * m_UIRoot.manualHeight * .5f, rect.y - m_UIRoot.manualHeight * .5f, rect.width, rect.height);
            m_JoystickAnimTime = 0f;
        }

        /// <summary>
        /// Hide normal tip.
        /// </summary>
        public void HideNormalTip()
        {
            HideTipNodes(m_NormalTipNodes);
        }

        /// <summary>
        /// Hide compulsory tip.
        /// </summary>
        public void HideCompulsoryTip()
        {
            HideTipNodes(m_CompulsoryTipNodes);
            DestroyGOsForCompulsoryTipInteraction(true);
        }

        /// <summary>
        /// Hide tip for the Joystick.
        /// </summary>
        public void HideJoystickTip()
        {
            HideTipNodes(m_JoystickTipNodes);
        }

        /// <summary>
        /// Hide all tips.
        /// </summary>
        public void HideAllTips()
        {
            HideTipNodes(m_AllNodes);
            DestroyGOsForCompulsoryTipInteraction();
        }

        /// <summary>
        /// Show normal tip.
        /// </summary>
        /// <param name="displayData">Display data for the normal tip.</param>
        /// <param name="hideCompulsory">Whether to hide compulsory tip first.</param>
        public bool ShowNormalTip(TutorialNormalTipDisplayData displayData, bool hideCompulsory = true)
        {
            if (hideCompulsory)
            {
                HideCompulsoryTip();
            }

            // Nothing at all.
            if (string.IsNullOrEmpty(displayData.TextKey) && string.IsNullOrEmpty(displayData.WidgetPath))
            {
                Log.Error("You're trying to display an empty tip, which has no text or widget indicator.");
                return false;
            }

            // Text only.
            if (string.IsNullOrEmpty(displayData.WidgetPath))
            {
                ShowFixedPositionTipText(displayData);
                return true;
            }

            // Should indicate some widget.
            Rect widgetRect;
            if (!TryGetWidgetRect(displayData, out widgetRect))
            {
                Log.Error("Widget on path '{0}' is invalid.", displayData.WidgetPath);
                return false;
            }

            ShowArrow(displayData, widgetRect);

            if (string.IsNullOrEmpty(displayData.TextKey))
            {
                m_FixedPositionTipTextRoot.SetActive(false);
                m_FloatTipTextRoot.SetActive(false);
            }
            else if (displayData.TextCategory == TutorialTipTextCategory.FixedPosition)
            {
                ShowFixedPositionTipText(displayData);
            }
            else
            {
                ShowFloatTipText(displayData, widgetRect);
            }

            return true;
        }

        /// <summary>
        /// Show compulsory tip.
        /// </summary>
        /// <param name="displayData">Display data for the compulsory tip.</param>
        public bool ShowCompulsoryTip(TutorialCompulsoryTipDisplayData displayData)
        {
            HideNormalTip();

            // Show normal part of the tip.
            if (!ShowNormalTip(displayData, false)) return false;

            m_ScreenBlocker.SetActive(true);
            m_ScreenMask.gameObject.SetActive(displayData.MaskVisible);

            string[] widgetPaths = displayData.WidgetPathsToInteract;

            for (int i = 0; i < widgetPaths.Length; i++)
            {
                string widgetPath = widgetPaths[i];
                Transform t = m_UIFormRoot.Find(widgetPath);
                if (t == null)
                {
                    Log.Error("Cannot find widget at path '{0}'.", widgetPath);
                    return false;
                }

                var newGO = NGUITools.AddChild(m_Panel.gameObject, t.gameObject);
                var newTrans = newGO.transform;
                newTrans.position = t.position;
                newTrans.rotation = t.rotation;
                var localScale = t.lossyScale;

                for (Transform parent = newTrans.parent; parent != null; parent = parent.parent)
                {
                    if (Mathf.Abs(parent.localScale.x) > MinScale) localScale.x /= parent.localScale.x;
                    if (Mathf.Abs(parent.localScale.y) > MinScale) localScale.y /= parent.localScale.y;
                    if (Mathf.Abs(parent.localScale.z) > MinScale) localScale.z /= parent.localScale.z;
                }

                newTrans.localScale = localScale;

                UpdateWidgetDepths(newGO);
                m_GOsForCompulsoryTipInteraction.Add(newGO);

                if (displayData.PauseGame)
                {
                    m_GOsWaitingForClick.Add(newGO);
                }
            }

            if (displayData.PauseGame)
            {
                m_HideCompulsoryTipOnResume = displayData.HideOnResume;
                GameEntry.TimeScale.PauseGame();
            }

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
            Transform t = m_UIFormRoot.Find(widgetPath);
            if (t == null)
            {
                Log.Info("Widget on path '{0}' is invalid.", widgetPath);
                return false;
            }

            m_GOsWaitingForClick.Add(t.gameObject);
            m_HideNormalTipOnResume = hideNormalTipOnResume;
            GameEntry.TimeScale.PauseGame();
            return true;
        }

        private void UpdateWidgetDepths(GameObject newGO)
        {
            var widgets = newGO.GetComponentsInChildren<UIWidget>(true);
            int minDepth = int.MaxValue;
            for (int j = 0; j < widgets.Length; j++)
            {
                if (widgets[j].depth < minDepth)
                {
                    minDepth = widgets[j].depth;
                }
            }

            for (int j = 0; j < widgets.Length; j++)
            {
                widgets[j].depth = widgets[j].depth - minDepth + m_CopiedWidgetMinDepth;
            }
        }

        private void DestroyGOsForCompulsoryTipInteraction(bool waitAFrame = false)
        {
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(DestroyGOsForCompulsoryTipInteractionCo(waitAFrame));
        }

        private IEnumerator DestroyGOsForCompulsoryTipInteractionCo(bool waitEndOfFrame)
        {
            if (waitEndOfFrame)
            {
                yield return null;
            }

            for (int i = 0; i < m_GOsForCompulsoryTipInteraction.Count; i++)
            {
                Destroy(m_GOsForCompulsoryTipInteraction[i]);
            }

            m_GOsForCompulsoryTipInteraction.Clear();
            yield break;
        }

        private void ShowFixedPositionTipText(TutorialNormalTipDisplayData displayData)
        {
            m_FixedPositionTipTextRoot.SetActive(true);
            m_FloatTipTextRoot.SetActive(false);
            m_FixedPositionTipText.text = GameEntry.Localization.GetString(displayData.TextKey);
        }

        private void ShowFloatTipText(TutorialNormalTipDisplayData displayData, Rect widgetRect)
        {
            m_FloatTipTextRoot.SetActive(true);
            m_FixedPositionTipTextRoot.SetActive(false);
            m_FloatTipText.text = GameEntry.Localization.GetString(displayData.TextKey);

            m_FloatTipTextBg.pivot = displayData.TextBgPivot;
            Transform rootTransform = m_FloatTipTextRoot.transform;

            float x = widgetRect.center.x + displayData.TextOffsetRatio.x * widgetRect.width + displayData.TextOffsetPixels.x;
            float y = widgetRect.center.y + displayData.TextOffsetRatio.y * widgetRect.height + displayData.TextOffsetPixels.y;
            rootTransform.localPosition = new Vector3(x, y, rootTransform.localPosition.z);
        }

        private bool TryGetWidgetRect(TutorialNormalTipDisplayData displayData, out Rect rect)
        {
            rect = new Rect();
            Transform widgetTransform = m_UIFormRoot.Find(displayData.WidgetPath);

            if (widgetTransform == null)
            {
                return false;
            }

            var widget = widgetTransform.GetComponent<UIWidget>();

            if (widget == null)
            {
                return false;
            }

            var corners = widget.worldCorners;
            corners[0] = m_Panel.cachedTransform.InverseTransformPoint(corners[0]); // bottom-left
            corners[2] = m_Panel.cachedTransform.InverseTransformPoint(corners[2]); // top-right

            rect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
            return true;
        }

        private void ShowArrow(TutorialNormalTipDisplayData displayData, Rect rect)
        {
            m_ArrowRoot.SetActive(true);
            m_ArrowTransform.localEulerAngles = new Vector3(0f, 0f, 90f * (int)displayData.ArrowDir);
            switch (displayData.ArrowDir)
            {
                case TutorialTipArrowDir.Right:
                    m_ArrowTransform.localPosition = new Vector3(rect.x - m_ArrowRootWidget.width * .5f - displayData.ArrowOffset, rect.center.y, m_ArrowTransform.localPosition.z);
                    break;
                case TutorialTipArrowDir.Left:
                    m_ArrowTransform.localPosition = new Vector3(rect.xMax + m_ArrowRootWidget.width * .5f + displayData.ArrowOffset, rect.center.y, m_ArrowTransform.localPosition.z);
                    break;
                case TutorialTipArrowDir.Down:
                    m_ArrowTransform.localPosition = new Vector3(rect.center.x, rect.yMax + m_ArrowRootWidget.height * .5f + displayData.ArrowOffset, m_ArrowTransform.localPosition.z);
                    break;
                case TutorialTipArrowDir.Up:
                default:
                    m_ArrowTransform.localPosition = new Vector3(rect.center.x, rect.y - m_ArrowRootWidget.height * .5f - displayData.ArrowOffset, m_ArrowTransform.localPosition.z);
                    break;
            }

            m_ArrowAnimTime = 0f;
        }

        private void SampleArrowAnim()
        {
            SampleAnim(m_ArrowAnim, m_ArrowRoot, ref m_ArrowAnimTime);
        }

        private void SampleJoystickAnim()
        {
            SampleAnim(m_JoystickTipAnim, m_JoystickTip, ref m_JoystickAnimTime);
        }

        private void SampleAnim(Animation anim, GameObject root, ref float time)
        {
            if (!root.activeSelf)
            {
                return;
            }

            anim.clip.SampleAnimation(anim.gameObject, time);

            time += Time.unscaledDeltaTime;
            if (time > anim.clip.length)
            {
                time %= anim.clip.length;
            }
        }

        private void OnClick_UICamera(GameObject go)
        {
            if (!m_GOsWaitingForClick.Contains(go))
            {
                return;
            }

            m_GOsWaitingForClick.Clear();
            GameEntry.TimeScale.ResumeGame();

            if (m_HideNormalTipOnResume)
            {
                HideNormalTip();
                m_HideNormalTipOnResume = false;
            }

            if (m_HideCompulsoryTipOnResume)
            {
                HideCompulsoryTip();
                m_HideCompulsoryTipOnResume = false;
            }
        }

        private void CacheNodes()
        {
            m_AllNodes = new HashSet<GameObject>();
            for (int i = 0; i < m_JoystickTipNodes.Length; i++)
            {
                m_AllNodes.Add(m_JoystickTipNodes[i]);
            }

            for (int i = 0; i < m_NormalTipNodes.Length; i++)
            {
                m_AllNodes.Add(m_NormalTipNodes[i]);
            }

            for (int i = 0; i < m_CompulsoryTipNodes.Length; i++)
            {
                m_AllNodes.Add(m_CompulsoryTipNodes[i]);
            }

            HideAllTips();
        }

        private static void HideTipNodes(IEnumerable<GameObject> tipNodes)
        {
            if (tipNodes == null)
            {
                return;
            }

            if (tipNodes is IList<GameObject>)
            {
                var tipNodeArray = tipNodes as IList<GameObject>;
                for (int i = 0; i < tipNodeArray.Count; i++)
                {
                    var go = tipNodeArray[i];
                    if (go == null)
                    {
                        continue;
                    }

                    go.SetActive(false);
                }

                return;
            }

            foreach (var go in tipNodes)
            {
                if (go == null)
                {
                    continue;
                }

                go.SetActive(false);
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_EffectsController = GetComponent<UIEffectsController>();

            m_JoystickTipAnim = m_JoystickTip.GetComponent<Animation>();
            m_JoystickTexture = m_JoystickTip.GetComponent<UITexture>();
            m_ArrowTransform = m_ArrowRoot.transform;
            m_Panel = GetComponent<UIPanel>();

            m_GOsWaitingForClick = new List<GameObject>();
            m_GOsForCompulsoryTipInteraction = new List<GameObject>();

            CacheNodes();
        }

        private void Start()
        {
            m_UIRoot = GetComponentInParent<UIRoot>();

            if (m_PendingJoystickTipRect.HasValue)
            {
                ShowJoystickTip(m_PendingJoystickTipRect.Value);
            }
        }

        private void OnEnable()
        {
            m_EffectsController.Resume();
            UICamera.onClick += OnClick_UICamera;
        }

        private void OnDisable()
        {
            HideAllTips();
            m_GOsWaitingForClick.Clear();
            UICamera.onClick -= OnClick_UICamera;
            m_EffectsController.Pause();
        }

        private void Update()
        {
            SampleArrowAnim();
            SampleJoystickAnim();
        }

        private void OnDestroy()
        {
            m_AllNodes.Clear();
        }

        #endregion MonoBehaviour
    }
}
