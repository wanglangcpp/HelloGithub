//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Download;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 编辑器资源组件。
    /// </summary>
    public class EditorResourceComponent : MonoBehaviour, IResourceManager
    {
        private string m_ReadOnlyPath = null;
        private string m_ReadWritePath = null;
        private List<LoadSceneInfo> m_LoadSceneInfos = null;
        [SerializeField]
        private ResourceMode resourceMode = ResourceMode.Unspecified;

        private void Awake()
        {
            m_ReadOnlyPath = null;
            m_ReadWritePath = null;
            m_LoadSceneInfos = new List<LoadSceneInfo>();

            BaseComponent baseComponent = GetComponent<BaseComponent>();
            
            if (baseComponent == null)
            {
                Log.Error("Can not find base component.");
                return;
            }
            if (resourceMode == ResourceMode.Package)
            {
                Genesis.GameClient.GameEntry.Resource.SetResourceMode(ResourceMode.Package);
            }
            if (baseComponent.EditorResourceMode)
            {
                baseComponent.EditorResourceHelper = this;
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (m_LoadSceneInfos.Count <= 0)
            {
                return;
            }

            LoadSceneInfo[] loadSceneInfos = m_LoadSceneInfos.ToArray();
            for (int i = 0; i < loadSceneInfos.Length; i++)
            {
                if (loadSceneInfos[i].AsyncOperation.isDone)
                {
                    if (loadSceneInfos[i].AsyncOperation.allowSceneActivation)
                    {
                        if (loadSceneInfos[i].LoadSceneCallbacks.LoadSceneSuccessCallback != null)
                        {
                            loadSceneInfos[i].LoadSceneCallbacks.LoadSceneSuccessCallback(loadSceneInfos[i].SceneName, loadSceneInfos[i].SceneAssetName, (float)(DateTime.Now - loadSceneInfos[i].StartTime).TotalSeconds, loadSceneInfos[i].UserData);
                        }
                    }
                    else
                    {
                        if (loadSceneInfos[i].LoadSceneCallbacks.LoadSceneFailureCallback != null)
                        {
                            loadSceneInfos[i].LoadSceneCallbacks.LoadSceneFailureCallback(loadSceneInfos[i].SceneName, loadSceneInfos[i].SceneAssetName, LoadResourceStatus.NotExist, "Can not load this scene from asset database.", loadSceneInfos[i].UserData);
                        }
                    }

                    m_LoadSceneInfos.Remove(loadSceneInfos[i]);
                }
                else
                {
                    if (loadSceneInfos[i].LoadSceneCallbacks.LoadSceneUpdateCallback != null)
                    {
                        loadSceneInfos[i].LoadSceneCallbacks.LoadSceneUpdateCallback(loadSceneInfos[i].SceneName, loadSceneInfos[i].SceneAssetName, loadSceneInfos[i].AsyncOperation.progress, loadSceneInfos[i].UserData);
                    }
                }
            }
        }

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath
        {
            get
            {
                return m_ReadOnlyPath;
            }
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath
        {
            get
            {
                return m_ReadWritePath;
            }
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        public ResourceMode ResourceMode
        {
            get
            {
                return ResourceMode.Unspecified;
            }
        }

        /// <summary>
        /// 获取当前变体。
        /// </summary>
        public string CurrentVariant
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion
        {
            get
            {
                throw new NotSupportedException("ApplicableGameVersion");
            }
        }

        /// <summary>
        /// 获取当前资源内部版本号。
        /// </summary>
        public int InternalResourceVersion
        {
            get
            {
                throw new NotSupportedException("InternalResourceVersion");
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int AssetCount
        {
            get
            {
                throw new NotSupportedException("AssetCount");
            }
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        public int ResourceCount
        {
            get
            {
                throw new NotSupportedException("ResourceCount");
            }
        }

        /// <summary>
        /// 获取资源组个数。
        /// </summary>
        public int ResourceGroupCount
        {
            get
            {
                throw new NotSupportedException("ResourceGroupCount");
            }
        }

        /// <summary>
        /// 获取或设置资源更新下载地址。
        /// </summary>
        public string UpdatePrefixUri
        {
            get
            {
                throw new NotSupportedException("UpdatePrefixUri");
            }
            set
            {
                throw new NotSupportedException("UpdatePrefixUri");
            }
        }

        /// <summary>
        /// 获取或设置资源更新重试次数。
        /// </summary>
        public int UpdateRetryCount
        {
            get
            {
                throw new NotSupportedException("UpdateRetryCount");
            }
            set
            {
                throw new NotSupportedException("UpdateRetryCount");
            }
        }

        /// <summary>
        /// 获取等待更新资源个数。
        /// </summary>
        public int UpdateWaitingCount
        {
            get
            {
                throw new NotSupportedException("UpdateWaitingCount");
            }
        }

        /// <summary>
        /// 获取正在更新资源个数。
        /// </summary>
        public int UpdatingCount
        {
            get
            {
                throw new NotSupportedException("UpdatingCount");
            }
        }

        /// <summary>
        /// 获取加载资源代理总个数。
        /// </summary>
        public int LoadTotalAgentCount
        {
            get
            {
                throw new NotSupportedException("LoadTotalAgentCount");
            }
        }

        /// <summary>
        /// 获取可用加载资源代理个数。
        /// </summary>
        public int LoadFreeAgentCount
        {
            get
            {
                throw new NotSupportedException("LoadFreeAgentCount");
            }
        }

        /// <summary>
        /// 获取工作中加载资源代理个数。
        /// </summary>
        public int LoadWorkingAgentCount
        {
            get
            {
                throw new NotSupportedException("LoadWorkingAgentCount");
            }
        }

        /// <summary>
        /// 获取等待加载资源任务个数。
        /// </summary>
        public int LoadWaitingTaskCount
        {
            get
            {
                throw new NotSupportedException("LoadWaitingTaskCount");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                throw new NotSupportedException("AssetAutoReleaseInterval");
            }
            set
            {
                throw new NotSupportedException("AssetAutoReleaseInterval");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                throw new NotSupportedException("AssetCapacity");
            }
            set
            {
                throw new NotSupportedException("AssetCapacity");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                throw new NotSupportedException("AssetExpireTime");
            }
            set
            {
                throw new NotSupportedException("AssetExpireTime");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                throw new NotSupportedException("AssetPriority");
            }
            set
            {
                throw new NotSupportedException("AssetPriority");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float ResourceAutoReleaseInterval
        {
            get
            {
                throw new NotSupportedException("ResourceAutoReleaseInterval");
            }
            set
            {
                throw new NotSupportedException("ResourceAutoReleaseInterval");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int ResourceCapacity
        {
            get
            {
                throw new NotSupportedException("ResourceCapacity");
            }
            set
            {
                throw new NotSupportedException("ResourceCapacity");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float ResourceExpireTime
        {
            get
            {
                throw new NotSupportedException("ResourceExpireTime");
            }
            set
            {
                throw new NotSupportedException("ResourceExpireTime");
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int ResourcePriority
        {
            get
            {
                throw new NotSupportedException("ResourcePriority");
            }
            set
            {
                throw new NotSupportedException("ResourcePriority");
            }
        }

#pragma warning disable 0414

        /// <summary>
        /// 资源初始化完成事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceInitCompleteEventArgs> ResourceInitComplete = null;

        /// <summary>
        /// 版本资源列表更新成功事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.VersionListUpdateSuccessEventArgs> VersionListUpdateSuccess = null;

        /// <summary>
        /// 版本资源列表更新失败事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.VersionListUpdateFailureEventArgs> VersionListUpdateFailure = null;

        /// <summary>
        /// 资源检查完成事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceCheckCompleteEventArgs> ResourceCheckComplete = null;

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceUpdateStartEventArgs> ResourceUpdateStart = null;

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceUpdateChangedEventArgs> ResourceUpdateChanged = null;

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess = null;

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceUpdateFailureEventArgs> ResourceUpdateFailure = null;

        /// <summary>
        /// 资源更新全部完成事件。
        /// </summary>
        public event EventHandler<GameFramework.Resource.ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete = null;

#pragma warning restore 0414

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        public void SetReadOnlyPath(string readOnlyPath)
        {
            if (string.IsNullOrEmpty(readOnlyPath))
            {
                throw new GameFrameworkException("Readonly path is invalid.");
            }

            m_ReadOnlyPath = readOnlyPath;
        }

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        public void SetReadWritePath(string readWritePath)
        {
            if (string.IsNullOrEmpty(readWritePath))
            {
                throw new GameFrameworkException("Read-write path is invalid.");
            }

            m_ReadWritePath = readWritePath;
        }

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        public void SetResourceMode(ResourceMode resourceMode)
        {
            throw new NotSupportedException("SetResourceMode");
        }

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        /// <param name="currentVariant">当前变体。</param>
        public void SetCurrentVariant(string currentVariant)
        {
            throw new NotSupportedException("SetCurrentVariant");
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            throw new NotSupportedException("SetObjectPoolManager");
        }

        /// <summary>
        /// 设置下载管理器。
        /// </summary>
        /// <param name="downloadManager">下载管理器。</param>
        public void SetDownloadManager(IDownloadManager downloadManager)
        {
            throw new NotSupportedException("SetDownloadManager");
        }

        /// <summary>
        /// 设置解密资源回调函数。
        /// </summary>
        /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
        /// <remarks>如果不设置，将使用默认的解密资源回调函数。</remarks>
        public void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback)
        {
            throw new NotSupportedException("SetDecryptResourceCallback");
        }

        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            throw new NotSupportedException("SetResourceHelper");
        }

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            throw new NotSupportedException("AddLoadResourceAgentHelper");
        }

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        public void InitResources()
        {
            throw new NotSupportedException("InitResources");
        }

        /// <summary>
        /// 检查版本资源列表。
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的资源内部版本号。</param>
        /// <returns>检查版本资源列表结果。</returns>
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            throw new NotSupportedException("CheckVersionList");
        }

        /// <summary>
        /// 更新版本资源列表。
        /// </summary>
        /// <param name="versionListLength">版本资源列表大小。</param>
        /// <param name="versionListHashCode">版本资源列表哈希值。</param>
        /// <param name="versionListZipLength">版本资源列表压缩后大小。</param>
        /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值。</param>
        public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode)
        {
            throw new NotSupportedException("UpdateVersionList");
        }

        /// <summary>
        /// 检查资源。
        /// </summary>
        public void CheckResources()
        {
            throw new NotSupportedException("CheckResources");
        }

        /// <summary>
        /// 更新资源。
        /// </summary>
        public void UpdateResources()
        {
            throw new NotSupportedException("UpdateResources");
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            LoadAsset(assetName, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

#if UNITY_EDITOR
            DateTime startTime = DateTime.Now;
            UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(assetName);
            if (asset != null)
            {
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, asset, (float)(DateTime.Now - startTime).TotalSeconds, userData);
                }

                return;
            }

            if (loadAssetCallbacks.LoadAssetFailureCallback != null)
            {
                loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotExist, "Can not load this asset from asset database.", userData);
            }
#endif
        }

        /// <summary>
        /// 异步实例化资源。
        /// </summary>
        /// <param name="assetName">要实例化资源的名称。</param>
        /// <param name="instantiateAssetCallbacks">实例化资源回调函数集。</param>
        public void InstantiateAsset(string assetName, InstantiateAssetCallbacks instantiateAssetCallbacks)
        {
            InstantiateAsset(assetName, instantiateAssetCallbacks, null);
        }

        /// <summary>
        /// 异步实例化资源。
        /// </summary>
        /// <param name="assetName">要实例化资源的名称。</param>
        /// <param name="instantiateAssetCallbacks">实例化资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void InstantiateAsset(string assetName, InstantiateAssetCallbacks instantiateAssetCallbacks, object userData)
        {
            if (instantiateAssetCallbacks == null)
            {
                throw new GameFrameworkException("Instantiate asset callbacks is invalid.");
            }

#if UNITY_EDITOR
            DateTime startTime = DateTime.Now;
            UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(assetName);
            if (asset != null)
            {
                UnityEngine.Object instance = Instantiate(asset);
                if (instance != null)
                {
                    if (instantiateAssetCallbacks.InstantiateAssetSuccessCallback != null)
                    {
                        instantiateAssetCallbacks.InstantiateAssetSuccessCallback(assetName, instance, (float)(DateTime.Now - startTime).TotalSeconds, userData);
                    }

                    return;
                }
            }

            if (instantiateAssetCallbacks.InstantiateAssetFailureCallback != null)
            {
                instantiateAssetCallbacks.InstantiateAssetFailureCallback(assetName, LoadResourceStatus.NotExist, "Can not instantiate this asset from asset database.", userData);
            }
#endif
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneName">要加载的场景名称。</param>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneName, string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks)
        {
            LoadScene(sceneName, sceneAssetName, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneName">要加载的场景名称。</param>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneName, string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            if (loadSceneCallbacks == null)
            {
                throw new GameFrameworkException("Load scene callbacks is invalid.");
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameFrameworkException("Scene name is invalid.");
            }

            DateTime startTime = DateTime.Now;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (asyncOperation == null)
            {
                return;
            }

            m_LoadSceneInfos.Add(new LoadSceneInfo(asyncOperation, sceneName, sceneAssetName, startTime, loadSceneCallbacks, userData));
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneName">场景名称。</param>
        /// <returns>是否成功卸载场景。</returns>
        public bool UnloadScene(string sceneName)
        {
            return SceneManager.UnloadScene(sceneName);
        }

        /// <summary>
        /// 回收资源或资源实例。
        /// </summary>
        /// <param name="assetOrInstance">要回收的资源或资源实例。</param>
        public void Recycle(object assetOrInstance)
        {
            // Do nothing in editor resource mode.
        }

        /// <summary>
        /// 获取资源组是否准备完毕。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public bool GetResourceGroupReady(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupReady");
        }

        /// <summary>
        /// 获取资源组资源个数。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupResourceCount(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupResourceCount");
        }

        /// <summary>
        /// 获取资源组已准备完成资源个数。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupReadyResourceCount(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupReadyResourceCount");
        }

        /// <summary>
        /// 获取资源组总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalLength(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupTotalLength");
        }

        /// <summary>
        /// 获取资源组已准备完成总大小。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public int GetResourceGroupTotalReadyLength(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupTotalReadyLength");
        }

        /// <summary>
        /// 获取资源组准备进度。
        /// </summary>
        /// <param name="resourceGroupName">要检查的资源组名称。</param>
        public float GetResourceGroupProgress(string resourceGroupName)
        {
            throw new NotSupportedException("GetResourceGroupProgress");
        }

        private class LoadSceneInfo
        {
            private readonly AsyncOperation m_AsyncOperation;
            private readonly string m_SceneName;
            private readonly string m_SceneAssetName;
            private readonly DateTime m_StartTime;
            private readonly LoadSceneCallbacks m_LoadSceneCallbacks;
            private readonly object m_UserData;

            public LoadSceneInfo(AsyncOperation asyncOperation, string sceneName, string sceneAssetName, DateTime startTime, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                m_AsyncOperation = asyncOperation;
                m_SceneName = sceneName;
                m_SceneAssetName = sceneAssetName;
                m_StartTime = startTime;
                m_LoadSceneCallbacks = loadSceneCallbacks;
                m_UserData = userData;
            }

            public AsyncOperation AsyncOperation
            {
                get
                {
                    return m_AsyncOperation;
                }
            }

            public string SceneName
            {
                get
                {
                    return m_SceneName;
                }
            }

            public string SceneAssetName
            {
                get
                {
                    return m_SceneAssetName;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_StartTime;
                }
            }

            public LoadSceneCallbacks LoadSceneCallbacks
            {
                get
                {
                    return m_LoadSceneCallbacks;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}
