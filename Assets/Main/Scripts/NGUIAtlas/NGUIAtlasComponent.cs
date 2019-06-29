using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// NGUI 图集组件。独立于 <see cref="UnityGameFramework.UIComponent"/> 而存在。
    /// </summary>
    public partial class NGUIAtlasComponent : MonoBehaviour
    {
        [SerializeField]
        private int m_ResourcePoolCapacity = 16;

        [SerializeField]
        private float m_ResourceExpireTime = 30f;

        [SerializeField]
        private int m_ResourcePriority = 0;

        private IObjectPool<NGUIAtlasObject> m_ResourcePool = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks = null;

        private void Awake()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadResourceSuccess, OnLoadResourceFailure);
        }

        private void Start()
        {
            m_ResourcePool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<NGUIAtlasObject>("NGUIAtlas", m_ResourcePoolCapacity, m_ResourceExpireTime, m_ResourcePriority);
        }

        /// <summary>
        /// 加载图集
        /// </summary>
        /// <param name="atlasPath">图集路径。</param>
        /// <param name="userData">用户数据。</param>
        public void LoadAtlas(string atlasPath, object userData = null)
        {
            string assetName = AssetUtility.GetAtlasAsset(atlasPath);
            var resourceObj = m_ResourcePool.Spawn(assetName);
            if (resourceObj == null)
            {
                GameEntry.Resource.LoadAsset(assetName, m_LoadAssetCallbacks, userData);
                return;
            }

            TackleResource(assetName, resourceObj.Target, userData);
        }

        public void UnloadAtlas(GameObject uiAtlasPrefab)
        {
            if (!GameEntry.IsAvailable || uiAtlasPrefab == null)
            {
                return;
            }

            Log.Info("Unload atlas '{0}'.", uiAtlasPrefab.name);
            m_ResourcePool.Unspawn(uiAtlasPrefab);
        }

        private void OnLoadResourceSuccess(string assetName, object asset, float duration, object userData)
        {
            var resourceObj = m_ResourcePool.Spawn(assetName);
            if (resourceObj == null)
            {
                resourceObj = new NGUIAtlasObject(assetName, asset);
                m_ResourcePool.Register(resourceObj, true);
            }

            TackleResource(assetName, asset, userData);
        }

        private void TackleResource(string assetName, object asset, object userData)
        {
            var loadAtlasData = userData as LoadAtlasData;
            if (loadAtlasData == null)
            {
                return;
            }

            var onSuccess = loadAtlasData.OnLoadAtlasSuccess;
            if (onSuccess != null)
            {
                onSuccess(assetName, asset, loadAtlasData.UserData);
            }
        }

        private void OnLoadResourceFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            var loadAtlasData = userData as LoadAtlasData;
            if (loadAtlasData == null)
            {
                return;
            }

            var onFailure = loadAtlasData.OnLoadAtlasFailure;
            if (onFailure != null)
            {
                onFailure(assetName, errorMessage, loadAtlasData.UserData);
            }
        }
    }
}
