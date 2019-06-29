using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝 -- 数据改变事件。
    /// </summary>
    public class CosmosCrackDataChanged : GameEventArgs
    {
        public CosmosCrackDataChanged()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.CosmosCrackDataChanged;
            }
        }
    }
}
