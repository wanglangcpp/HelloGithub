using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 切换英雄开始事件。
    /// </summary>
    public class SwitchHeroStartEventArgs : GameEventArgs
    {
        public SwitchHeroStartEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SwitchHeroStart;
            }
        }

        public int PlayerId { get; private set; }

        public bool IsMe
        {
            get
            {
                if (!GameEntry.IsAvailable)
                {
                    return false;
                }

                return GameEntry.Data.Player.Id == PlayerId;
            }
        }
    }
}
