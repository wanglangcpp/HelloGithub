using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 切换英雄完毕事件。
    /// </summary>
    public class SwitchHeroCompleteEventArgs : GameEventArgs
    {
        public SwitchHeroCompleteEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SwitchHeroComplete;
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
