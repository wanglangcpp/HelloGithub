using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 复活事件。
    /// </summary>
    public class ReviveEventArgs : GameEventArgs
    {
        public ReviveEventArgs(HeroCharacter heroCharacter)
        {
            HeroCharacter = heroCharacter;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.Revive;
            }
        }

        public HeroCharacter HeroCharacter
        {
            get;
            private set;
        }
    }
}
