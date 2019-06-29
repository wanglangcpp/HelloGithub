using GameFramework.Event;

namespace Genesis.GameClient
{
    class PlayerLevelUpAnimationEventArgs : GameEventArgs
    {
        public PlayerLevelUpAnimationEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.PlayerLevelUpAnimationCallBack;
            }
        }
    }
}
