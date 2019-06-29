using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///技能徽章背包数据变化。
    /// </summary>
    public class OnSkillBadgeBagDataChangedEventArgs : GameEventArgs
    {
        public OnSkillBadgeBagDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.OnSkillBadgeBagDataChanged;
            }
        }
    }
}
