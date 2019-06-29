using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 切换场景完成事件。
    /// </summary>
    public class ChangeSceneCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化切换场景完成事件的新实例。
        /// </summary>
        /// <param name="instanceLogicType">副本逻辑类型。</param>
        public ChangeSceneCompleteEventArgs(InstanceLogicType instanceLogicType)
        {
            InstanceLogicType = instanceLogicType;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChangeSceneComplete;
            }
        }

        public InstanceLogicType InstanceLogicType
        {
            get;
            private set;
        }
    }
}
