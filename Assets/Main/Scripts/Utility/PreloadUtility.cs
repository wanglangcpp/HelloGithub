using GameFramework;
using GameFramework.Resource;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 预加载资源相关工具类。
    /// </summary>
    public static class PreloadUtility
    {
        /// <summary>
        /// 加载预加载资源。
        /// </summary>
        /// <param name="name">资源在预加载资源包中的名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public static void LoadPreloadResource(string name, object userData = null)
        {
            Dictionary<string, object> internalUserData = new Dictionary<string, object> { { "Name", name }, { "UserData", userData } };
            GameEntry.Resource.LoadAsset(string.Format("Assets/Main/Prefabs/UI/Preload/{0}.prefab", name), new LoadAssetCallbacks(LoadResourceSuccessCallback, LoadResourceFailureCallback), internalUserData);
        }

        private static void LoadResourceSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            Dictionary<string, object> hashtable = userData as Dictionary<string, object>;
            GameEntry.Event.Fire(null, new LoadPreloadResourceSuccessEventArgs(hashtable["Name"] as string, assetName, asset, hashtable["UserData"]));
        }

        private static void LoadResourceFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Dictionary<string, object> hashtable = userData as Dictionary<string, object>;
            string name = hashtable["Name"] as string;
            Log.Warning("Load preload '{0}' failed, status '{1}', error message '{2}'.", name, status.ToString(), errorMessage);
            GameEntry.Event.Fire(null, new LoadPreloadResourceFailureEventArgs(name, assetName, errorMessage, hashtable["UserData"]));
        }
    }
}
