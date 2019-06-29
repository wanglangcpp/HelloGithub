using GameFramework.Event;
using GameFramework.Resource;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载并实例化界面实例失败事件。
    /// </summary>
    public class LoadAndInstantiateUIInstanceFailureEventArgs : GameEventArgs
    {
        public LoadAndInstantiateUIInstanceFailureEventArgs(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            AssetName = assetName;
            Status = status;
            ErrorMessage = errorMessage;
            UserData = userData;
        }

        public string AssetName { get; private set; }
        public string ErrorMessage { get; private set; }
        public LoadResourceStatus Status { get; private set; }
        public object UserData { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.LoadAndInstantiateUIInstanceFailure;
            }
        }
    }
}
