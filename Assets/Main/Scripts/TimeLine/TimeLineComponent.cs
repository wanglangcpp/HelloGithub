using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴组件。
    /// </summary>
    public class TimeLineComponent : MonoBehaviour
    {
        private IDictionary<string, GameFrameworkFunc<TimeLineData, bool>> m_TimeLineBuilders = null;
        private IDictionary<string, GameFrameworkFunc<string, TimeLineActionData>> m_ActionDataBuilders = null;
        private ITimeLineManager<Entity> m_EntityTimeLineManager = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks = null;

        /// <summary>
        /// 角色时间轴。
        /// </summary>
        public ITimeLineManager<Entity> Entity
        {
            get
            {
                return m_EntityTimeLineManager;
            }
        }

        public void DestroyTimeLineInstance(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_EntityTimeLineManager.DestroyTimeLineInstance(timeLineInstance);
        }

        public void DestroyTimeLineInstances(Entity timeLineInstanceOwner)
        {
            m_EntityTimeLineManager.DestroyTimeLineInstances(timeLineInstanceOwner);
        }

        private void Awake()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadTimeLineGroupSuccessHandler, LoadTimeLineGroupFailureHandler);
            m_TimeLineBuilders = new Dictionary<string, GameFrameworkFunc<TimeLineData, bool>>();
            m_ActionDataBuilders = new Dictionary<string, GameFrameworkFunc<string, TimeLineActionData>>();
            m_EntityTimeLineManager = TimeLineCreator.CreateTimeLineManager<Entity>();
            m_TimeLineBuilders["EntityTimeLine"] = BuildEntityTimeLine;
            m_ActionDataBuilders["EntityTimeLine"] = EntityTimeLineActionFactory.CreateData;
            Log.Info("Time line component has been initialized.");
        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
            m_EntityTimeLineManager.Shutdown();
            m_EntityTimeLineManager = null;
        }

        private void Update()
        {
            float elapseSeconds = Time.deltaTime;
            float realElapseSeconds = Time.unscaledDeltaTime;

            m_EntityTimeLineManager.Update(elapseSeconds, realElapseSeconds);
        }

        private void OnDrawGizmos()
        {
            if (m_EntityTimeLineManager != null)
            {
                m_EntityTimeLineManager.DebugDraw();
            }
        }

        public void ClearAllTimeInstances()
        {
            m_EntityTimeLineManager.ClearAllTimeInstances();
        }

        public void LoadTimeLineGroup(string timeLineName, object userData = null)
        {
            if (string.IsNullOrEmpty(timeLineName))
            {
                Log.Error("Time line name is invalid.");
                return;
            }

            Dictionary<string, object> internalUserData = new Dictionary<string, object> { { "TimeLineName", timeLineName }, { "UserData", userData } };
            GameEntry.Resource.LoadAsset(AssetUtility.GetTimeLineAsset(timeLineName), m_LoadAssetCallbacks, internalUserData);
        }

        private string LoadTimeLineGroup(object timeLineResource, object userData)
        {
            Dictionary<string, object> hashtable = userData as Dictionary<string, object>;
            string timeLineName = hashtable["TimeLineName"] as string;

            TextAsset textAsset = timeLineResource as TextAsset;
            if (textAsset == null)
            {
                return string.Format("Time line '{0}' resource is invalid.", timeLineName);
            }

            var timeLineGroupData = new TimeLineGroupData(timeLineName, m_ActionDataBuilders[timeLineName]);
            string errorMessage = timeLineGroupData.ParseData(textAsset.text);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }

            for (int i = 0; i < timeLineGroupData.TimeLines.Count; ++i)
            {
                var timeLineData = timeLineGroupData.TimeLines[i];
                if (!BuildTimeLine(timeLineName, timeLineData))
                {
                    return string.Format("Can not build time line '{0}' when time line id '{1}', maybe duplicated.", timeLineName, timeLineData.Id);
                }
            }

            return string.Empty;
        }

        private bool BuildTimeLine(string timeLineName, TimeLineData timeLineData)
        {
            return m_TimeLineBuilders[timeLineName](timeLineData);
        }

        private bool BuildEntityTimeLine(TimeLineData timeLineData)
        {
            ITimeLine<Entity> timeLine = TimeLineCreator.CreateTimeLine<Entity>(timeLineData.Id);
            for (int i = 0; i < timeLineData.Actions.Count; i++)
            {
                try
                {
                    var actionData = timeLineData.Actions[i];
                    timeLine.AddAction(EntityTimeLineActionFactory.Create(actionData));
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Can not add action at line '{0}' with exception '{1}'.", i.ToString(), exception.Message), exception);
                }
            }

            return m_EntityTimeLineManager.AddTimeLine(timeLine);
        }

        private void LoadTimeLineGroupSuccessHandler(string timeLineAssetName, object timeLineAsset, float duration, object userData)
        {
            Dictionary<string, object> hashtable = userData as Dictionary<string, object>;
            string errorMessage = LoadTimeLineGroup(timeLineAsset, userData);

            GameEntry.Resource.Recycle(timeLineAsset);
            Log.Info("Recycle time line group '{0}'.", timeLineAssetName);

            if (string.IsNullOrEmpty(errorMessage))
            {
                GameEntry.Event.Fire(this, new LoadTimeLineGroupSuccessEventArgs(hashtable["TimeLineName"] as string, timeLineAssetName, hashtable["UserData"]));
            }
            else
            {
                GameEntry.Event.Fire(this, new LoadTimeLineGroupFailureEventArgs(hashtable["TimeLineName"] as string, timeLineAssetName, errorMessage, hashtable["UserData"]));
            }
        }

        private void LoadTimeLineGroupFailureHandler(string timeLineAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Dictionary<string, object> hashtable = userData as Dictionary<string, object>;
            string timeLineName = hashtable["TimeLineName"] as string;
            Log.Warning("Load time line '{0}' failed, resource name '{1}', status '{2}', error message '{3}'.", timeLineName, timeLineAssetName, status.ToString(), errorMessage);

            GameEntry.Event.Fire(this, new LoadTimeLineGroupFailureEventArgs(timeLineName, timeLineAssetName, errorMessage, hashtable["UserData"]));
        }
    }
}
