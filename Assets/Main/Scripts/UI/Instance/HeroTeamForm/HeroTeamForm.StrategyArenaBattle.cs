using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private class StrategyArenaBattle : StrategyBase
        {
            public override HeroTeamType? HeroTeamInfoType
            {
                get
                {
                    return HeroTeamType.Arena;
                }
            }

            public override IList<int> HeroTeam
            {
                get
                {
                    return GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType;
                }
            }

            public override void Init(HeroTeamForm form)
            {
                base.Init(form);
                var title = m_Form.GetComponent<UITitle>();
                title.SetTitle("UI_TEXT_OFFLINE_PVP_TEAM");
            }
        }
    }
}
