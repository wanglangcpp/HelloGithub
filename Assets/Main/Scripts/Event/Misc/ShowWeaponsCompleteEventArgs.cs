using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 武器显示完成事件。。
    /// </summary>
    public class ShowWeaponsCompleteEventArgs : GameEventArgs
    {
        public ShowWeaponsCompleteEventArgs(HeroData heroData)
        {
            HeroData = heroData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ShowWeaponsComplete;
            }
        }

        public HeroData HeroData
        {
            get; private set;
        }
    }
}
