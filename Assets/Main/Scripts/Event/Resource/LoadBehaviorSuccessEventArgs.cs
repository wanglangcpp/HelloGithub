using BehaviorDesigner.Runtime;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载行为成功事件。
    /// </summary>
    public class LoadBehaviorSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载行为成功事件的新实例。
        /// </summary>
        /// <param name="behavior">行为引用。</param>
        /// <param name="behaviorName">行为名称。</param>
        /// <param name="behaviorAssetName">行为资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadBehaviorSuccessEventArgs(BehaviorTree behavior, string behaviorName, string behaviorAssetName, object userData)
        {
            Behavior = behavior;
            BehaviorName = behaviorName;
            BehaviorAssetName = behaviorAssetName;
            UserData = userData;
        }

        /// <summary>
        /// 获取加载行为成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadBehaviorSuccess;
            }
        }

        /// <summary>
        /// 获取行为引用。
        /// </summary>
        public BehaviorTree Behavior
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行为名称。
        /// </summary>
        public string BehaviorName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行为资源名称。
        /// </summary>
        public string BehaviorAssetName
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
