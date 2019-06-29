using GameFramework;
using System;
using System.Reflection;
using UnityEngine;

namespace Genesis.GameClient
{
    public sealed partial class GameEntry
    {
        private DataComponent m_Data = null;
        private InputComponent m_Input = null;
        private SceneLogicComponent m_SceneLogic = null;
        private LobbyLogicComponent m_LobbyLogic = null;
        private RoomLogicComponent m_RoomLogic = null;
        private TimeLineComponent m_TimeLine = null;
        private BehaviorComponent m_Behavior = null;
        private ImpactComponent m_Impact = null;
        private CameraShakingComponent m_CameraShaking = null;
        private NativeCallbackComponent m_NativeCallback = null;
        private OfflineModeComponent m_OfflineMode = null;
        private LoadingComponent m_Loading = null;
        private WaitingComponent m_Waiting = null;
        private NGUIAtlasComponent m_NGUIAtlas = null;
        private ServerConfigComponent m_ServerConfig = null;
        private TimeComponent m_Time = null;
        private StringReplacementComponent m_StringReplacement = null;
        private TimeScaleComponent m_TimeScale = null;
        private HighlightProjectorComponent m_HighlightProjector = null;
        private CameraPostEffectComponent m_CameraPostEffect = null;
        private TextureComponent m_Texture = null;
        private UIBackgroundComponent m_UIBackground = null;
        private ScreenTouchEffectComponent m_ScreenTouchEffect = null;
        private AntiOcclusionComponent m_AntiOcclusion = null;
        private PlayerEnergyComponent m_PlayerEnergy = null;
        private ReminderComponent m_Reminder = null;
        private TutorialComponent m_Tutorial = null;
        private ClientConfigComponent m_ClientConfig = null;
        private RewardViewerComponent m_RewardViewer = null;
        private UIFragmentComponent m_UIFragment = null;
        private LuaComponent m_Lua = null;
        private DataTableProxyComponent m_DataTableProxy = null;
        private UwaComponent m_Uwa = null;
        private WhereToGetComponent m_WhereToGet = null;
        private DisplayModelComponent m_DisplayModel = null;
        private OpenFunctionComponent m_OpenFunctions = null;
        private NoviceGuideCompoment m_noviceGuide = null;
        private IChessManager m_ChessManager = null;
        private TaskComponent m_TaskComponent = null;

        private static bool s_BuglyInited = false;

        /// <summary>
        /// 获取数据组件。
        /// </summary>
        public static DataComponent Data
        {
            get
            {
                return s_Instance.m_Data;
            }
        }

        /// <summary>
        /// 获取输入组件。
        /// </summary>
        public static InputComponent Input
        {
            get
            {
                return s_Instance.m_Input;
            }
        }

        /// <summary>
        /// 获取场景逻辑组件。
        /// </summary>
        public static SceneLogicComponent SceneLogic
        {
            get
            {
                return s_Instance.m_SceneLogic;
            }
        }

        /// <summary>
        /// 获取大厅逻辑组件。
        /// </summary>
        public static LobbyLogicComponent LobbyLogic
        {
            get
            {
                return s_Instance.m_LobbyLogic;
            }
        }

        /// <summary>
        /// 获取房间逻辑组件。
        /// </summary>
        public static RoomLogicComponent RoomLogic
        {
            get
            {
                return s_Instance.m_RoomLogic;
            }
        }

        /// <summary>
        /// 获取时间轴组件。
        /// </summary>
        public static TimeLineComponent TimeLine
        {
            get
            {
                return s_Instance.m_TimeLine;
            }
        }

        /// <summary>
        /// 获取行为组件。
        /// </summary>
        public static BehaviorComponent Behavior
        {
            get
            {
                return s_Instance.m_Behavior;
            }
        }

        /// <summary>
        /// 获取伤害组件。
        /// </summary>
        public static ImpactComponent Impact
        {
            get
            {
                return s_Instance.m_Impact;
            }
        }

        /// <summary>
        /// 获取摄像机震动组件。
        /// </summary>
        public static CameraShakingComponent CameraShaking
        {
            get
            {
                return s_Instance.m_CameraShaking;
            }
        }

        /// <summary>
        /// 获取本地代码回调组件。
        /// </summary>
        public static NativeCallbackComponent NativeCallback
        {
            get
            {
                return s_Instance.m_NativeCallback;
            }
        }

        /// <summary>
        /// 获取离线模式组件。
        /// </summary>
        public static OfflineModeComponent OfflineMode
        {
            get
            {
                return s_Instance.m_OfflineMode;
            }
        }

        /// <summary>
        /// 获取加载组件。
        /// </summary>
        public static LoadingComponent Loading
        {
            get
            {
                return s_Instance.m_Loading;
            }
        }

        /// <summary>
        /// 获取等待组件。
        /// </summary>
        public static WaitingComponent Waiting
        {
            get
            {
                return s_Instance.m_Waiting;
            }
        }

        /// <summary>
        /// 获取 NGUI 图集组件。
        /// </summary>
        public static NGUIAtlasComponent NGUIAtlas
        {
            get
            {
                return s_Instance.m_NGUIAtlas;
            }
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static ServerConfigComponent ServerConfig
        {
            get
            {
                return s_Instance.m_ServerConfig;
            }
        }

        /// <summary>
        /// 获取时间组件。
        /// </summary>
        public static TimeComponent Time
        {
            get
            {
                return s_Instance.m_Time;
            }
        }

        /// <summary>
        /// 获取文本替换组件。
        /// </summary>
        public static StringReplacementComponent StringReplacement
        {
            get
            {
                return s_Instance.m_StringReplacement;
            }
        }

        /// <summary>
        /// 获取游戏速度组件。
        /// </summary>
        public static TimeScaleComponent TimeScale
        {
            get
            {
                return s_Instance.m_TimeScale;
            }
        }

        /// <summary>
        /// 获取高亮投影器组件。
        /// </summary>
        public static HighlightProjectorComponent HighlightProjector
        {
            get
            {
                return s_Instance.m_HighlightProjector;
            }
        }

        /// <summary>
        /// 获取摄像机后处理组件。
        /// </summary>
        public static CameraPostEffectComponent CameraPostEffect
        {
            get
            {
                return s_Instance.m_CameraPostEffect;
            }
        }

        /// <summary>
        /// 获取翻翻棋管理器。
        /// </summary>
        public static IChessManager ChessManager
        {
            get
            {
                return s_Instance.m_ChessManager;
            }
        }

        /// <summary>
        /// 获取贴图组件。
        /// </summary>
        public static TextureComponent Texture
        {
            get
            {
                return s_Instance.m_Texture;
            }
        }

        /// <summary>
        /// 获取界面背景组件。
        /// </summary>
        public static UIBackgroundComponent UIBackground
        {
            get
            {
                return s_Instance.m_UIBackground;
            }
        }

        /// <summary>
        /// 获取屏幕触效组件。
        /// </summary>
        public static ScreenTouchEffectComponent ScreenTouchEffect
        {
            get
            {
                return s_Instance.m_ScreenTouchEffect;
            }
        }

        /// <summary>
        /// 获取主角防遮挡组件。
        /// </summary>
        public static AntiOcclusionComponent AntiOcclusion
        {
            get
            {
                return s_Instance.m_AntiOcclusion;
            }
        }

        /// <summary>
        /// 获取玩家体力组件。
        /// </summary>
        public static PlayerEnergyComponent PlayerEnergy
        {
            get
            {
                return s_Instance.m_PlayerEnergy;
            }
        }

        /// <summary>
        /// 获取提醒器组件。
        /// </summary>
        public static ReminderComponent Reminder
        {
            get
            {
                return s_Instance.m_Reminder;
            }
        }

        /// <summary>
        /// 获取教程组件。
        /// </summary>
        public static TutorialComponent Tutorial
        {
            get
            {
                return s_Instance.m_Tutorial;
            }
        }

        /// <summary>
        /// 获取教程组件。
        /// </summary>
        public static ClientConfigComponent ClientConfig
        {
            get
            {
                return s_Instance.m_ClientConfig;
            }
        }

        /// <summary>
        /// 获取奖励视图组件。
        /// </summary>
        public static RewardViewerComponent RewardViewer
        {
            get
            {
                return s_Instance.m_RewardViewer;
            }
        }

        /// <summary>
        /// 获取界面片段池组件。
        /// </summary>
        public static UIFragmentComponent UIFragment
        {
            get
            {
                return s_Instance.m_UIFragment;
            }
        }

        /// <summary>
        /// 获取 Lua 组件。
        /// </summary>
        public static LuaComponent Lua
        {
            get
            {
                return s_Instance.m_Lua;
            }
        }

        /// <summary>
        /// 获取数据表代理组件。
        /// </summary>
        public static DataTableProxyComponent DataTableProxy
        {
            get
            {
                return s_Instance.m_DataTableProxy;
            }
        }

        /// <summary>
        /// 获取 UWA 组件。
        /// </summary>
        public static UwaComponent Uwa
        {
            get
            {
                return s_Instance.m_Uwa;
            }
        }

        /// <summary>
        /// 获取『获取途径及功能跳转』组件。
        /// </summary>
        public static WhereToGetComponent WhereToGet
        {
            get
            {
                return s_Instance.m_WhereToGet;
            }
        }

        /// <summary>
        /// 获取任务组件
        /// </summary>
        public static TaskComponent TaskComponent
        {
            get
            {
                return s_Instance.m_TaskComponent;
            }
        }
        public static DisplayModelComponent DisplayModel
        {
            get { return s_Instance.m_DisplayModel; }
        }

        public static OpenFunctionComponent OpenFunction {
            get { return s_Instance.m_OpenFunctions; }
        }

        public static NoviceGuideCompoment NoviceGuide {
            get { return s_Instance.m_noviceGuide; }
        }


        private void AdditionalComponents()
        {
            // Add additional components
            m_Data = GetCacheComponent<DataComponent>();
            m_Input = GetCacheComponent<InputComponent>();
            m_SceneLogic = GetCacheComponent<SceneLogicComponent>();
            m_LobbyLogic = GetCacheComponent<LobbyLogicComponent>();
            m_RoomLogic = GetCacheComponent<RoomLogicComponent>();
            m_TimeLine = GetCacheComponent<TimeLineComponent>();
            m_Behavior = GetCacheComponent<BehaviorComponent>();
            m_Impact = GetCacheComponent<ImpactComponent>();
            m_CameraShaking = GetCacheComponent<CameraShakingComponent>();
            m_NativeCallback = GetCacheComponent<NativeCallbackComponent>();
            m_Loading = GetCacheComponent<LoadingComponent>();
            m_Waiting = GetCacheComponent<WaitingComponent>();
            m_OfflineMode = GetCacheComponent<OfflineModeComponent>();
            m_NGUIAtlas = GetCacheComponent<NGUIAtlasComponent>();
            m_ServerConfig = GetCacheComponent<ServerConfigComponent>();
            m_Time = GetCacheComponent<TimeComponent>();
            m_StringReplacement = GetCacheComponent<StringReplacementComponent>();
            m_TimeScale = GetCacheComponent<TimeScaleComponent>();
            m_HighlightProjector = GetCacheComponent<HighlightProjectorComponent>();
            m_CameraPostEffect = GetCacheComponent<CameraPostEffectComponent>();
            m_Texture = GetCacheComponent<TextureComponent>();
            m_UIBackground = GetCacheComponent<UIBackgroundComponent>();
            m_ScreenTouchEffect = GetCacheComponent<ScreenTouchEffectComponent>();
            m_AntiOcclusion = GetCacheComponent<AntiOcclusionComponent>();
            m_PlayerEnergy = GetCacheComponent<PlayerEnergyComponent>();
            m_Reminder = GetCacheComponent<ReminderComponent>();
            m_Tutorial = GetCacheComponent<TutorialComponent>();
            m_ClientConfig = GetCacheComponent<ClientConfigComponent>();
            m_RewardViewer = GetCacheComponent<RewardViewerComponent>();
            m_UIFragment = GetCacheComponent<UIFragmentComponent>();
            m_Lua = GetCacheComponent<LuaComponent>();
            m_DataTableProxy = GetCacheComponent<DataTableProxyComponent>();
            m_Uwa = GetCacheComponent<UwaComponent>();
            m_WhereToGet = GetCacheComponent<WhereToGetComponent>();
            m_DisplayModel = GetCacheComponent<DisplayModelComponent>();
            m_OpenFunctions = GetCacheComponent<OpenFunctionComponent>();
            m_noviceGuide = GetCacheComponent<NoviceGuideCompoment>();
            m_TaskComponent = GetCacheComponent<TaskComponent>();
        }

        public static void RegisterAdditionalDebuggers()
        {
            if (s_Instance == null)
            {
                return;
            }

            s_Instance.RegisterAdditionalDebuggers_Internal();
        }

        private void RegisterAdditionalDebuggers_Internal()
        {
            // Add additional debuggers
            m_Debugger.WindowScale = Screen.height / 800f;
#if UNITY_IOS
            m_Debugger.RegisterDebuggerWindow("Information/Other/iOS", new InformationIOSWindow(), m_Debugger, m_Setting);
#endif
            if (!BuildInfo.GitBranch.Contains("release"))
            {
                if (!OfflineMode.OfflineModeEnabled)
                {
                    m_Debugger.RegisterDebuggerWindow("Game/GM Command/Currency", new GMCommandDebuggerWindow_Currency(), m_Debugger, m_Setting);
                    m_Debugger.RegisterDebuggerWindow("Game/GM Command/Hero", new GMCommandDebuggerWindow_Hero(), m_Debugger, m_Setting);
                    m_Debugger.RegisterDebuggerWindow("Game/GM Command/Level", new GMCommandDebuggerWindow_Level(), m_Debugger, m_Setting);
                    m_Debugger.RegisterDebuggerWindow("Game/GM Command/Item", new GMCommandDebuggerWindow_Item(), m_Debugger, m_Setting);
                    m_Debugger.RegisterDebuggerWindow("Game/GM Command/Other", new GMCommandDebuggerWindow_Other(), m_Debugger, m_Setting);
                }
                m_Debugger.RegisterDebuggerWindow("Game/Scene", new ChangeSceneDebuggerWindow(), m_Debugger, m_Setting);
                m_Debugger.RegisterDebuggerWindow("Game/Instance", new ChangeInstanceDebuggerWindow(), m_Debugger, m_Setting);
            }

            //if (!Base.EditorResourceMode)
            //{
            //    m_Debugger.RegisterDebuggerWindow("Game/Language", new ChangeLanguageDebuggerWindow(), m_Debugger, m_Setting);
            //}

            m_Debugger.RegisterDebuggerWindow("Game/Memory", new MemoryDebuggerWindow(), m_Debugger, m_Setting);
            m_Debugger.RegisterDebuggerWindow("Game/Exception", new ExceptionDebuggerWindow(), m_Debugger, m_Setting);
            m_Debugger.RegisterDebuggerWindow("Game/UI Forms", new UIFormDebuggerWindow(), m_Debugger, m_Setting);
            m_Debugger.RegisterDebuggerWindow("Game/Waiting", new WaitingComponentDebuggerWindow(), m_Debugger, m_Setting);

            m_Debugger.RegisterDebuggerWindow("Information/Other/Build Info", new BuildInfoDebuggerWindow(), m_Debugger, m_Setting);

#if UNITY_EDITOR
            if (Base.EditorResourceMode && OfflineMode.OfflineModeEnabled)
            {
                m_Debugger.RegisterDebuggerWindow("Game/Time Line", new TimeLineDebuggerWindow(), m_Debugger, m_Setting);
            }
#endif
        }

        private void PerformInits()
        {
            m_ChessManager = ChessManagerFactory.Create();
            m_ChessManager.Init(m_Event, new GameFrameworkFunc<ChessBattleEnemyData>(delegate { return m_Data.ChessBattleEnemy; }));
            InitBuglyIfNeeded();
        }

        private void InitBuglyIfNeeded()
        {
            if (!s_BuglyInited)
            {
                BuglyAgent.ConfigDebugMode(Debug.isDebugBuild);
                BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogError);
                BuglyAgent.SetLogCallbackExtrasHandler(BuglyUtility.PrepareExtraData);
                BuglyAgent.InitWithAppId(Constant.BuglyAppId);
                BuglyAgent.EnableExceptionHandler();
                s_BuglyInited = true;
            }

            BuglyAgent.RegisterLogCallback(OnLogMessageReceived);
        }

        private void ResetStaticClasses()
        {
            Type[] types = Assembly.GetCallingAssembly().GetTypes();

            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];

                // Static classes are marked as abstract and sealed in IL.
                if (type.IsClass && type.IsAbstract && type.IsSealed)
                {
                    MethodInfo resetMethod = type.GetMethod("Reset", BindingFlags.Public | BindingFlags.Static, null, new Type[] { }, null);
                    if (resetMethod != null)
                    {
                        resetMethod.Invoke(null, null);
                    }
                }
            }
        }
    }
}
