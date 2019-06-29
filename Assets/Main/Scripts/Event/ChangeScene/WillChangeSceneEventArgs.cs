using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 即将切换场景事件。
    /// </summary>
    public class WillChangeSceneEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化即将切换场景事件的新实例。
        /// </summary>
        /// <param name="instanceLogicType">副本逻辑类型。</param>
        /// <param name="sceneOrInstanceId">场景或副本编号。</param>
        /// <param name="autoHideLoading">是否自动隐藏加载界面。</param>
        /// <param name="userData">用户数据</param>
        public WillChangeSceneEventArgs(InstanceLogicType instanceLogicType, int sceneOrInstanceId, bool autoHideLoading, object userData = null)
        {
            InstanceLogicType = instanceLogicType;
            SceneOrInstanceId = sceneOrInstanceId;
            AutoHideLoading = autoHideLoading;
            UserData = userData;
        }

        /// <summary>
        /// 获取即将切换场景事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.WillChangeScene;
            }
        }

        /// <summary>
        /// 副本逻辑类型
        /// </summary>
        public InstanceLogicType InstanceLogicType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景或副本编号。
        /// </summary>
        public int SceneOrInstanceId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否自动隐藏加载界面。
        /// </summary>
        public bool AutoHideLoading
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
