using UnityEngine;

namespace Genesis.GameClient
{
    public class EliteNameBoard : BaseNameBoard
    {
        protected override TargetType NameBoardType
        {
            get
            {
                return TargetType.Elite;
            }
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            base.RefreshNameBoard(entity, mode);
            TargetableObject targetableObject = entity as TargetableObject;
            if (m_NameBoard != null && targetableObject != null)
            {
                if (targetableObject.Camp == CampType.Enemy || targetableObject.Camp == CampType.Enemy2)
                {
                    SetHPBarColor(m_HPColorNames["Yellow"]);
                }
                else if (targetableObject.Camp == CampType.PlayerFriend)
                {
                    SetHPBarColor(m_HPColorNames["Green"]);
                }
                else
                {
                    SetHPBarColor(m_HPColorNames["Red"]);
                }
                StartTime = Time.time;
            }
        }
    }
}
