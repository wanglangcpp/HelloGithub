using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 角色死亡事件。
    /// </summary>
    public class CharacterDeadEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化角色死亡事件的新实例。
        /// </summary>
        /// <param name="deadCharacterData">死亡的角色的数据。</param>
        /// <param name="deadCharacter">死亡的角色。</param>
        /// <param name="deadlyImpactSourceEntity">致死伤害来源实体。</param>
        /// <param name="deadlyImpactSourceType">致死伤害来源类型。</param>
        public CharacterDeadEventArgs(CharacterData deadCharacterData, Character deadCharacter, Entity deadlyImpactSourceEntity, ImpactSourceType deadlyImpactSourceType)
        {
            CharacterData = deadCharacterData;
            Character = deadCharacter;
            DeadlyImpactSourceEntity = deadlyImpactSourceEntity;
            DeadlyImpactSourceType = deadlyImpactSourceType;
        }

        /// <summary>
        /// 获取角色死亡事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.CharacterDead;
            }
        }

        /// <summary>
        /// 获取死亡角色的数据。
        /// </summary>
        public CharacterData CharacterData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡角色。
        /// </summary>
        public Character Character
        {
            get;
            private set;
        }

        /// <summary>
        /// 致死伤害来源实体。
        /// </summary>
        public Entity DeadlyImpactSourceEntity
        {
            get;
            private set;
        }

        /// <summary>
        /// 致死伤害来源类型。
        /// </summary>
        public ImpactSourceType DeadlyImpactSourceType
        {
            get;
            private set;
        }
    }
}
