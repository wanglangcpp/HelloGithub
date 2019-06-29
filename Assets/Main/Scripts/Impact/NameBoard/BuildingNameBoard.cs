using UnityEngine;

namespace Genesis.GameClient
{
    public class BuildingNameBoard : BaseNameBoard
    {
        protected override TargetType NameBoardType
        {
            get
            {
                return TargetType.Building;
            }
        }

        public override void Init(NameBoard nameBoard)
        {
            base.Init(nameBoard);
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            base.RefreshNameBoard(entity, mode);

            if (m_NameBoard != null)
            {
                Building building = entity as Building;
                SetName(GameEntry.Localization.GetString(building.Data.Name));
                m_NameBoard.NameLabel.gameObject.SetActive(building.ShowNameBoard);
                var hpBars = m_NameBoard.HpBars;
                var elements = m_NameBoard.Elements;
                for (int i = 0; i < (int)TargetType.Count; i++)
                {
                    hpBars[i].gameObject.SetActive(building.HpBarRule != HPBarDisplayRule.DontDisplay && i == (int)NameBoardType);
                    elements[i].gameObject.SetActive(false);
                }
                StartTime = Time.time;
            }
        }
    }
}
