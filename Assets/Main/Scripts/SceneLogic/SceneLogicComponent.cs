using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 场景逻辑组件。
    /// </summary>
    public class SceneLogicComponent : MonoBehaviour
    {
        private SceneLogicState m_State = SceneLogicState.Unknown;
        private int m_SceneId = 0;
        private string m_SceneName = string.Empty;

        private BaseInstanceLogic m_BaseInstanceLogic = null;

#if UNITY_EDITOR
#pragma warning disable 0414

        [SerializeField]
        private NonInstanceLogic m_NonInstanceLogic = null;

        [SerializeField]
        private SinglePlayerInstanceLogic m_SinglePlayerInstanceLogic = null;

        [SerializeField]
        private ChessBattleInstanceLogic m_ChessBattleInstanceLogic = null;

        [SerializeField]
        private ArenaBattleInstanceLogic m_ArenaBattleInstanceLogic = null;

        [SerializeField]
        private SinglePvpInstanceLogic m_SinglePvpInstanceLogic = null;

        [SerializeField]
        private DemoInstanceLogic m_DemoInstanceLogic = null;

        [SerializeField]
        private CosmosCrackInstanceLogic m_CosmosCrackInstanceLogic = null;

        [SerializeField]
        private InstanceForResourceLogic m_InstanceForResourceLogic = null;

        [SerializeField]
        private MimicMeleeInstanceLogic m_MimicMeleeInstanceLogic = null;

#pragma warning restore 0414
#endif

        private UIForm m_MainForm = null;
        private UIForm m_BattleForm = null;

        /// <summary>
        /// 获取当前的 <see cref="Genesis.GameClient.BattleForm"/> 实例。如不存在则返回 null。
        /// </summary>
        public BattleForm BattleForm
        {
            get
            {
                if (m_BattleForm == null)
                {
                    return null;
                }

                return m_BattleForm.Logic as BattleForm;
            }
        }

        /// <summary>
        /// 当前的场景状态。
        /// </summary>
        public SceneLogicState State
        {
            get
            {
                return m_State;
            }
        }

        /// <summary>
        /// 当前的场景编号。切换场景时返回 0。
        /// </summary>
        public int SceneId
        {
            get
            {
                return m_SceneId;
            }

            private set
            {
                m_SceneId = value;
                BuglyAgent.SetScene(m_SceneId);
            }
        }

        /// <summary>
        /// 当前的场景名称。
        /// </summary>
        public string SceneName
        {
            get
            {
                return m_SceneName;
            }

            private set
            {
                m_SceneName = value;
                if (!string.IsNullOrEmpty(m_SceneName))
                {
                    BuglyAgent.AddSceneData("name", m_SceneName);
                }
            }
        }

        /// <summary>
        /// 当前是否在副本中。
        /// </summary>
        public bool IsInstance
        {
            get
            {
                return InstanceLogicType != InstanceLogicType.NonInstance;
            }
        }

        /// <summary>
        /// 当前副本逻辑类型。
        /// </summary>
        public InstanceLogicType InstanceLogicType
        {
            get
            {
                return m_BaseInstanceLogic.Type;
            }
        }

        /// <summary>
        /// 当前副本逻辑对象。
        /// </summary>
        public BaseInstanceLogic BaseInstanceLogic
        {
            get
            {
                return m_BaseInstanceLogic;
            }

            private set
            {
                m_BaseInstanceLogic = value;

#if UNITY_EDITOR
                m_NonInstanceLogic = m_BaseInstanceLogic as NonInstanceLogic;
                m_SinglePlayerInstanceLogic = m_BaseInstanceLogic as SinglePlayerInstanceLogic;
                m_ChessBattleInstanceLogic = m_BaseInstanceLogic as ChessBattleInstanceLogic;
                m_ArenaBattleInstanceLogic = m_BaseInstanceLogic as ArenaBattleInstanceLogic;
                m_SinglePvpInstanceLogic = m_BaseInstanceLogic as SinglePvpInstanceLogic;
                m_DemoInstanceLogic = m_BaseInstanceLogic as DemoInstanceLogic;
                m_CosmosCrackInstanceLogic = m_BaseInstanceLogic as CosmosCrackInstanceLogic;
                m_InstanceForResourceLogic = m_BaseInstanceLogic as InstanceForResourceLogic;
                m_MimicMeleeInstanceLogic = m_BaseInstanceLogic as MimicMeleeInstanceLogic;
#endif
            }
        }

        public SinglePlayerInstanceLogic SinglePlayerInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as SinglePlayerInstanceLogic;
            }
        }

        public BaseSinglePlayerInstanceLogic BaseSinglePlayerInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as BaseSinglePlayerInstanceLogic;
            }
        }

        public BasePvpaiInstanceLogic BasePvpaiInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as BasePvpaiInstanceLogic;
            }
        }

        public ChessBattleInstanceLogic ChessBattleInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as ChessBattleInstanceLogic;
            }
        }

        public ArenaBattleInstanceLogic ArenaBattleInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as ArenaBattleInstanceLogic;
            }
        }

        public SinglePvpInstanceLogic SinglePvpInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as SinglePvpInstanceLogic;
            }
        }

        public DemoInstanceLogic DemoInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as DemoInstanceLogic;
            }
        }

        public NonInstanceLogic NonInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as NonInstanceLogic;
            }
        }

        public CosmosCrackInstanceLogic CosmosCrackInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as CosmosCrackInstanceLogic;
            }
        }

        public InstanceForResourceLogic InstanceForResourceLogic
        {
            get
            {
                return BaseInstanceLogic as InstanceForResourceLogic;
            }
        }

        public MimicMeleeInstanceLogic MimicMeleeInstanceLogic
        {
            get
            {
                return BaseInstanceLogic as MimicMeleeInstanceLogic;
            }
        }

        public MeHeroCharacter MeHeroCharacter
        {
            get
            {
                return BaseInstanceLogic.MeHeroCharacter;
            }
        }

        /// <summary>
        /// 返回主城。
        /// </summary>
        public void GoBackToLobby(bool hideLoading = true)
        {
            bool autoHideLoading = BaseInstanceLogic.Type != InstanceLogicType.ArenaBattle
                && BaseInstanceLogic.Type != InstanceLogicType.ChessBattle
                && BaseInstanceLogic.Type != InstanceLogicType.CosmosCrack
                && hideLoading;
            GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.NonInstance, Constant.LobbySceneId, autoHideLoading));
        }

        /// <summary>
        /// 打开主界面。
        /// </summary>
        public void OpenMainUI()
        {
            GameEntry.UI.OpenUIForm(UIFormId.Main);
        }

        /// <summary>
        /// 关闭主界面。
        /// </summary>
        private void CloseMainUI()
        {
            if (m_MainForm != null)
            {
                GameEntry.UI.CloseUIForm(m_MainForm);
                m_MainForm = null;
            }
        }

        /// <summary>
        /// 打开战斗界面。
        /// </summary>
        public void OpenBattleForm()
        {
            GameEntry.UI.OpenUIForm(UIFormId.BattleForm);
        }

        /// <summary>
        /// 关闭战斗界面。
        /// </summary>
        private void CloseBattleForm()
        {
            if (m_BattleForm != null)
            {
                GameEntry.UI.CloseUIForm(m_BattleForm);
                m_BattleForm = null;
            }
        }

        #region MonoBehaviour

        private void Start()
        {
            InitInstanceLogic(InstanceLogicType.NonInstance, GameEntry.OfflineMode.OfflineModeEnabled ? GameEntry.OfflineMode.OfflineSceneId : Constant.LobbySceneId);
            Log.Info("Scene logic component has been initialized.");

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(EventId.WillChangeScene, OnWillChangeScene);
            GameEntry.Event.Subscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
            GameEntry.Event.Subscribe(EventId.ChangeSceneComplete, OnChangeSceneComplete);
        }

        private void Update()
        {
            BaseInstanceLogic.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(EventId.WillChangeScene, OnWillChangeScene);
                GameEntry.Event.Unsubscribe(EventId.ChangeSceneStart, OnChangeSceneStart);
                GameEntry.Event.Unsubscribe(EventId.ChangeSceneComplete, OnChangeSceneComplete);
            }

            ClearInstanceLogic();
        }

        #endregion MonoBehaviour

        private void InitInstanceLogic(InstanceLogicType logicType, int instanceOrSceneId, object userData = null)
        {
            BaseInstanceLogic = InstanceLogicFactory.Create(logicType);
            BaseInstanceLogic.Init(instanceOrSceneId, userData);
        }

        private void ClearInstanceLogic()
        {
            if (BaseInstanceLogic != null)
            {
                BaseInstanceLogic.Shutdown();
                BaseInstanceLogic = null;
            }
            if (null != GameEntry.ObjectPool)
            {
                GameEntry.ObjectPool.Release();
                GameEntry.ObjectPool.ReleaseAllUnused();
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
            if (ne.UIForm.Logic is MainForm)
            {
                m_MainForm = ne.UIForm;
            }

            if (ne.UIForm.Logic is BattleForm)
            {
                m_BattleForm = ne.UIForm;
            }
        }

        private void OnWillChangeScene(object sender, GameEventArgs e)
        {
            var ne = e as WillChangeSceneEventArgs;
            //关闭输入操作接收
            GameEntry.Input.MeHeroCharacter = null;
            GameEntry.Input.JoystickActivated = false;
            GameEntry.Input.SkillActivated = false;
            //清空实体数据
            GameEntry.Entity.RemoveAllLocks();
            GameEntry.Entity.HideAllEntities();
            GameEntry.Data.ClearAllEntityDatas();
            SwitchInstanceLogic(ne.InstanceLogicType, ne.SceneOrInstanceId, ne.UserData);
            SceneName = null;
            SceneId = 0;
            m_State = SceneLogicState.WillLoad;

            GameEntry.Tutorial.HideMask();
        }

        private void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            m_State = SceneLogicState.Loading;
            CloseMainUI();
            CloseBattleForm();
        }

        private void OnChangeSceneComplete(object sender, GameEventArgs e)
        {
            SceneId = BaseInstanceLogic.SceneId;
            BaseInstanceLogic.RequestStartInstance();

            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene sceneDataRow = dtScene.GetDataRow(m_SceneId);
            if (sceneDataRow == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", m_SceneId.ToString());
                return;
            }

            SceneName = GameEntry.Localization.GetString(sceneDataRow.Name);
            m_State = SceneLogicState.Ready;
        }

        private void SwitchInstanceLogic(InstanceLogicType logicType, int instanceOrSceneId, object userData)
        {
            ClearInstanceLogic();
            InitInstanceLogic(logicType, instanceOrSceneId, userData);
        }
    }
}
