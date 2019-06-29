using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 等待组件。
    /// </summary>
    public class WaitingComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_WaitingTemplate = null;

        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Waiting";

        private Waiting m_WaitingPanel = null;
        private int m_WaitingCount = 0;
        private IDictionary<WaitingType, int> m_CategorizedWaitingCounts;
        private IDictionary<WaitingType, IDictionary<string, int>> m_TypesAndKeysToCounts;

        #region MonoBehaviour

        private void Awake()
        {
            m_WaitingPanel = NGUITools.AddChild(m_ParentNode, m_WaitingTemplate).GetComponent<Waiting>();
            m_WaitingPanel.name = m_NodeName;
            m_WaitingPanel.gameObject.SetActive(false);

            m_CategorizedWaitingCounts = new Dictionary<WaitingType, int>();
            m_TypesAndKeysToCounts = new Dictionary<WaitingType, IDictionary<string, int>>();
            var waitingTypes = System.Enum.GetValues(typeof(WaitingType));
            foreach (var waitingType in waitingTypes)
            {
                m_CategorizedWaitingCounts.Add((WaitingType)waitingType, 0);
                m_TypesAndKeysToCounts.Add((WaitingType)waitingType, new Dictionary<string, int>());
            }
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUISuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormFailure, OnOpenUIFailure);
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUISuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormFailure, OnOpenUIFailure);
            }
        }

        #endregion MonoBehaviour

        /// <summary>
        /// 获取详细等待计数信息。
        /// </summary>
        /// <remarks>用于调试。</remarks>
        /// <returns>详细等待计数信息。</returns>
        public IDictionary<WaitingType, IDictionary<string, int>> GetDetailedWaitingCounts()
        {
            return m_TypesAndKeysToCounts;
        }

        public void StartWaiting(WaitingType waitingType, string key)
        {
            m_WaitingCount++;
            m_CategorizedWaitingCounts[waitingType]++;
            if (m_TypesAndKeysToCounts[waitingType].ContainsKey(key))
            {
                m_TypesAndKeysToCounts[waitingType][key]++;
            }
            else
            {
                m_TypesAndKeysToCounts[waitingType].Add(key, 1);
            }

            //Log.Info("[WaitingComponent StartWaiting] waiting type is {0}; waiting count is {1}; key is {2}.", waitingType.ToString(), m_WaitingCount.ToString(), key);
            Refresh();
        }

        public void StopWaiting(WaitingType waitingType, string key)
        {
            m_WaitingCount--;
            m_CategorizedWaitingCounts[waitingType]--;
            if (m_TypesAndKeysToCounts[waitingType].ContainsKey(key))
            {
                m_TypesAndKeysToCounts[waitingType][key]--;
            }
            else
            {
                m_TypesAndKeysToCounts[waitingType].Add(key, -1);
            }

            //Log.Info("[WaitingComponent StopWaiting] waiting type is {0}; waiting count is {1}; key is {2}.", waitingType.ToString(), m_WaitingCount.ToString(), key);
            Refresh();
        }

        public void ClearWaitingOfType(WaitingType waitingType)
        {
            int currentCount = m_CategorizedWaitingCounts[waitingType];
            m_CategorizedWaitingCounts[waitingType] = 0;
            m_WaitingCount -= currentCount;
            m_TypesAndKeysToCounts[waitingType].Clear();

            //Log.Info("[WaitingComponent ClearWaitingOfType] waiting type is {0}; waiting count is {1}.", waitingType.ToString(), m_WaitingCount.ToString());
            Refresh();
        }

        private void Refresh()
        {
            if (m_WaitingCount > 0)
            {
                m_WaitingPanel.FadeIn();
            }
            else
            {
                m_WaitingPanel.FadeOut();
            }
        }

        private void OnOpenUISuccess(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
            StopWaiting(WaitingType.Default, ((UIFormId)ne.UIForm.TypeId).ToString());
        }

        private void OnOpenUIFailure(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.OpenUIFormFailureEventArgs;
            StopWaiting(WaitingType.Default, ((UIFormId)ne.UIFormTypeId).ToString());
        }
    }
}
