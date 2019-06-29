using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄升品道具数据变化事件。
    /// </summary>
    public class HeroQualityItemDataChangeEventArgs : GameEventArgs
    {
        public HeroQualityItemDataChangeEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.HeroQualityItemDataChange;
            }
        }
    }
}
