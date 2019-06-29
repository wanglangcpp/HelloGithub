using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ShowBossEventArgs : GameEventArgs
    {
        public ShowBossEventArgs(TargetableObjectData bossData, int bossHPBarCount)
        {
            BossData = bossData;
            BossHPBarCount = bossHPBarCount;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ShowBoss;
            }
        }

        public TargetableObjectData BossData
        {
            get;
            private set;
        }

        public int BossHPBarCount
        {
            get;
            private set;
        }
    }
}
