using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 贴图组件。
    /// </summary>
    public partial class TextureComponent : MonoBehaviour
    {
        [SerializeField]
        private int m_ResourcePoolCapacity = 5;

        [SerializeField]
        private float m_ResourceExpireTime = 30f;

        [SerializeField]
        private int m_ResourcePriority = 0;

        private IObjectPool<TextureObject> m_TextureObjectPool;
        private LoadAssetCallbacks m_LoadAssetCallbacks = null;

        private void Awake()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadResourceSuccessCallback, LoadResourceFailureCallback);
        }

        private void Start()
        {
            m_TextureObjectPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<TextureObject>("Texture", m_ResourcePoolCapacity, m_ResourceExpireTime, m_ResourcePriority);
        }

        /// <summary>
        /// 加载贴图。
        /// </summary>
        /// <param name="texturePath">贴图路径。</param>
        /// <param name="loadTextureSuccessCallback">加载贴图成功回调。</param>
        /// <param name="loadTextureFailureCallback">加载贴图失败回调。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadTexture(string texturePath, GameFrameworkAction<string, object, object> loadTextureSuccessCallback, GameFrameworkAction<string, string, object> loadTextureFailureCallback = null, object userData = null)
        {
            string assetName = AssetUtility.GetTextureAsset(texturePath);
            LoadTextureData loadTextureData = new LoadTextureData(loadTextureSuccessCallback, loadTextureFailureCallback, userData);
            var resourceObj = m_TextureObjectPool.Spawn(assetName);
            if (resourceObj == null)
            {
                GameEntry.Resource.LoadAsset(assetName, m_LoadAssetCallbacks, loadTextureData);
                return;
            }

            TackleResource(assetName, resourceObj.Target, loadTextureData);
        }

        public void UnloadTexture(Texture texture)
        {
            if (!GameEntry.IsAvailable || texture == null)
            {
                return;
            }

            Log.Info("Unload texture '{0}'.", texture.name);
            m_TextureObjectPool.Unspawn(texture);
        }

        private void TackleResource(string assetName, object resource, object userData)
        {
            var loadTextureData = userData as LoadTextureData;
            if (loadTextureData == null)
            {
                return;
            }

            var onSuccess = loadTextureData.LoadTextureSuccessCallback;
            if (onSuccess != null)
            {
                onSuccess(assetName, resource, loadTextureData.UserData);
            }
        }

        private void LoadResourceSuccessCallback(string assetName, object resource, float duration, object userData)
        {
            var resourceObj = m_TextureObjectPool.Spawn(assetName);
            if (resourceObj == null)
            {
                resourceObj = new TextureObject(assetName, resource);
                m_TextureObjectPool.Register(resourceObj, true);
            }

            TackleResource(assetName, resource, userData);
        }

        private void LoadResourceFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Warning("Load texture failure, assetName name '{0}', status '{1}', error message '{2}'.", assetName, status.ToString(), errorMessage);
            var loadTextureData = userData as LoadTextureData;
            if (loadTextureData == null)
            {
                return;
            }

            var onFailure = loadTextureData.LoadTextureFailureCallback;
            if (onFailure != null)
            {
                onFailure(assetName, errorMessage, loadTextureData.UserData);
            }
        }
    }
}
