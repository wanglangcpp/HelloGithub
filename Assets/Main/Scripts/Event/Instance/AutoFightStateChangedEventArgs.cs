using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 自动战斗状态改变事件。
    /// </summary>
    public class AutoFightStateChangedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.AutoFightStateChanged;
            }
        }
    }
}
