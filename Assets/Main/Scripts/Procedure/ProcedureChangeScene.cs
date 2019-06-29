using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public partial class ProcedureChangeScene : ProcedureBase
    {
        private bool m_AutoHideLoading;
        private InstanceLogicType m_InstanceLogicType;
        private bool m_IsChangeSceneComplete;
        private DRScene m_CachedDataRow;
        private int m_LoadSceneAdditiveRegionCount;
        private int m_CurrentLoadedSceneAdditiveRegionCount;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);
            GameEntry.Event.Subscribe(EventId.InstanceReadyToStart, OnInstanceReadyToStart);

            m_IsChangeSceneComplete = false;
            m_CachedDataRow = null;
            m_LoadSceneAdditiveRegionCount = 0;
            m_CurrentLoadedSceneAdditiveRegionCount = 0;
            GameEntry.Sound.StopAllSounds();

            int showId = GameEntry.Data.HasTempData(Constant.TempData.SignInShowId) ? GameEntry.Data.GetAndRemoveTempData<int>(Constant.TempData.SignInShowId) : -1;
            GameEntry.Loading.Show(showId);

            string[] loadedSceneNames = GameEntry.Scene.GetLoadedSceneNames();
            for (int i = 0; i < loadedSceneNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneNames[i]);
            }

            GameEntry.Resource.ForceUnloadUnusedAssets(true);
            GameEntry.Lua.GC();

            int sceneId = GameEntry.SceneLogic.BaseInstanceLogic.SceneId;
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            m_CachedDataRow = dtScene.GetDataRow(sceneId);
            if (m_CachedDataRow == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }

            LoadSceneMainRegion();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);
                GameEntry.Event.Unsubscribe(EventId.InstanceReadyToStart, OnInstanceReadyToStart);
            }

            if (GameEntry.IsAvailable && m_AutoHideLoading)
            {
                GameEntry.Loading.Hide();
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_IsChangeSceneComplete)
            {
                ChangeState<ProcedureMain>(procedureOwner);
                return;
            }
        }

        public void ChangeScene(ProcedureOwner procedureOwner, InstanceLogicType instanceLogicType, bool autoHideLoading)
        {
            m_AutoHideLoading = autoHideLoading;
            m_InstanceLogicType = instanceLogicType;
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private void LoadSceneMainRegion()
        {
            GameEntry.Event.Fire(this, new ChangeSceneStartEventArgs(m_InstanceLogicType));

            SceneRegion.Reset();
            GameEntry.Scene.LoadScene(m_CachedDataRow.ResourceName, AssetUtility.GetSceneAsset(m_CachedDataRow.ResourceName));
            if (m_CachedDataRow.BackgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(m_CachedDataRow.BackgroundMusicId);
            }
        }

        private void LoadSceneAdditiveRegions()
        {
            int[] sceneRegionIds = GameEntry.SceneLogic.BaseInstanceLogic.GetSceneRegionIds();
            m_LoadSceneAdditiveRegionCount = sceneRegionIds.Length;
            for (int i = 0; i < m_LoadSceneAdditiveRegionCount; i++)
            {
                string sceneRegionName = string.Format("{0}_{1}", m_CachedDataRow.ResourceName, sceneRegionIds[i].ToString("D3"));
                GameEntry.Scene.LoadScene(sceneRegionName, AssetUtility.GetSceneAsset(sceneRegionName), true);
            }
        }

        private void OnInstanceReadyToStart(object sender, GameEventArgs e)
        {
            var ne = e as InstanceReadyToStartEventArgs;
            if (ne == null)
            {
                Log.Warning("Oops, type of e ('{0}') is incorrect.", e == null ? "null" : e.GetType().FullName);
                return;
            }

            m_IsChangeSceneComplete = true;
            GameEntry.Event.Fire(this, new ChangeSceneCompleteEventArgs(m_InstanceLogicType));
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneSuccessEventArgs;
            Log.Info("Load scene '{0}' OK.", ne.SceneName);

            if (!ne.SceneName.Contains("_"))
            {
                LoadSceneAdditiveRegions();
            }
            else
            {
                m_CurrentLoadedSceneAdditiveRegionCount++;
            }

            if (m_CurrentLoadedSceneAdditiveRegionCount < m_LoadSceneAdditiveRegionCount)
            {
                return;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.OnLoadSceneSuccess(e);
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneFailureEventArgs;
            OnError("Load scene failure, scene name '{0}', asset name '{1}', error message '{2}'.", ne.SceneName, ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            //UnityGameFramework.LoadSceneUpdateEventArgs ne = e as UnityGameFramework.LoadSceneUpdateEventArgs;
            //Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            //UnityGameFramework.LoadSceneDependencyAssetEventArgs ne = e as UnityGameFramework.LoadSceneDependencyAssetEventArgs;
            //Log.Debug("Load scene '{0}' dependency resource '{1}', current count '{2}', total count '{3}'.", ne.SceneName, ne.DependencyResourceName, ne.CurrentCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
