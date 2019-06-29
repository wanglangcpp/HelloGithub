using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 行为组件。
    /// </summary>
    public partial class BehaviorComponent : MonoBehaviour
    {
        [SerializeField]
        private int m_ResourcePoolCapacity = 16;

        private IObjectPool<BehaviorObject> m_BehaviorObjectPool = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks = null;

        private void Awake()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadBehaviorSuccessHandler, LoadBehaviorFailureHandler);
        }

        private void Start()
        {
            m_BehaviorObjectPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<BehaviorObject>("Behavior", m_ResourcePoolCapacity);
            Log.Info("Behavior component has been initialized.");
        }

        public void LoadBehavior(BehaviorTree behavior, string behaviorName, object userData = null)
        {
            if (string.IsNullOrEmpty(behaviorName))
            {
                Log.Error("Behavior name is invalid.");
                return;
            }

            string assetName = AssetUtility.GetBehaviorAsset(behaviorName);
            Dictionary<string, object> internalUserData = new Dictionary<string, object> { { "Behavior", behavior }, { "BehaviorName", behaviorName }, { "UserData", userData } };
            var resourceObj = m_BehaviorObjectPool.Spawn(assetName);
            if (resourceObj == null)
            {
                GameEntry.Resource.LoadAsset(assetName, m_LoadAssetCallbacks, internalUserData);
                return;
            }

            TackleResource(assetName, resourceObj.Target, internalUserData);
        }

        public void UnloadBehavior(ExternalBehavior behavior)
        {
            if (!GameEntry.IsAvailable || behavior == null)
            {
                return;
            }

            Log.Info("Unload behavior '{0}'.", behavior.name);
            m_BehaviorObjectPool.Unspawn(behavior);
        }

        private void TackleResource(string assetName, object asset, object userData)
        {
            var loadTextureData = userData as Dictionary<string, object>;
            if (loadTextureData == null)
            {
                return;
            }

            Dictionary<string, object> userDataDict = userData as Dictionary<string, object>;
            BehaviorTree behavior = userDataDict["Behavior"] as BehaviorTree;
            behavior.BehaviorName = userDataDict["BehaviorName"] as string;
            behavior.ExternalBehavior = asset as ExternalBehavior;

            if (behavior.ExternalBehavior == null)
            {
                GameEntry.Event.Fire(this, new LoadBehaviorFailureEventArgs(behavior, behavior.BehaviorName, assetName, "Can not load behavior.", userDataDict["UserData"]));
                return;
            }

            GameEntry.Event.Fire(this, new LoadBehaviorSuccessEventArgs(behavior, behavior.BehaviorName, assetName, userDataDict["UserData"]));
        }

        private void LoadBehaviorSuccessHandler(string assetName, object asset, float duration, object userData)
        {
            var resourceObj = m_BehaviorObjectPool.Spawn(assetName);
            if (resourceObj == null)
            {
                resourceObj = new BehaviorObject(assetName, asset);
                m_BehaviorObjectPool.Register(resourceObj, true);
            }

            TackleResource(assetName, asset, userData);
        }

        private void LoadBehaviorFailureHandler(string behaviorAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Dictionary<string, object> userDataDict = userData as Dictionary<string, object>;
            GameEntry.Event.Fire(this, new LoadBehaviorFailureEventArgs(userDataDict["Behavior"] as BehaviorTree, userDataDict["BehaviorName"] as string, behaviorAssetName, errorMessage, userDataDict["UserData"]));
        }
    }
}
