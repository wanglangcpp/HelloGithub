using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色属性变更事件。
    /// </summary>
    public class CharacterPropertyChangeEventArgs : GameEventArgs
    {
        public CharacterPropertyChangeEventArgs(int characterEntityId)
        {
            CharacterEntityId = characterEntityId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.CharacterPropertyChange;
            }
        }

        public int CharacterEntityId
        {
            get;
            private set;
        }
    }
}
