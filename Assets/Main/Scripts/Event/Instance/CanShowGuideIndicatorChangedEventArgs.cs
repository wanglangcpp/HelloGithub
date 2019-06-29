using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本中为玩家英雄显示向导提示标志的 flag 发生改变的事件。
    /// </summary>
    public class CanShowGuideIndicatorChangedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.CanShowGuideIndicatorChanged;
            }
        }
    }
}
