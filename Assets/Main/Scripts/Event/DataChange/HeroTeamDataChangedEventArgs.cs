using GameFramework.Event;

namespace Genesis.GameClient
{
    public class HeroTeamDataChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 若参数是HeroTeamCount，则所有阵容都改变
        /// </summary>
        /// <param name="teamType"></param>
        public HeroTeamDataChangedEventArgs(HeroTeamType teamType)
        {
            TeamType = teamType;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.HeroTeamDataChanged;
            }
        }

        public HeroTeamType TeamType { get; set; }
    }
}
