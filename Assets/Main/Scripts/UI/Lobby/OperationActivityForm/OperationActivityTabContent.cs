using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 运营活动标签页内容。
    /// </summary>
    [RequireComponent(typeof(BehaviorTree))]
    [RequireComponent(typeof(UIPanel))]
    public class OperationActivityTabContent : MonoBehaviour, IUpdatableUIFragment
    {
        [SerializeField]
        private ExternalBehavior m_ExternalBehavior = null;

        [SerializeField]
        private DataFilterItem[] m_DataFilterItems = null;

        [SerializeField]
        private UILabel m_StartEndTimeLabel = null;

        [SerializeField]
        private UILabel m_DescLabel = null;

        private bool m_FirstOnOpen = true;
        private UIEffectsController m_EffectsController = null;
        private UIInvoke m_LastUIInvoke = null;
        private BehaviorTree m_BehaviorTree = null;
        private bool m_DataChanged = false;
        private UIPanel m_Panel = null;
        private int m_OriginalDepth = 0;
        private Dictionary<string, string> m_DataDict = new Dictionary<string, string>();
        private Dictionary<string, string> m_DataFilter = new Dictionary<string, string>();

        public int OriginalDepth
        {
            get { return m_OriginalDepth; }
        }

        public int Depth
        {
            get { return m_Panel.depth; }
        }

        public UIInvoke LastUIInvoke
        {
            get { return m_LastUIInvoke; }
        }

        public bool DataChanged
        {
            get { return m_DataChanged; }
        }

        public UIEffectsController EffectsController
        {
            get { return m_EffectsController; }
        }

        public UILabel DescLabel
        {
            get { return m_DescLabel; }
        }

        public UILabel StartEndTimeLabel
        {
            get { return m_StartEndTimeLabel; }
        }

        public void ClearLastUIInvoke()
        {
            m_LastUIInvoke = null;
        }

        public void ClearDataChanged()
        {
            m_DataChanged = false;
        }

        public GameObject GetGameObject(string gameObjectPath)
        {
            if (string.IsNullOrEmpty(gameObjectPath))
            {
                Log.Warning("GameObject path is invalid.");
                return null;
            }

            Transform t = gameObject.transform.Find(gameObjectPath);

            if (t == null)
            {
                Log.Warning("Can not find GameObject '{0}'.", gameObjectPath);
                return null;
            }

            return t.gameObject;
        }

        public void OnClickButton(UIInvoke uiInvoke)
        {
            if (m_LastUIInvoke != null)
            {
                return;
            }

            m_LastUIInvoke = uiInvoke;
        }

        public void OnOpen()
        {
            if (m_FirstOnOpen)
            {
                UIUtility.ReplaceDictionaryTextForLabels(gameObject, m_StartEndTimeLabel, m_DescLabel);
                m_FirstOnOpen = false;
            }

            m_LastUIInvoke = null;
            m_DataDict.Clear();
            if (m_EffectsController != null) m_EffectsController.Resume();
            GameEntry.Event.Subscribe(EventId.OperationActivityResponse, OnOperationActivityResponse);
            m_BehaviorTree.ExternalBehavior = m_ExternalBehavior;
            GameEntry.Waiting.StartWaiting(WaitingType.Default, "BehaviorDesigner");
            m_BehaviorTree.EnableBehavior();
        }

        public void OnClose()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.OperationActivityResponse, OnOperationActivityResponse);
            }
            m_DataDict.Clear();
            m_DataFilter.Clear();
            if (m_EffectsController != null) m_EffectsController.Pause();
            m_BehaviorTree.DisableBehavior();
            m_BehaviorTree.ExternalBehavior = null;
            m_LastUIInvoke = null;
        }

        public bool TryGetData(string key, out string value)
        {
            return m_DataDict.TryGetValue(key, out value);
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_Panel = GetComponent<UIPanel>();
            m_OriginalDepth = m_Panel.depth;
            m_EffectsController = GetComponent<UIEffectsController>();
            m_BehaviorTree = GetComponent<BehaviorTree>();

            for (int i = 0; i < m_DataFilterItems.Length; ++i)
            {
                DataFilterItem dataFilterItem = m_DataFilterItems[i];
                m_DataDict.Add(dataFilterItem.Key, dataFilterItem.Value);
            }
        }

        private void OnDestroy()
        {
            m_EffectsController = null;
        }

        #endregion MonoBehaviour

        private void OnOperationActivityResponse(object sender, GameEventArgs e)
        {
            var ne = e as OperationActivityResponseEventArgs;
            var responseData = ne.GetResponseData();

            foreach (var kv in m_DataFilter)
            {
                if (!responseData.ContainsKey(kv.Key) || responseData[kv.Key] != m_DataFilter[kv.Key])
                {
                    return;
                }
            }

            foreach (var kv in responseData)
            {
                m_DataDict[kv.Key] = responseData[kv.Key];
            }

            m_DataChanged = true;
        }

        [Serializable]
        private class DataFilterItem
        {
            public string Key = string.Empty;
            public string Value = string.Empty;
        }
    }
}
