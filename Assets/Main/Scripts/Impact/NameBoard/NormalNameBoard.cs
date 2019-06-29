using UnityEngine;

namespace Genesis.GameClient
{
    public class NormalNameBoard : BaseNameBoard
    {
        protected override TargetType NameBoardType
        {
            get
            {
                return TargetType.Normal;
            }
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            base.RefreshNameBoard(entity, mode);
            TargetableObject targetableObject = entity as TargetableObject;
            if (m_NameBoard != null && targetableObject != null)
            {
                if (GameEntry.SceneLogic.BaseInstanceLogic.Type == InstanceLogicType.MimicMelee)
                {
                    m_NameBoard.NameLabel.gameObject.SetActive(Character.Data.ShowName);
                    SetName(Character.Data.Name);
                }
                if (targetableObject.Camp == CampType.PlayerFriend)
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
