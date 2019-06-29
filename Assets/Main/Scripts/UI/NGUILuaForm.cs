using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using LuaInterface;
using GameFramework;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 使用 Lua 脚本的 <see cref="NGUIForm"/>。
    /// </summary>
    public class NGUILuaForm : NGUIForm
    {
        #region Lua methods name for beat events
        private const string UpdateMethodName = "Update";
        private const string FixedUpdateMethodName = "FixedUpdate";
        private const string LateUpdateMethodName = "LateUpdate";
        #endregion Lua methods name for beat events

        [SerializeField]
        private ClientConfig.LuaScriptToPreload[] m_LuaScriptsToPreload = null;

        [SerializeField]
        private LinkedPrefab[] m_LinkedPrefabs = null;

        [SerializeField]
        private string m_LuaMainScriptName = string.Empty;

        [SerializeField]
        private bool m_NeedUpdate = false;

        [SerializeField]
        private bool m_NeedLateUpdate = false;

        [SerializeField]
        private bool m_NeedFixedUpdate = false;

        private HashSet<string> m_WaitingLuaScriptSet = new HashSet<string>();
        private bool m_IsObservingLoadLuaEvents = false;
        private bool m_IsObservingNetworkPackets = false;
        private LuaFormDisplayData m_CachedDisplayData = null;
        private LuaTable m_CachedLuaTable = null;
        private bool m_FirstOnResume = false;

        private Dictionary<string, LuaFunction> m_CachedLuaMethods = new Dictionary<string, LuaFunction>();
        private Dictionary<string, GameObject> m_LinkedPrefabDict = new Dictionary<string, GameObject>();

        // Called by NGUI via reflection.
        public void OnClickButton(string key)
        {
            CallMethodIfExists("OnClickButton", key);
        }

        // Called by Lua.
        public GameObject GetLinkedPrefab(string key)
        {
            GameObject go;
            if (!m_LinkedPrefabDict.TryGetValue(key, out go))
            {
                Log.Warning("Prefab with key '{0}' not found.", key);
            }

            return go;
        }

        // Called by Lua.
        public void SubscribeEvent(int eventId)
        {
            GameEntry.Event.Subscribe((UnityGameFramework.Runtime.EventId)eventId, OnEvent);
        }

        // Called by Lua
        public void UnsubscribeEvent(int eventId)
        {
            GameEntry.Event.Unsubscribe((UnityGameFramework.Runtime.EventId)eventId, OnEvent);
        }

        // Called by Lua.
        public void SetToggleCallback(UIToggle uiToggle)
        {
            uiToggle.onChange.Clear();
            uiToggle.onChange.Add(new EventDelegate(delegate ()
            {
                if (uiToggle == null || uiToggle.gameObject == null)
                {
                    return;
                }

                var key = uiToggle.GetComponent<UIStringKey>();
                if (key == null)
                {
                    return;
                }

                CallMethodIfExists("OnToggle", uiToggle.gameObject, key.Key, uiToggle.value);
            }));
        }

        // Called by Lua.
        public void SetButtonCallback(UIButton uiButton)
        {
            uiButton.onClick.Clear();
            uiButton.onClick.Add(new EventDelegate(delegate ()
            {
                if (uiButton == null || uiButton.gameObject == null)
                {
                    return;
                }

                var key = uiButton.GetComponent<UIStringKey>();
                if (key == null)
                {
                    return;
                }

                CallMethodIfExists("OnClickButton", uiButton.gameObject, key.Key);
            }));
        }

        #region NGUIForm

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            for (int i = 0; i < m_LinkedPrefabs.Length; ++i)
            {
                m_LinkedPrefabDict.Add(m_LinkedPrefabs[i].Key, m_LinkedPrefabs[i].GO);
            }
        }

        protected override void OnOpen(object userData)
        {
            m_FirstOnResume = true;
            m_IsObservingNetworkPackets = false;
            m_IsObservingLoadLuaEvents = false;
            m_CachedDisplayData = userData as LuaFormDisplayData;
            if (m_CachedDisplayData == null)
            {
                m_CachedDisplayData = new LuaFormDisplayData { };
            }

            m_CachedLuaMethods.Clear();
            PrepareWaitingLuaScriptSet();
            LoadLuaScripts();
        }

        protected override void OnResume()
        {
            if (m_FirstOnResume)
            {
                return;
            }

            RealOnResume();
        }

        protected override void OnPause()
        {
            RemoveLuaBeatEvents();
            CallMethodIfExists("OnPause");
            base.OnPause();
        }

        protected override void OnClose(object userData)
        {
            if (m_CachedDisplayData != null)
            {
                m_CachedDisplayData.RootGO = null;
                m_CachedDisplayData = null;
            }

            ClearCachedLuaMethods();

            if (m_IsObservingLoadLuaEvents)
            {
                m_IsObservingLoadLuaEvents = false;
                UnsubscribeLoadLuaEvents();
            }

            if (m_IsObservingNetworkPackets)
            {
                m_IsObservingNetworkPackets = false;
                UnsubscribeNetworkEvents();
            }

            if (m_CachedLuaTable != null)
            {
                CallMethodIfExists("OnClose");
                m_CachedLuaTable.Dispose();
                m_CachedLuaTable = null;
            }

            base.OnClose(userData);
        }

        #endregion NGUIForm

        private LuaFunction GetMethod(string methodName)
        {
            LuaFunction func;
            if (!m_CachedLuaMethods.TryGetValue(methodName, out func))
            {
                func = m_CachedLuaTable.GetLuaFunction(methodName);
            }

            return func;
        }

        private object[] CallMethodIfExists(string methodName, params object[] args)
        {
            if (m_CachedLuaTable == null)
            {
                Log.Warning("Cached Lua table is invalid.");
                return null;
            }

            var func = GetMethod(methodName);
            if (func == null)
            {
                return null;
            }

            var ret = func.Call(m_CachedLuaTable, args);
            return ret;
        }

        private void ClearCachedLuaMethods()
        {
            foreach (var kv in m_CachedLuaMethods)
            {
                kv.Value.Dispose();
            }

            m_CachedLuaMethods.Clear();
        }

        private void RealOnOpen()
        {
            m_CachedLuaTable = GameEntry.Lua.DoScript(m_LuaMainScriptName)[0] as LuaTable;
            base.OnOpen(m_CachedDisplayData);

            if (!m_IsObservingNetworkPackets)
            {
                m_IsObservingNetworkPackets = true;
                SubscribeNetworkEvents();
            }

            m_CachedDisplayData.RootGO = gameObject;
            m_CachedDisplayData.Form = this;
            m_CachedDisplayData.ToggleGroupBaseValue = ToggleGroupBaseValue;
            CallMethodIfExists("OnOpen", m_CachedDisplayData);
        }

        private void RealOnResume()
        {
            base.OnResume();
            CallMethodIfExists("OnResume");
            AddLuaBeatEvents();
        }

        private void PrepareWaitingLuaScriptSet()
        {
            m_WaitingLuaScriptSet.Clear();
            for (int i = 0; i < m_LuaScriptsToPreload.Length; ++i)
            {
                m_WaitingLuaScriptSet.Add(m_LuaScriptsToPreload[i].Name);
            }
        }

        private void LoadLuaScripts()
        {
            if (!m_IsObservingLoadLuaEvents)
            {
                m_IsObservingLoadLuaEvents = true;
                SubscribeLoadLuaEvents();
            }

            for (int i = 0; i < m_LuaScriptsToPreload.Length; ++i)
            {
                GameEntry.Lua.LoadScript(m_LuaScriptsToPreload[i].Name, m_LuaScriptsToPreload[i].Category);
            }
        }

        private void SubscribeLoadLuaEvents()
        {
            GameEntry.Event.Subscribe(EventId.LoadLuaScriptSuccess, OnLoadLuaScriptSuccess);
            GameEntry.Event.Subscribe(EventId.LoadLuaScriptFailure, OnLoadLuaScriptFailure);
        }

        private void UnsubscribeLoadLuaEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.LoadLuaScriptSuccess, OnLoadLuaScriptSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadLuaScriptFailure, OnLoadLuaScriptFailure);
        }

        private void SubscribeNetworkEvents()
        {
            GameEntry.Event.Subscribe(EventId.OperationActivityResponse, OnOperationActivityResponse);
        }

        private void UnsubscribeNetworkEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.OperationActivityResponse, OnOperationActivityResponse);
        }

        private void OnEvent(object sender, GameEventArgs e)
        {
            var eventId = e.Id;
            var eventName = Enum.GetName(typeof(EventId), eventId);
            if (string.IsNullOrEmpty(eventName))
            {
                eventName = Enum.GetName(typeof(UnityGameFramework.Runtime.EventId), eventId);
            }

            if (string.IsNullOrEmpty(eventName))
            {
                Log.Warning("Event ID '{0}' not used.", eventId.ToString());
                return;
            }

            CallMethodIfExists(string.Format("OnEvent_{0}", eventName), e);
        }

        private void OnOperationActivityResponse(object sender, GameEventArgs e)
        {
            var ne = e as OperationActivityResponseEventArgs;
            CallMethodIfExists("OnNetworkResponse", ne.GetResponseData() as Dictionary<string, string>);
        }

        private void OnLoadLuaScriptSuccess(object sender, GameEventArgs e)
        {
            var ne = e as LoadLuaScriptSuccessEventArgs;

            if (!m_WaitingLuaScriptSet.Remove(ne.ScriptName))
            {
                return;
            }

            if (m_WaitingLuaScriptSet.Count > 0)
            {
                return;
            }

            if (m_IsObservingLoadLuaEvents)
            {
                m_IsObservingLoadLuaEvents = false;
                UnsubscribeLoadLuaEvents();
            }

            RealOnOpen();
            RealOnResume();
            m_FirstOnResume = false;
        }

        private void OnLoadLuaScriptFailure(object sender, GameEventArgs e)
        {
            // Empty for now.
        }

        private void AddLuaBeatEvents()
        {
            if (m_NeedUpdate)
            {
                AddUpdateEvent();
            }

            if (m_NeedFixedUpdate)
            {
                AddFixedUpdateEvent();
            }

            if (m_NeedLateUpdate)
            {
                AddLateUpdateEvent();
            }
        }

        private void AddUpdateEvent()
        {
            var func = GetMethod(UpdateMethodName);
            if (func != null)
            {
                GameEntry.Lua.UpdateEvent.Add(func, m_CachedLuaTable);
            }
        }

        private void AddFixedUpdateEvent()
        {
            var func = GetMethod("FixedUpdate");
            if (func != null)
            {
                GameEntry.Lua.FixedUpdateEvent.Add(func, m_CachedLuaTable);
            }
        }

        private void AddLateUpdateEvent()
        {
            var func = GetMethod(LateUpdateMethodName);
            if (func != null)
            {
                GameEntry.Lua.LateUpdateEvent.Add(func, m_CachedLuaTable);
            }
        }

        private void RemoveLuaBeatEvents()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            RemoveUpdateEvent();
            RemoveFixedUpdateEvent();
            RemoveLateUpdateEvent();
        }

        private void RemoveUpdateEvent()
        {
            var func = GetMethod(UpdateMethodName);
            if (func != null)
            {
                GameEntry.Lua.UpdateEvent.Remove(func, m_CachedLuaTable);
            }
        }

        private void RemoveFixedUpdateEvent()
        {
            var func = GetMethod(FixedUpdateMethodName);
            if (func != null)
            {
                GameEntry.Lua.FixedUpdateEvent.Remove(func, m_CachedLuaTable);
            }
        }

        private void RemoveLateUpdateEvent()
        {
            var func = GetMethod(LateUpdateMethodName);
            if (func != null)
            {
                GameEntry.Lua.LateUpdateEvent.Remove(func, m_CachedLuaTable);
            }
        }

        [Serializable]
        private class LinkedPrefab
        {
            public string Key = string.Empty;
            public GameObject GO = null;
        }
    }
}
