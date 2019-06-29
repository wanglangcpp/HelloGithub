using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载组件。
    /// </summary>
    public class LoadingComponent : MonoBehaviour
    {
        [SerializeField]
        private string m_TemplateName = null;

        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Loading";

        [SerializeField, Range(0f, 1f)]
        private float m_DependencyOverLoadingMin = 0.85f;

        [SerializeField, Range(0f, 1f)]
        private float m_DependencyOverLoadingMax = 0.98f;

        private GameObject m_LoadingTemplate = null;
        private float m_DependencyOverLoading = 0f;
        private Loading m_LoadingPanel = null;

        public bool PreloadComplete
        {
            get
            {
                return m_LoadingTemplate != null;
            }
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);
            GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);

            m_DependencyOverLoading = m_DependencyOverLoadingMin + (m_DependencyOverLoadingMax - m_DependencyOverLoadingMin) * Random.value;
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneFailure, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);
            GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        public void Preload()
        {
            PreloadUtility.LoadPreloadResource(m_TemplateName);
        }

        public void Show(int id = -1)
        {
            m_LoadingPanel.ShowBackgroundMask();
            m_LoadingPanel.ProgressText = string.Empty;
            m_LoadingPanel.Progress = 0f;
            m_LoadingPanel.TipsText = GameEntry.Localization.GetString(string.Format("UI_TEXT_LOADINGTIPS_{0:D3}", Random.Range(0, Constant.LoadingTipsCount)));
            var drList = GameEntry.DataTable.GetDataTable<DRLoadingBackground>().GetAllDataRows();
            if (id != -1)
            {
                for (int i = 0; i < drList.Length; i++)
                {
                    if (drList[i].Id == id)
                    {
                        m_LoadingPanel.Background.LoadAsync(drList[i].TexturePath, OnLoadTextureSuccess);
                        return;
                    }
                }
            }
            else {
                float sum = 0f;
                for (int i = 0; i < drList.Length; i++)
                {
                    sum += drList[i].Weight;
                }
                float weight = Random.Range(0, sum);
                sum = 0;
                for (int i = 0; i < drList.Length; i++)
                {
                    sum += drList[i].Weight;
                    if (sum >= weight)
                    {
                        m_LoadingPanel.Background.LoadAsync(drList[i].TexturePath, OnLoadTextureSuccess);
                        return;
                    }
                }
            }
        }

        public void Hide()
        {
            m_LoadingPanel.FadeOut(OnFadeOutComplete);
        }

        private void OnFadeOutComplete()
        {
            if (m_LoadingPanel.Background == null)
            {
                return;
            }

            m_LoadingPanel.Background.mainTexture = null;
            m_LoadingPanel.gameObject.SetActive(false);
            NGUIExtensionMethods.ReleaseTextureIfNeeded(m_LoadingPanel.Background.GetHashCode());
        }

        private void SetProgress(float progress)
        {
            m_LoadingPanel.ProgressText = GameEntry.Localization.GetString("CHANGE_SCENE_PROGRESS", progress);
            m_LoadingPanel.Progress = progress;
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneSuccessEventArgs;
            if (ne.SceneName.Contains("_"))
            {
                return;
            }

            m_LoadingPanel.ProgressText = GameEntry.Localization.GetString("CHANGE_SCENE_SUCCESS");
            m_LoadingPanel.Progress = 1f;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneFailureEventArgs;
            if (ne.SceneName.Contains("_"))
            {
                return;
            }

            m_LoadingPanel.ProgressText = GameEntry.Localization.GetString("CHANGE_SCENE_FAILED");
            m_LoadingPanel.Progress = 0f;
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneUpdateEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneUpdateEventArgs;
            if (ne.SceneName.Contains("_"))
            {
                return;
            }

            if (GameEntry.Base.EditorResourceMode)
            {
                SetProgress(ne.Progress);
            }
            else
            {
                SetProgress(m_DependencyOverLoading + (1f - m_DependencyOverLoading) * ne.Progress);
            }
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.LoadSceneDependencyAssetEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneDependencyAssetEventArgs;
            if (ne.SceneName.Contains("_"))
            {
                return;
            }

            SetProgress(m_DependencyOverLoading * ne.LoadedCount / ne.TotalCount);
        }

        private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
        {
            LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
            if (ne.Name != m_TemplateName)
            {
                return;
            }

            m_LoadingTemplate = ne.Resource as GameObject;
            m_LoadingPanel = NGUITools.AddChild(m_ParentNode, m_LoadingTemplate).GetComponent<Loading>();
            m_LoadingPanel.name = m_NodeName;
            m_LoadingPanel.gameObject.SetActive(false);
            m_LoadingPanel.VersionText = !Debug.isDebugBuild ? string.Empty
                : string.Format("{0} ({1})", string.IsNullOrEmpty(GameEntry.Base.GameVersion) ? Application.version : GameEntry.Base.GameVersion, GameEntry.Base.InternalApplicationVersion.ToString());
            m_LoadingPanel.ResizeBackground();
        }

        private void OnLoadTextureSuccess(UITexture texture, object userData)
        {
            if (!m_LoadingPanel.gameObject.activeSelf || texture != m_LoadingPanel.Background)
            {
                return;
            }

            if (texture.mainTexture)
            {
                texture.height = texture.width * texture.mainTexture.height / texture.mainTexture.width;
            }

            m_LoadingPanel.FadeInMainPanel();
        }
    }
}
