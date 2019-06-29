using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 界面片段池组件。
    /// </summary>
    public class UIFragmentComponent : MonoBehaviour
    {
        [SerializeField]
        private int m_InstancePoolCapacity = 4;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        private IObjectPool<UIInstanceObject> m_InstancePool = null;
        private InstantiateAssetCallbacks m_InstantiateAssetCallbacks = null;

        public void LoadAndInstantiate(string prefabPath, object userData)
        {
            string assetName = AssetUtility.GetUIFormAsset(prefabPath);
            UIInstanceObject uiInstanceObj = m_InstancePool.Spawn(assetName);
            if (uiInstanceObj == null)
            {
                GameEntry.Resource.InstantiateAsset(assetName, m_InstantiateAssetCallbacks, userData);
                return;
            }

            GameEntry.Event.Fire(this, new LoadAndInstantiateUIInstanceSuccessEventArgs(assetName, uiInstanceObj.Target as GameObject, userData));
        }

        public void Recycle(GameObject go)
        {
            if (go == null)
            {
                Log.Error("Cannot recycle a null game object.");
                return;
            }

            var trans = go.transform;
            var cachedPos = trans.localPosition;
            var cachedRot = trans.localRotation;
            var cachedScale = trans.localScale;
            trans.parent = m_InstanceRoot;
            trans.localPosition = cachedPos;
            trans.localRotation = cachedRot;
            trans.localScale = cachedScale;
            go.SetActive(false);
            m_InstancePool.Unspawn(go);
        }

        private void OnLoadAndInstantiateSuccess(string assetName, object instance, float duration, object userData)
        {
            UIInstanceObject uiInstanceObj = new UIInstanceObject(assetName, instance);
            m_InstancePool.Register(uiInstanceObj, true);

            var go = uiInstanceObj.Target as GameObject;
            if (go == null)
            {
                Log.Error("Game Object is not found in instance cloned from '{0}'.", assetName);
                return;
            }

            go.SetActive(false);
            var trans = go.transform;
            var cachedPos = trans.localPosition;
            var cachedRot = trans.localRotation;
            var cachedScale = trans.localScale;
            trans.parent = m_InstanceRoot;
            trans.localPosition = cachedPos;
            trans.localRotation = cachedRot;
            trans.localScale = cachedScale;

            GameEntry.Event.Fire(this, new LoadAndInstantiateUIInstanceSuccessEventArgs(assetName, uiInstanceObj.Target as GameObject, userData));
        }

        private void OnLoadAndInstantiateFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            GameEntry.Event.Fire(this, new LoadAndInstantiateUIInstanceFailureEventArgs(assetName, status, errorMessage, userData));
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_InstantiateAssetCallbacks = new InstantiateAssetCallbacks(OnLoadAndInstantiateSuccess, OnLoadAndInstantiateFailure);
        }

        private void Start()
        {
            m_InstancePool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<UIInstanceObject>("UI Instance", m_InstancePoolCapacity);
        }

        #endregion MonoBehaviour

        private class UIInstanceObject : ObjectBase
        {
            public UIInstanceObject(string name, object target)
                : base(name, target)
            {

            }

            protected override void Release()
            {
                GameEntry.Resource.Recycle(Target);
                Log.Info("Recycle UI fragment '{0}'.", Name);
            }
        }
    }
}
