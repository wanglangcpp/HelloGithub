using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载并实例化界面实例成功事件。
    /// </summary>
    public class LoadAndInstantiateUIInstanceSuccessEventArgs : GameEventArgs
    {
        public LoadAndInstantiateUIInstanceSuccessEventArgs(string assetName, GameObject go, object userData)
        {
            AssetName = assetName;
            GameObject = go;
            UserData = userData;
        }

        public string AssetName { get; private set; }

        public GameObject GameObject { get; private set; }

        public object UserData { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.LoadAndInstantiateUIInstanceSuccess;
            }
        }
    }
}
