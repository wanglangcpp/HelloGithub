using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载预加载资源成功事件。
    /// </summary>
    public class LoadPreloadResourceSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载预加载资源成功事件的新实例。
        /// </summary>
        /// <param name="name">预加载资源的名称。</param>
        /// <param name="assetName">预加载资源的名称。</param>
        /// <param name="resource">已加载的资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadPreloadResourceSuccessEventArgs(string name, string assetName, object resource, object userData)
        {
            Name = name;
            AssetName = assetName;
            Resource = resource;
            UserData = userData;
        }

        /// <summary>
        /// 获取加载预加载资源成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadPreloadResourceSuccess;
            }
        }

        /// <summary>
        /// 获取预加载资源的名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取预加载资源的名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取时已加载的资源。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
