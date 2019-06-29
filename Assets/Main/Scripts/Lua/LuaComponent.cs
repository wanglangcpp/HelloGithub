using GameFramework;
using GameFramework.Resource;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Lua 组件。
    /// </summary>
    public class LuaComponent : MonoBehaviour, ILuaComponent
    {
        private const string AssetPathPrefixRegex = @"^Assets";
        private const string TextAssetSuffix = ".bytes";

        private LuaState m_Lua = null;
        private LuaLooper m_Looper = null;

        private IDictionary<string, LuaScript> m_ScriptNamesToScripts = new Dictionary<string, LuaScript>();

        private LoadAssetCallbacks m_LoadAssetCallbacks = null;

#if UNITY_EDITOR
        private bool m_CachedDebuggerUseLog = false;
        private LuaInterface.ILogger m_CachedDebuggerLogger = null;
#endif

        public LuaBeatEvent UpdateEvent
        {
            get
            {
                CheckLuaLooper();
                return m_Looper.UpdateEvent;
            }
        }

        public LuaBeatEvent LateUpdateEvent
        {
            get
            {
                CheckLuaLooper();
                return m_Looper.LateUpdateEvent;
            }
        }

        public LuaBeatEvent FixedUpdateEvent
        {
            get
            {
                CheckLuaLooper();
                return m_Looper.FixedUpdateEvent;
            }
        }

        #region ILuaComponent

        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = m_Lua.GetFunction(funcName);
            if (func == null)
            {
                return null;
            }

            return func.Call(args);
        }

        public bool HasScript(string scriptName)
        {
            return m_ScriptNamesToScripts.ContainsKey(scriptName);
        }

        public string GetScriptContent(string scriptName)
        {
            return m_ScriptNamesToScripts[scriptName].Content;
        }

        public void Require(string scriptName)
        {
            m_Lua.Require(scriptName);
        }

        public void StartVM()
        {
            StartLuaState();
            AddLuaLooper();
        }

        public object[] DoScript(string scriptContent, string chunkName)
        {
            if (string.IsNullOrEmpty(chunkName))
            {
                return m_Lua.DoString(scriptContent);
            }

            return m_Lua.DoString(scriptContent, chunkName);
        }

        public object[] DoScript(string scriptName)
        {
            LuaScript script;

            if (!m_ScriptNamesToScripts.TryGetValue(scriptName, out script))
            {
                Log.Error("Script '{0}' not loaded yet.", scriptName);
                return null;
            }
            return DoScript(script.Content, scriptName);
        }

        public void GC()
        {
            m_Lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void LoadScript(string scriptName, LuaScriptCategory category, object userData = null)
        {
            string assetName = AssetUtility.GetLuaAsset(scriptName, category);
            LuaScript script;

            if (m_ScriptNamesToScripts.TryGetValue(scriptName, out script))
            {
                OnLoadScriptSuccess(scriptName, script.Content, userData);
                return;
            }

            if (!GameEntry.Base.EditorResourceMode)
            {
                GameEntry.Resource.LoadAsset(assetName + TextAssetSuffix, m_LoadAssetCallbacks,
                    new LoadLuaScriptUserData { ScriptName = scriptName, ScriptCategory = category, UserData = userData });
                return;
            }

            // Local resource mode: Simply read Lua script synchronously from the file system.
            var fullPath = Regex.Replace(assetName, AssetPathPrefixRegex, Application.dataPath);
            var content = File.ReadAllText(fullPath);
            m_ScriptNamesToScripts[scriptName] = new LuaScript { Name = scriptName, Content = content, Category = category };
            OnLoadScriptSuccess(scriptName, content, userData);
        }

        #endregion ILuaComponent

        private void OnLoadScriptFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            var myUserData = userData as LoadLuaScriptUserData;
            GameEntry.Event.Fire(this, new LoadLuaScriptFailureEventArgs(myUserData.ScriptName, myUserData.UserData));
        }

        private void OnLoadScriptSuccess(string assetName, object asset, float duration, object userData)
        {
            var textAsset = asset as TextAsset;
            if (textAsset != null)
            {
                var content = textAsset.text;
                var myUserData = userData as LoadLuaScriptUserData;
                m_ScriptNamesToScripts[myUserData.ScriptName] = new LuaScript { Name = myUserData.ScriptName, Content = content, Category = myUserData.ScriptCategory };
                OnLoadScriptSuccess(myUserData.ScriptName, content, myUserData.UserData);
                return;
            }

            OnLoadScriptFailure(assetName, LoadResourceStatus.TypeError, "Not a TextAsset.", userData);
        }

        private void OnLoadScriptSuccess(string scriptName, string scriptContent, object externalUserData)
        {
            GameEntry.Event.Fire(this, new LoadLuaScriptSuccessEventArgs(scriptName, scriptContent, externalUserData));
        }

        private void InitLuaState()
        {
            new CustomLuaResLoader(this);

#if UNITY_EDITOR
            m_CachedDebuggerUseLog = Debugger.useLog;
            m_CachedDebuggerLogger = Debugger.logger;
#endif

            Debugger.useLog = false;
            Debugger.logger = new LuaLogger();
            m_Lua = new LuaState();
            m_Lua.LuaSetTop(0);
            LuaBinder.Bind(m_Lua);
            LuaCoroutine.Register(m_Lua, this);
        }

        private void StartLuaState()
        {
            m_Lua.Start();
        }

        private void AddLuaLooper()
        {
            m_Looper = gameObject.AddComponent<LuaLooper>();
            m_Looper.luaState = m_Lua;
        }

        private void ClearLuaLooper()
        {
            if (m_Looper == null)
            {
                return;
            }

            m_Looper.Destroy();
            m_Looper = null;
        }

        private void CheckLuaLooper()
        {
            if (m_Looper == null)
            {
                throw new InvalidOperationException("Looper is invalid.");
            }
        }

        private void InitLoadAssetCallbacks()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadScriptSuccess, OnLoadScriptFailure);
        }

        #region MonoBehaviour

        private void Awake()
        {
            InitLoadAssetCallbacks();
            InitLuaState();
        }

        private void OnDestroy()
        {
            ClearLuaLooper();
            ShutdownLuaState();
        }

        private void ShutdownLuaState()
        {
#if UNITY_EDITOR
            Debugger.useLog = m_CachedDebuggerUseLog;
            Debugger.logger = m_CachedDebuggerLogger;
#endif

            m_Lua.Dispose();
            m_Lua = null;
        }

        #endregion MonoBehaviour

        private class LoadLuaScriptUserData
        {
            public string ScriptName { get; set; }
            public LuaScriptCategory ScriptCategory { get; set; }
            public object UserData { get; set; }
        }

        private class LuaScript
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public LuaScriptCategory Category { get; set; }
        }

        private class LuaLogger : LuaInterface.ILogger
        {
            public void Log(string msg, string stack, LogType type)
            {
                if (!string.IsNullOrEmpty(stack))
                {
                    msg += "\n" + stack;
                }

                switch (type)
                {
                    case LogType.Log:
                        LogInfo(msg);
                        break;

                    case LogType.Warning:
                        LogWarning(msg);
                        break;

                    case LogType.Exception:
                        LogException(msg);
                        break;

                    case LogType.Error:
                    default:
                        LogError(msg);
                        break;
                }
            }

            private static void LogError(string msg)
            {
                if (Application.isPlaying)
                {
                    GameFramework.Log.Error(msg);
                }
                else
                {
                    Debug.LogError(msg);
                }
            }

            private static void LogException(string msg)
            {
                if (Application.isPlaying)
                {
                    GameFramework.Log.Fatal(msg);
                }
                else
                {
                    Debug.LogException(new UnityException(msg));
                }
            }

            private static void LogWarning(string msg)
            {
                if (Application.isPlaying)
                {
                    GameFramework.Log.Warning(msg);
                }
                else
                {
                    Debug.LogWarning(msg);
                }
            }

            private static void LogInfo(string msg)
            {
                if (Application.isPlaying)
                {
                    GameFramework.Log.Info(msg);
                }
                else
                {
                    Debug.Log(msg);
                }
            }
        }
    }
}
