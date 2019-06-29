using GameFramework.Event;

namespace Genesis.GameClient
{
    public class HeroTeamDataChangedChessBattleEventArgs : GameEventArgs
    {
        public HeroTeamDataChangedChessBattleEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.HeroTeamDataChangedChessBattle;
            }
        }
    }
}
