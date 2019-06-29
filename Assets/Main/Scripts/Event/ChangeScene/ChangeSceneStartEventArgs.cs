using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 切换场景开始事件。
    /// </summary>
    public class ChangeSceneStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化切换场景开始事件的新实例。
        /// </summary>
        /// <param name="instanceLogicType">副本逻辑类型。</param>
        public ChangeSceneStartEventArgs(InstanceLogicType instanceLogicType)
        {
            InstanceLogicType = instanceLogicType;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChangeSceneStart;
            }
        }

        public InstanceLogicType InstanceLogicType
        {
            get;
            private set;
        }
    }
}
