using GameFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using System.Collections;
using System;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.GameClient
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public sealed partial class GameEntry : MonoBehaviour
    {
        [SerializeField]
        private bool m_TreatWarningsAsErrors = true;

#if UNITY_EDITOR

        [SerializeField]
        private bool m_FrameCountLog = false;

#endif
        private static GameEntry s_Instance_;

        private static GameEntry s_Instance
        {
            get
            {
                return s_Instance_;
            }
            set
            {
                s_Instance_ = value;
            }
        }

        //#if UNITY_EDITOR
        //        private EditorResourceManager m_EditorResourceManager = null;
        //#endif

        private BaseComponent m_Base = null;
        private DataTableComponent m_DataTable = null;
        private DebuggerComponent m_Debugger = null;
        private DownloadComponent m_Download = null;
        private EntityComponent m_Entity = null;
        private EventComponent m_Event = null;
        private FsmComponent m_Fsm = null;
        private LocalizationComponent m_Localization = null;
        private NetworkComponent m_Network = null;
        private ObjectPoolComponent m_ObjectPool = null;
        private ProcedureComponent m_Procedure = null;
        private ResourceComponent m_Resource = null;
        private SceneComponent m_Scene = null;
        private SettingComponent m_Setting = null;
        private SoundComponent m_Sound = null;
        private UIComponent m_UI = null;
        private WebRequestComponent m_WebRequest = null;

        private string[] m_LogWhiteList = null;
        private BuildInfo m_BuildInfo = new BuildInfo();

        private bool m_IsShuttingDown = false;

        /// <summary>
        /// 游戏入口是否可用。
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                return s_Instance != null;
            }
        }

        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static BaseComponent Base
        {
            get
            {
                return s_Instance.m_Base;
            }
        }

        /// <summary>
        /// 获取数据表组件。
        /// </summary>
        public static DataTableComponent DataTable
        {
            get
            {
                return s_Instance.m_DataTable;
            }
        }

        /// <summary>
        /// 获取调试组件。
        /// </summary>
        public static DebuggerComponent Debugger
        {
            get
            {
                return s_Instance.m_Debugger;
            }
        }

        /// <summary>
        /// 获取下载组件。
        /// </summary>
        public static DownloadComponent Download
        {
            get
            {
                return s_Instance.m_Download;
            }
        }

        /// <summary>
        /// 获取实体组件。
        /// </summary>
        public static EntityComponent Entity
        {
            get
            {
                return s_Instance.m_Entity;
            }
        }

        /// <summary>
        /// 获取事件组件。
        /// </summary>
        public static EventComponent Event
        {
            get
            {
                if (null == s_Instance)//restart时会有空引用，只能这样写了
                    return null;
                return s_Instance.m_Event;
            }
        }

        /// <summary>
        /// 获取有限状态机组件。
        /// </summary>
        public static FsmComponent Fsm
        {
            get
            {
                return s_Instance.m_Fsm;
            }
        }

        /// <summary>
        /// 获取本地化组件。
        /// </summary>
        public static LocalizationComponent Localization
        {
            get
            {
                return s_Instance.m_Localization;
            }
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static NetworkComponent Network
        {
            get
            {
                return s_Instance.m_Network;
            }
        }

        /// <summary>
        /// 获取对象池组件。
        /// </summary>
        public static ObjectPoolComponent ObjectPool
        {
            get
            {
                if (null == s_Instance)//restart时会有空引用，只能这样写了
                    return null;
                return s_Instance.m_ObjectPool;
            }
        }

        /// <summary>
        /// 获取流程组件。
        /// </summary>
        public static ProcedureComponent Procedure
        {
            get
            {
                if (null == s_Instance)
                    return null;
                return s_Instance.m_Procedure;
            }
        }

        /// <summary>
        /// 获取资源组件。
        /// </summary>
        public static ResourceComponent Resource
        {
            get
            {
                return s_Instance.m_Resource;
            }
        }

        /// <summary>
        /// 获取场景组件。
        /// </summary>
        public static SceneComponent Scene
        {
            get
            {
                return s_Instance.m_Scene;
            }
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static SettingComponent Setting
        {
            get
            {
                return s_Instance.m_Setting;
            }
        }

        /// <summary>
        /// 获取声音组件。
        /// </summary>
        public static SoundComponent Sound
        {
            get
            {
                return s_Instance.m_Sound;
            }
        }

        /// <summary>
        /// 获取界面组件。
        /// </summary>
        public static UIComponent UI
        {
            get
            {
                return s_Instance.m_UI;
            }
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static WebRequestComponent WebRequest
        {
            get
            {
                return s_Instance.m_WebRequest;
            }
        }

        /// <summary>
        /// 构建信息。
        /// </summary>
        public static BuildInfo BuildInfo
        {
            get
            {
                return s_Instance.m_BuildInfo;
            }

            set
            {
                if (value != null)
                {
                    s_Instance.m_BuildInfo = value;
                }
            }
        }

        #region MonoBahaviour

        private void Awake()
        {
            s_Instance = this;
        }

        private IEnumerator Start()
        {
            //Debug.LogError("开始游戏");
#if !UNITY_EDITOR
            SDKManager.CreateInstance();
#endif
            //永不休眠
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.runInBackground = true;
            Application.targetFrameRate = 30;
            // Base component
            m_Base = GetCacheComponent<BaseComponent>();

            // Cache components
            m_DataTable = GetCacheComponent<DataTableComponent>();
            m_Debugger = GetCacheComponent<DebuggerComponent>();
            m_Download = GetCacheComponent<DownloadComponent>();
            m_Entity = GetCacheComponent<EntityComponent>();
            m_Event = GetCacheComponent<EventComponent>();
            m_Fsm = GetCacheComponent<FsmComponent>();
            m_Localization = GetCacheComponent<LocalizationComponent>();
            m_Network = GetCacheComponent<NetworkComponent>();
            m_ObjectPool = GetCacheComponent<ObjectPoolComponent>();
            m_Procedure = GetCacheComponent<ProcedureComponent>();
            m_Resource = GetCacheComponent<ResourceComponent>();
            m_Scene = GetCacheComponent<SceneComponent>();
            m_Setting = GetCacheComponent<SettingComponent>();
            m_Sound = GetCacheComponent<SoundComponent>();
            m_UI = GetCacheComponent<UIComponent>();
            m_WebRequest = GetCacheComponent<WebRequestComponent>();

            AdditionalComponents();

            yield return new WaitForEndOfFrame();

            PerformInits();
            UICamera.useButtonClickGapTime = true;
            //Debug.LogError("开始游戏初始化结束");
        }



        private void Update()
        {
#if UNITY_EDITOR
            if (m_FrameCountLog)
            {
                Log.Info("Frame Count: {0}.", UnityEngine.Time.frameCount.ToString());
            }
#endif
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (SDKManager.HasExitWindow)
                {
                    SDKManager.Instance.helper.ExitGame();
                }
                else
                {
                    ExitGame();
                }
            }
        }


        #endregion MonoBahaviour

        public static void Restart()
        {
            if (s_Instance != null && s_Instance.m_IsShuttingDown)
            {
                return;
            }

            s_Instance.m_IsShuttingDown = true;

            Log.Info("Restart game framework...");
            CreateShuttingDownCamera();
            BuglyAgent.UnregisterLogCallback(s_Instance.OnLogMessageReceived);
            TimerUtility.WaitEndOfFrame(s_Instance.InternalShutdown);
            TimerUtility.WaitFrames(2, OnRestart);
            CloseNetwork();
        }

        public static void Shutdown()
        {
            if (s_Instance != null && s_Instance.m_IsShuttingDown)
            {
                return;
            }

            s_Instance.m_IsShuttingDown = true;

            SDKManager.Instance.helper.UploadData("Exit");

            Log.Info("Shutdown game framework...");
            CreateShuttingDownCamera();
            BuglyAgent.UnregisterLogCallback(s_Instance.OnLogMessageReceived);
            TimerUtility.WaitEndOfFrame(s_Instance.InternalShutdownAndQuit);
            CloseNetwork();
        }
        public static void ExitGame()
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_EXIT_GAME"),
                ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM"),
                CancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL"),
                OnClickConfirm = (obj) => { Shutdown(); Application.Quit(); },
                OnClickCancel = (obj) => { },
            });
        }
        public static void SetLogWhiteList(string[] logWhiteList)
        {
            s_Instance.m_LogWhiteList = logWhiteList;
        }
        private static void CloseNetwork()
        {
            //GameEntry.Network.
            //var lc = Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);
            //if (lc == null)
            //{
            //    GameFramework.Log.Error("Network channel 'Lobby' doesn't exist.");
            //}
            //else
            //    lc.Close();
            Network.DestroyNetworkChannel(Constant.Network.LobbyNetworkChannelName);
            //var nc = Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
            //if (nc == null)
            //{
            //    GameFramework.Log.Error("Network channel 'Room' doesn't exist.");
            //}
            //else
            //    nc.Close();
            Network.DestroyNetworkChannel(Constant.Network.RoomNetworkChannelName);
        }
        private void OnApplicationQuit()
        {
            Shutdown();
        }

        private void InternalShutdown(object userData)
        {
            m_ChessManager.ShutDown();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.None);
            ResetStaticClasses();
            Destroy(gameObject);
            s_Instance = null;
        }

        private void InternalShutdownAndQuit(object userData)
        {
            InternalShutdown(userData);

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static void OnRestart(object userData)
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            SceneManager.LoadScene(0);
        }

        private static T GetCacheComponent<T>() where T : Component
        {
            T[] components = s_Instance.GetComponentsInChildren<T>();
            if (components.Length > 1)
            {
                Log.Fatal("Game framework must have only one '{0}' type component.", typeof(T).Name);
            }
            else if (components.Length == 1)
            {
                return components[0];
            }
            else
            {
                Log.Fatal("Game framework must have '{0}' type component.", typeof(T).Name);
            }

            return null;
        }

        private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
        {
            if (logType == LogType.Log)
            {
                return;
            }

            for (int i = 0; i < m_LogWhiteList.Length; ++i)
            {
                if (logMessage.Contains(m_LogWhiteList[i]))
                {
                    return;
                }
            }

            if (logType == LogType.Warning && (!Debug.isDebugBuild || !m_TreatWarningsAsErrors || GameEntry.ServerConfig.GetBool(Constant.ServerConfig.System.IgnoreWarningLog, false)))
            {
                return;
            }

            BuglyAgent.UnregisterLogCallback(OnLogMessageReceived);
            var dialogMessage = Localization.GetString("UI_TEXT_FATAL_ERROR",
                (Debug.isDebugBuild ? "\n" + logMessage : string.Empty),
                (Debug.isDebugBuild ? "\n" + stackTrace : string.Empty));
            var confirmText = Localization.GetString("UI_TEXT_RESTART");
            var cancelText = Localization.GetString("UI_TEXT_DONOT_RESTART");

            UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = dialogMessage,
                Width = 880,
                PauseGame = true,
                ConfirmText = confirmText,
                OnClickConfirm = OnClickConfirmRestart,
                CancelText = cancelText,
                OnClickCancel = OnClickCancelRestart,
            });
        }

        private void OnClickConfirmRestart(object userData)
        {
            if (this == null)
            {
                return;
            }

            Restart();
        }

        private void OnClickCancelRestart(object userData)
        {
            if (this == null)
            {
                return;
            }

            BuglyAgent.RegisterLogCallback(OnLogMessageReceived);
        }

        private static void CreateShuttingDownCamera()
        {
            var go = new GameObject(Constant.ShuttingDownCameraName);
            DontDestroyOnLoad(go);
            var camera = go.AddComponent<Camera>();
            camera.depth = 100;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.cullingMask = 0;
        }
    }
}
