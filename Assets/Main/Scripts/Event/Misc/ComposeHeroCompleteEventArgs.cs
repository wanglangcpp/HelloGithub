using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 合成英雄成功事件。
    /// </summary>
    public class ComposeHeroCompleteEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)(EventId.ComposeHeroComplete);
            }
        }

        public int HeroType { get; private set; }

        public ComposeHeroCompleteEventArgs(int heroType)
        {
            HeroType = heroType;
        }
    }
}
