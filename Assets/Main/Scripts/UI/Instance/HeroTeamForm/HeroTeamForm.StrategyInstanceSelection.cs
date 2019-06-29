namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private class StrategyInstanceSelection : StrategyBase
        {
            public override void Init(HeroTeamForm form)
            {
                base.Init(form);
                m_Form.SetInstanceId();
            }

            public override void OnHeroTeamDataChanged()
            {
                if (m_Form.m_InstanceType == InstanceLogicType.SinglePlayer)
                {
                    base.OnHeroTeamDataChanged();
                }
            }
        }
    }
}
